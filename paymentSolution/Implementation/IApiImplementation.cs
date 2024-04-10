using paymentSolution.Models;
using paymentSolution.Models.Request;
using paymentSolution.Models.Responsex;

namespace paymentSolution.Implementation
{
    public interface IApiImplementation
    {
        GenerateAccountResp GenerateAccount(GenerateAccountReq req);
        MailResp Sendmail(MailReq req);
        ClusterdrawResp ClustercreateaccC(ClusterdrawReq req);
    }
}
