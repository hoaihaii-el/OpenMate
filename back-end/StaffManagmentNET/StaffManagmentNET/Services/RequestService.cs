using Microsoft.EntityFrameworkCore;
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



                result.Add(new RequestCreate
                {
                    CreateID = req.CreateID,
                    RequestID = req.RequestID,
                    RequestName = request.RequestName,
                    CreateTime = req.CreateTime.ToString(),
                    Status = req.Status,
                    StaffID = req.StaffID,
                });
            }

            return result;
        }

        public Task<IEnumerable<RequestCreate>> GetNeedToAcceptRequest(string managerID)
        {
            throw new NotImplementedException();
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
