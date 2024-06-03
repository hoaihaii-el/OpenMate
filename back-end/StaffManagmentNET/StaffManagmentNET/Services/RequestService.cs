using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Responses;

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
                var lastActivity = activities.Split("_").LastOrDefault();

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

        public async Task<string> GetActivities(RequestCreateDetail reqCreate, Request request)
        {
            var acceptLevel = request.AcceptLevel;
            switch (acceptLevel)
            {
                case 0:
                    var acceptDetail = await _context.RequestAcceptDetails.FindAsync(reqCreate.CreateID, request.ExternalAcceptID);
                    var external = await _context.Staffs.FindAsync(request.ExternalAcceptID);
                    if (acceptDetail == null || acceptDetail.Action != "Accept") 
                        return external!.StaffName + " is considering your request!";
                    return external!.StaffName + "has accepted your request!";
                case 1:
                    var staff = await _context.Staffs.FindAsync(reqCreate.StaffID);
                    var manager = await _context.Staffs.FindAsync(staff!.ManagerID);
                    if (manager == null)
                        return "";
                    var acceptD = await _context.RequestAcceptDetails.FindAsync(reqCreate.CreateID, manager.StaffID);
                    if (acceptD == null || acceptD.Action != "Accept")
                        return manager.StaffName + " is considering yout request!";
                    return manager.StaffName + " has accepted your request!";
                case -1:
                    var ceo = await _context.Staffs.FindAsync("24001");
                    var acptDetail = await _context.RequestAcceptDetails.FindAsync(reqCreate.CreateID, ceo!.StaffID);
                    if (acptDetail == null || acptDetail.Action != "Accept")
                        return ceo.StaffName + " is considering your request!";
                    return ceo.StaffName + " has accepted your request!";
                case 2:
                    var activities = "";
                    var requester = await _context.Staffs.FindAsync(reqCreate.StaffID);
                    var accepterID = requester!.ManagerID;
                    var acptD = await _context.RequestAcceptDetails.FindAsync(reqCreate.CreateID, accepterID);
                    while (acptD != null)
                    {
                        var accepter = await _context.Staffs.FindAsync(accepterID);
                        activities += accepter!.StaffName + " has accepted your request!_";
                        if (string.IsNullOrEmpty(accepter!.ManagerID)) break;
                        accepterID = accepter!.ManagerID;
                        acptD = await _context.RequestAcceptDetails.FindAsync(reqCreate.CreateID, accepterID);
                    }

                    var acpter = await _context.Staffs.FindAsync(accepterID);
                    if (acptD != null)
                    {
                        return activities + acpter!.StaffName + " has accepted your request!";
                    }
                    else
                    {
                        return activities + acpter!.StaffName + " is considering your request!";
                    }
            }
            return "";
        }
    }
}
