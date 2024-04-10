using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog.Fluent;
using paymentSolution.Models;
using paymentSolution.Models.Request;
using paymentSolution.Models.Responsex;
using RestSharp;

namespace paymentSolution.Implementation
{
    public class ApiImplementation : IApiImplementation 
    { 
    private readonly ILogger<ApiImplementation> _logger;
    private readonly IConfiguration _config;
    private readonly string authToken;
    private readonly string authToken2;
    private readonly string GenAccURL;
    private readonly string SendMailURL;
    private readonly string MailAuth;

        public ApiImplementation(ILogger<ApiImplementation> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        authToken = _config["APISettings:Authorization"];
        authToken2 = _config["APISettings:AzuAuthorization"];
        GenAccURL = _config["APISettings:GeneratAccURL"];
        SendMailURL = _config["APISettings:mailURL"];
        MailAuth = _config["APISettings:MailAuth"];
        }
        public GenerateAccountResp GenerateAccount(GenerateAccountReq req)
        {
            try
            {
                string fullUrl = GenAccURL;
                string reqbody = JsonConvert.SerializeObject(req);
                var client = new RestClient(fullUrl);

                var request = new RestRequest(Method.POST);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", authToken);
                request.AddParameter("reqbody", reqbody, ParameterType.RequestBody);
                IRestResponse<GenerateAccountResp> response = client.Execute<GenerateAccountResp>(request);
                GenerateAccountResp resp = JsonConvert.DeserializeObject<GenerateAccountResp>(response.Content);
                if (resp == null)
                {
                    Log.Debug($"An error occured while generating account number");
                    Console.WriteLine($"An error occured while generating account number");

                    return null;
                }
                Log.Debug($"{resp.response_message}");
                return resp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public ClusterdrawResp ClustercreateaccC (ClusterdrawReq req)
        {
            try
            {
                string fullUrl = "";
                string reqbody = JsonConvert.SerializeObject(req);
                var client = new RestClient(fullUrl);

                var request = new RestRequest(Method.POST);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-API-Key", "");
                request.AddParameter("reqbody", reqbody, ParameterType.RequestBody);
                IRestResponse<ClusterdrawResp> response = client.Execute<ClusterdrawResp>(request);
                ClusterdrawResp resp = JsonConvert.DeserializeObject<ClusterdrawResp>(response.Content);
                if (resp == null)
                {
                    Log.Debug($"An error occured while generating account number");
                    Console.WriteLine($"An error occured while generating account number");

                    return null;
                }
                Log.Debug($"{resp.ResponseMessage}");
                return resp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public MailResp Sendmail(MailReq req)
        {
            string fullUrl = SendMailURL;
            var client = new RestClient(fullUrl);
            var request = new RestRequest(Method.POST);
            var jsonbody = JsonConvert.SerializeObject(req);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Authorization", MailAuth);
            request.AddParameter("jsonbody", jsonbody, ParameterType.RequestBody);
            IRestResponse<MailResp> response = client.Execute<MailResp>(request);
            MailResp jsonresp = JsonConvert.DeserializeObject<MailResp>(response.Content);

            if (jsonresp == null)
            {
                Log.Debug($"An error occured when calling the Email service");
                Console.WriteLine($"An error occured when calling the Email service");

                return null;
            }
            Log.Debug($"{jsonresp.message}");
            Console.WriteLine($"{jsonresp.message}");
            return jsonresp;
        }
    }
}
