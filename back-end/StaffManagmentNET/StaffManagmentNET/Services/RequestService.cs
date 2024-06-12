using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

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
            }

            await _context.SaveChangesAsync();
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
    }
}
