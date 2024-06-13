using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Resources;
using StaffManagmentNET.Responses;
using StaffManagmentNET.StaticServices;
using StaffManagmentNET.ViewModels;
using System.Globalization;

namespace StaffManagmentNET.Services
{
    public class RequestService : IRequestRepo
    {
        private readonly DataContext _context;

        public RequestService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RequestType>> GetAllRqst()
        {
            var request = await _context.Requests.ToListAsync();
            var result = new List<RequestType>();
            foreach (var req in  request)
            {
                if (req.RequestID == "1102") continue;

                result.Add(new RequestType
                {
                    RqstID = req.RequestID,
                    RqstType = req.RequestType,
                    RqstName = req.RequestName
                });
            }
            return result;
        }

        public async Task<IEnumerable<RequestCreate>> GetYourRequest(string staffID)
        {
            var requests = await _context.RequestCreateDetails.Where(r => r.StaffID == staffID).ToListAsync();
            var result = new List<RequestCreate>();

            foreach (var req in requests)
            {
                var request = await _context.Requests.FindAsync(req.RequestID);
                if (request == null)
                {
                    continue;
                }

                var activities = await GetActivities(req, request);
                var lastActivity = activities.Where(a => !a.Contains("has not")).LastOrDefault();

                result.Add(new RequestCreate
                {
                    CreateID = req.CreateID,
                    RequestID = req.RequestID,
                    RequestName = request.RequestName,
                    CreateTime = req.CreateTime.ToString(),
                    Status = req.Status,
                    StaffID = req.StaffID,
                    LastActivity = lastActivity!
                });
            }

            return result;
        }

        public async Task<IEnumerable<RequestCreate>> GetNeedToAcceptRequest(string managerID)
        {
            var needToAcpt = await _context.RequestAcceptDetails
                .Where(r => r.ManagerID == managerID)
                .ToListAsync();
            var manager = await _context.Staffs.FindAsync(managerID);
            var result = new List<RequestCreate>();

            foreach (var nTA in needToAcpt)
            {
                var reqCreate = await _context.RequestCreateDetails.FindAsync(nTA.CreateID);
                var request = await _context.Requests.FindAsync(reqCreate!.RequestID);
                var staff = await _context.Staffs.FindAsync(reqCreate!.StaffID);

                result.Add(new RequestCreate
                {
                    CreateID = nTA.CreateID,
                    RequestID = reqCreate.RequestID,
                    RequestName = request!.RequestName,
                    CreateTime = reqCreate.CreateTime.ToString(),
                    Status = reqCreate.Status,
                    StaffID = reqCreate.StaffID,
                    StaffName = staff!.StaffName,
                    LastActivity = nTA.Action == "Accept" ? "You have accepted this request!" 
                    : nTA.Action == "" ? "No action yet!" : "You have rejected this request!"
                });
            }

            return result;
        }

        public async Task<RequestDetailResponse> GetRequestDetail(string requestID, string staffID)
        {
            var request = await _context.Requests.FindAsync(requestID);
            var staff = await _context.Staffs.FindAsync(staffID);

            var accepters = "";
            if (request!.AcceptLevel == 1)
            {
                var accepter = await _context.Staffs.FindAsync(staff!.ManagerID);
                if (accepter != null)
                {
                    accepters += accepter.StaffName + " will consider your request.";
                }

                if (!string.IsNullOrEmpty(request.ExternalAcceptID))
                {
                    var external = await _context.Staffs.FindAsync(request.ExternalAcceptID);
                    accepters += "_" + external!.StaffName + " will take responsibility for this request.";
                }
            }
            else 
            if (request!.AcceptLevel == 2)
            {
                var managerID = staff!.ManagerID;
                var accepter = await _context.Staffs.FindAsync(managerID);
                while (accepter != null)
                {
                    accepters += accepter.StaffName + " will consider your request._";
                    if (string.IsNullOrEmpty(accepter.ManagerID)) break;
                    managerID = accepter.ManagerID;
                    accepter = await _context.Staffs.FindAsync(managerID);
                }

                if (!string.IsNullOrEmpty(request.ExternalAcceptID))
                {
                    var external = await _context.Staffs.FindAsync(request.ExternalAcceptID);
                    accepters += external!.StaffName + " will take responsibility for this request.";
                }
                else
                {
                    if (accepters.EndsWith('_')) accepters = accepters.Remove(accepters.Length - 1);
                }
            }
            else
            if (request.AcceptLevel == 0)
            {
                if (!string.IsNullOrEmpty(request.ExternalAcceptID))
                {
                    var external = await _context.Staffs.FindAsync(request.ExternalAcceptID);
                    accepters += external!.StaffName + " will take responsibility for this request.";
                }
            }
            else 
            if (request.AcceptLevel == -1)
            {
                var ceo = await _context.Staffs.FindAsync("24001");
                accepters += ceo!.StaffName + " will consider your request.";
                if (!string.IsNullOrEmpty(request.ExternalAcceptID))
                {
                    var external = await _context.Staffs.FindAsync(request.ExternalAcceptID);
                    accepters += "_" + external!.StaffName + " will take responsibility for this request.";
                }
            }

            return new RequestDetailResponse
            {
                RequestID = requestID,
                RequestName = request.RequestName,
                Rules = request.Rules,
                Title1 = request.Title1,
                Title2 = request.Title2,
                Title3 = request.Title3,
                Answer1Type = request.Answer1Type,
                Answer2Type = request.Answer2Type,
                Answer3Type = request.Answer3Type,
                AcceptLevel = request.AcceptLevel,
                FallbackID = request.ExternalAcceptID,
                Accepters = accepters
            };
        }

        public async Task CreateRequest(RequestCreateVM vm)
        {
            var newCreate = new RequestCreateDetail
            {
                CreateID = Guid.NewGuid().ToString(),
                RequestID = vm.RequestID,
                StaffID = vm.StaffID,
                CreateTime = DateTime.Now,
                Content1 = vm.Content1,
                Content2 = vm.Content2,
                Content3 = vm.Content3,
                Status = "Pending"
            };
            _context.RequestCreateDetails.Add(newCreate);

            await _context.SaveChangesAsync();

            var request = await _context.Requests.FindAsync(vm.RequestID);
            var accepters = await GetAccepters(vm.StaffID, request!);

            _context.RequestAcceptDetails.Add(new RequestAcceptDetail
            {
                CreateID = newCreate.CreateID,
                ManagerID = accepters[0]
            });

            await _context.SaveChangesAsync();
        }

        public async Task CreateReqChangeTime(ChangeTimeRequestVM vm)
        {
            var oldTime = await _context.TimeSheets.FindAsync(vm.Date, vm.StaffID);

            //if (oldTime == null)
            //{
            //    throw new Exception("Can't find timesheet!");
            //}

            var changeTimeModel = AppResources.ChangeTime;
            changeTimeModel = changeTimeModel.Replace("date", vm.Date);
            changeTimeModel = changeTimeModel.Replace("oldCheckIn", $"{oldTime?.CheckIn.Hour}:{oldTime?.CheckIn.Minute}");
            changeTimeModel = changeTimeModel.Replace("newCheckIn", $"{vm.H1}:{vm.M1}");
            changeTimeModel = changeTimeModel.Replace("oldCheckOut", $"{oldTime?.CheckOut.Hour}:{oldTime?.CheckOut.Minute}");
            changeTimeModel = changeTimeModel.Replace("newCheckOut", $"{vm.H2}:{vm.M2}");
            changeTimeModel = changeTimeModel.Replace("oldWorkType", $"{oldTime?.WorkingType}");
            changeTimeModel = changeTimeModel.Replace("newWorkType", $"{vm.WrkType}");
            changeTimeModel = changeTimeModel.Replace("oldOFF", $"{oldTime?.Off}");
            changeTimeModel = changeTimeModel.Replace("newOFF", $"{vm.Off}");

            var newCreate = new RequestCreateDetail
            {
                CreateID = Guid.NewGuid().ToString(),
                RequestID = "1102",
                StaffID = vm.StaffID!,
                CreateTime = DateTime.Now,
                Content1 = changeTimeModel,
                Content2 = vm.Reason!,
                Status = "Pending"
            };
            newCreate.Content3 = await UploadImage.Instance.UploadAsync(Guid.NewGuid().ToString(), vm.Evidence!);

            _context.RequestCreateDetails.Add(newCreate);

            await _context.SaveChangesAsync();

            var request = await _context.Requests.FindAsync("1102");
            var accepters = await GetAccepters(vm.StaffID!, request!);

            _context.RequestAcceptDetails.Add(new RequestAcceptDetail
            {
                CreateID = newCreate.CreateID,
                ManagerID = accepters[0]
            });

            await _context.SaveChangesAsync();
        }

        public async Task<ReqCreateDetailReponse> GetReqCreateDetail(string createID)
        {
            var reqCreate = await _context.RequestCreateDetails.FindAsync(createID);
            var request = await _context.Requests.FindAsync(reqCreate!.RequestID);

            var activites = await GetActivities(reqCreate, request!);

            return new ReqCreateDetailReponse
            {
                RequestID = reqCreate.RequestID,
                RequestName = request!.RequestName,
                Rules = request.Rules,
                Title1 = request.Title1,
                Title2 = request.Title2,
                Title3 = request.Title3,
                Answer1Type = request.Answer1Type,
                Answer2Type = request.Answer2Type,
                Answer3Type = request.Answer3Type,
                Content1 = reqCreate.Content1,
                Content2 = reqCreate.Content2,
                Content3 = reqCreate.Content3,
                Activities = string.Join('_', activites)
            };
        }

        public async Task ConsiderRequest(ConsiderRequestVM vm)
        {
            var acceptDetail = await _context.RequestAcceptDetails.FindAsync(vm.CreateID, vm.ManagerID);
            acceptDetail!.ActionTime = DateTime.Now;
            acceptDetail!.Action = vm.Action;

            _context.RequestAcceptDetails.Update(acceptDetail);

            var create = await _context.RequestCreateDetails.FindAsync(vm.CreateID);
            var request = await _context.Requests.FindAsync(create!.RequestID);
            var accepters = await GetAccepters(create.StaffID, request!);

            var index = accepters.IndexOf(vm.ManagerID);
            if (index >= 0 && index < accepters.Count -1)
            {
                if (vm.Action == "Accept")
                {
                    _context.RequestAcceptDetails.Add(new RequestAcceptDetail
                    {
                        CreateID = vm.CreateID,
                        ManagerID = accepters[index + 1]
                    });
                }
                else
                {
                    create.Status = "Rejected";
                    _context.RequestCreateDetails.Update(create);
                }
            }
            else if (index == accepters.Count - 1)
            {
                create.Status = vm.Action + "ed";
                _context.RequestCreateDetails.Update(create);
                if (vm.Action == "Accept")
                {
                    await HandleAfterAcceptRequest(create);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task HandleAfterAcceptRequest(RequestCreateDetail create)
        {
            var request = await _context.Requests.FindAsync(create.RequestID);
            if (request == null) return;
            if (request.RequestID != "1102" && request.RequestID != "1101") return;

            if (request.RequestID == "1102")
            {
                var newTimeSheet = create.Content1;
                var newCheckInIndex = newTimeSheet.IndexOf("</p>", 25);
                newCheckInIndex--;
                var newCheckIn = "";
                while (newTimeSheet[newCheckInIndex] != ' ')
                {
                    newCheckIn = newTimeSheet[newCheckInIndex] + newCheckIn;
                    newCheckInIndex--;
                }

                var newCheckOutIndex = newTimeSheet.IndexOf("</p>", newCheckInIndex + 10);
                newCheckOutIndex--;
                var newCheckOut = "";
                while (newTimeSheet[newCheckOutIndex] != ' ')
                {
                    newCheckOut = newTimeSheet[newCheckOutIndex] + newCheckOut;
                    newCheckOutIndex--;
                }

                var newWorkTypeIndex = newTimeSheet.IndexOf("</p>", newCheckOutIndex + 10);
                newWorkTypeIndex--;
                var newWorkType = "";
                while (newTimeSheet[newWorkTypeIndex] != ' ')
                {
                    newWorkType = newTimeSheet[newWorkTypeIndex] + newWorkType;
                    newWorkTypeIndex--;
                }

                var newOffIndex = newTimeSheet.IndexOf("</p>", newWorkTypeIndex + 10);
                newOffIndex--;
                var newOff = "";
                while (newTimeSheet[newOffIndex] != ' ')
                {
                    newOff = newTimeSheet[newOffIndex] + newOff;
                    newOffIndex--;
                }

                var date = "";
                var startDateIndex = 3;
                while (newTimeSheet[startDateIndex] != ':')
                {
                    date += newTimeSheet[startDateIndex++];
                }

                var timeSheet = await _context.TimeSheets.FindAsync(date, create.StaffID);
                if (timeSheet == null)
                {
                    timeSheet = new TimeSheet
                    {
                        Date = date,
                        StaffID = create.StaffID
                    };
                    _context.TimeSheets.Add(timeSheet);
                    await _context.SaveChangesAsync();
                }

                var keys = date.Split('/');
                var checkIn = newCheckIn.Split(':');
                timeSheet.CheckIn = new DateTime(
                    int.Parse(keys[2]), int.Parse(keys[1]), int.Parse(keys[0]), 
                    int.Parse(checkIn[0]), int.Parse(checkIn[1]), 0
                );

                var checkOut = newCheckOut.Split(':');
                timeSheet.CheckOut = new DateTime(
                    int.Parse(keys[2]), int.Parse(keys[1]), int.Parse(keys[0]),
                    int.Parse(checkOut[0]), int.Parse(checkOut[1]), 0
                );

                timeSheet.WorkingType = newWorkType;
                timeSheet.Off = newOff;
                timeSheet.Total = CalcTotal(timeSheet);
                _context.TimeSheets.Update(timeSheet);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ChangeTimeResponse>> GetChangeTimeRequest(string staffID, int month, int year)
        {
            var creates = await _context.RequestCreateDetails
                .Where(c => c.StaffID == staffID && c.RequestID == "1102" && c.CreateTime.Month == month && c.CreateTime.Year == year)
                .OrderByDescending(c => c.CreateTime)
                .ToListAsync();

            var result = new List<ChangeTimeResponse>();
            foreach (var cr in creates)
            {
                var newTimeSheet = cr.Content1;
                var date = "";
                var startDateIndex = 3;
                while (newTimeSheet[startDateIndex] != ':')
                {
                    date += newTimeSheet[startDateIndex++];
                }

                result.Add(new ChangeTimeResponse
                {
                    Date = date,
                    CreateID = cr.CreateID,
                    Status = cr.Status
                });
            }
            return result;
        }

        public async Task<IEnumerable<RequestCreate>> GetDeviceRequest()
        {
            var creates = await _context.RequestCreateDetails
                .Where(c => c.RequestID == "1105")
                .OrderByDescending(c => c.CreateTime)
                .ToListAsync();

            var request = await _context.Requests.FindAsync("1105");
            var result = new List<RequestCreate>();
            foreach (var cr in creates)
            {
                var staff = await _context.Staffs.FindAsync(cr.StaffID);
                result.Add(new RequestCreate
                {
                    CreateID = cr.CreateID,
                    RequestID = request!.RequestID,
                    RequestName = request.RequestName,
                    CreateTime = cr.CreateTime.ToString(),
                    Status = cr.Status,
                    StaffID = cr.StaffID,
                    StaffName = staff!.StaffName
                });
            }
            return result;
        }

        public async Task<List<string>> GetAccepters(string staffID, Request request)
        {
            var accepters = new List<string>();
            var staff = await _context.Staffs.FindAsync(staffID);

            switch (request.AcceptLevel)
            {
                case 0:
                    accepters.Add(request.ExternalAcceptID);
                    break;
                case 1:
                    accepters.Add(staff!.ManagerID);
                    break;
                case -1:
                    accepters.Add("24001");
                    break;
                case 2:
                    var accepterID = staff!.ManagerID;
                    while (!string.IsNullOrEmpty(accepterID))
                    {
                        var accepter = await _context.Staffs.FindAsync(accepterID);
                        accepters.Add(accepterID);
                        accepterID = accepter!.ManagerID;
                    }
                    if (!string.IsNullOrEmpty(request.ExternalAcceptID))
                    {
                        accepters.Add(request.ExternalAcceptID);
                    }
                    break;
            }

            return accepters;
        }

        public async Task<List<string>> GetActivities(RequestCreateDetail reqCreate, Request request)
        {
            var activities = new List<string>();
            var accepters = await GetAccepters(reqCreate.StaffID, request);

            foreach (var accepterID in accepters)
            {
                var accepter = await _context.Staffs.FindAsync(accepterID);
                var acceptDetail = await _context.RequestAcceptDetails.FindAsync(reqCreate.CreateID, accepterID);
                if (acceptDetail == null || acceptDetail.Action == "")
                {
                    activities.Add(accepter!.StaffName + " has not taken any action yet!");
                    continue;
                }

                if (acceptDetail.Action == "Accept")
                {
                    activities.Add(accepter!.StaffName + $" has accepted this request at {acceptDetail.ActionTime}.");
                }

                if (acceptDetail.Action == "Reject")
                {
                    activities.Add(accepter!.StaffName + $" has rejected this request at {acceptDetail.ActionTime}.");
                }
            }

            return activities;
        }

        double CalcTotal(TimeSheet timeSheet)
        {
            var total = (timeSheet.CheckOut - timeSheet.CheckIn).TotalHours - timeSheet.LunchBreakHour;

            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                return Math.Round(total * 1.5, 1);
            }

            if (IsVietNameseHoliday(timeSheet.Date))
            {
                return Math.Round(total * 2, 1);
            }

            return Math.Round(total, 1);
        }

        bool IsVietNameseHoliday(string input)
        {
            var date = DateTime.ParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            List<DateTime> holidays = new List<DateTime>
            {
                new DateTime(date.Year, 1, 1),    // Tết Dương lịch
                new DateTime(date.Year, 4, 30),   // Ngày Giải phóng miền Nam
                new DateTime(date.Year, 5, 1),    // Ngày Quốc tế Lao động
                new DateTime(date.Year, 9, 2)     // Ngày Quốc khánh
            };

            var lunarCalendar = new ChineseLunisolarCalendar();
            holidays.Add(lunarCalendar.ToDateTime(date.Year, 3, 10, 6, 0, 0, 0));
            holidays.Add(lunarCalendar.ToDateTime(date.Year, 1, 1, 6, 0, 0, 0));
            holidays.Add(lunarCalendar.ToDateTime(date.Year, 1, 2, 6, 0, 0, 0));
            holidays.Add(lunarCalendar.ToDateTime(date.Year, 1, 3, 6, 0, 0, 0));

            foreach (var holiday in holidays)
            {
                if (date.Date == holiday.Date) return true;
            }

            return false;
        }
    }
}
