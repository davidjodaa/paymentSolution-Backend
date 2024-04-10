using PartyPal.Models;
using PartyPal.Models.Request;
using paymentSolution.Models;
using paymentSolution.Models.Request;
using paymentSolution.Models.Responsex;

namespace paymentSolution.Implementation
{
    public interface IRepository
    {
        Task<Response<string>> RegisterUser(RegisterReq req);
        GenerateAccountResp GenerateAccNo(GenerateAccReq req);
        public IEnumerable<Users.ResponseData> GetAllUsers(string channelCode, string authorization);
        Task<int> GetFemaleUsers(string channelCode, string authorization);
        Users GetUserByEmail(string Email);
        Task<CallbackResp<string>> CallbackService(CallbackReq req);
        ClusterdrawResp clusterCreateAcc(ClusterdrawReq req);
    }
}
