using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Repositories
{
    public interface IRequestRepo
    {
        Task<IEnumerable<RequestType>> GetAllRqst();
        Task<IEnumerable<RequestCreate>> GetYourRequest(string staffID);
        Task<IEnumerable<RequestCreate>> GetNeedToAcceptRequest(string managerID);
        Task<RequestDetailResponse> GetRequestDetail(string requestID, string staffID);
        Task CreateRequest(RequestCreateVM vm);
        Task CreateReqChangeTime(ChangeTimeRequestVM vm);
        Task<ReqCreateDetailReponse> GetReqCreateDetail(string createID);
        Task ConsiderRequest(ConsiderRequestVM vm);
        Task<IEnumerable<ChangeTimeResponse>> GetChangeTimeRequest(string staffID, int month, int year);
        Task<IEnumerable<RequestCreate>> GetDeviceRequest();
    }
}
