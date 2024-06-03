using StaffManagmentNET.Responses;

namespace StaffManagmentNET.Repositories
{
    public interface IRequestRepo
    {
        Task<IEnumerable<RequestType>> GetAllRqst();
        Task<IEnumerable<RequestCreate>> GetYourRequest(string staffID);
        Task<IEnumerable<RequestCreate>> GetNeedToAcceptRequest(string managerID);
        Task<RequestDetailResponse> GetRequestDetail(string requestID, string staffID);
    }
}
