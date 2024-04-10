using PartyPal.Models;
using PartyPal.Models.Request;
using paymentSolution.Models;

namespace paymentSolution.Implementation
{
    public interface IDatabaseCalls
    {
        Task<List<Users>> GetFemaleUsers();
        Users GetUserByEmail(string Email);
        Task<bool> InsertNewUser(InsertUserReq req);
        public IEnumerable<Users.ResponseData> GetAllUsers();
        Users CheckWithUniqueId(string Email);
        AuthenticationResp CheckIfAuthorized(string channelcode, string authorization, string OperationName);
        bool UpdateRequestId(string accountNo, string requestId, string Email);
        bool UpdateTransRef(string transRef, string sessionId);
        Users CheckDuplicateTransref(string transRef);
        Users CheckTransExists(string transRef);
    }
}
