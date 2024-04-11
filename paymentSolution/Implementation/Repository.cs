using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using NLog.Fluent;
using PartyPal.Models;
using PartyPal.Models.Request;
using paymentSolution.Models;
using paymentSolution.Models.Request;
using paymentSolution.Models.Responsex;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
//using Twilio.Exceptions;

namespace paymentSolution.Implementation
{
    public class Repository : IRepository
    {

        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private IApiImplementation apiImpl;
        private IDatabaseCalls dbCalls;
        private readonly string defaultLimitCategory;
        private readonly string portalUrl;
        private readonly IMemoryCache cache;
        private readonly string channelCode;
        private readonly string limitExcludeCategory;

        public Repository(ILogger<Repository> logger, IConfiguration config, IDatabaseCalls dbCalls, IApiImplementation apiImpl, IMemoryCache cache)
        {
            _logger = logger;
            _config = config;
            this.dbCalls = dbCalls;
            this.apiImpl = apiImpl;
            this.cache = cache;
            _config = config;
        }


        public async Task<int> GetFemaleUsers(string channelCode, string authorization)
        {
            try
            {
                
                var operationname = "AFFPAYMENTS";

                var checkIfAuthorized = dbCalls.CheckIfAuthorized(channelCode, authorization, operationname);
                if (checkIfAuthorized == null)
                {
                    Log.Debug($"Not authorized");
                    return 99;

                }

                List<Users> user = await dbCalls.GetFemaleUsers();
                var User = user.Count;
                return User;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return 0;
            }
           
        }

        public ClusterdrawResp clusterCreateAcc(ClusterdrawReq req)
        {
            try
            {
                var resps = apiImpl.ClustercreateaccC(req);
                return resps;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public IEnumerable<Users.ResponseData> GetAllUsers(string channelCode, string authorization)
        {
            try
            {
                //ClusterdrawReq req = new ClusterdrawReq()
                //{
                //    name = "Sammy",
                //    target = "5",
                //    accountNumber = "1484223602",
                //    phoneNumber = "1484223602"
                //};

                //var resps = apiImpl.ClustercreateaccC(req);


                var operationname = "";

                var checkIfAuthorized = dbCalls.CheckIfAuthorized(channelCode, authorization, operationname);
                if (checkIfAuthorized == null)
                {
                    Log.Debug($"Not authorized");
                    Users resp = new Users()
                    {
                        response_code = "34",
                        response_message = "Unauthorized"
                    };
                    return (IEnumerable<Users.ResponseData>)resp;

                }

                var user = dbCalls.GetAllUsers();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }

        public Users GetUserByEmail(string Email)
        {
            try
            {
                var user = dbCalls.GetUserByEmail(Email);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }

        

        public string GenerateUniqueId(string Email)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(Email);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                string uniqueId = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return uniqueId;
            }
        }


        public async Task<Response<string>> RegisterUser(RegisterReq req)
        {

            var Email = req.EMAIL.ToUpper();

            
            var userWithSameEmail = GetUserByEmail(Email);
            if (userWithSameEmail != null)
            {
                return new Response<string>(req.EMAIL, message: $"Email already exists.");
            }

            var channelcode = req.CHANNELCODE.ToUpper();
            var authorization = req.AUTHENTICATION;
            var operationname = "";

            var checkIfAuthorized = dbCalls.CheckIfAuthorized(channelcode, authorization, operationname);
            if (checkIfAuthorized == null)
            {
                Log.Debug($"Not authorized");
                return new Response<string>(req.EMAIL, message: $"Not authorized to call this service.");

            }

            var uniqueId = GenerateUniqueId(Email);

            var insertuser = new InsertUserReq
            {
                EMAIL = req.EMAIL.ToUpper(),
                FULLNAME = req.FULLNAME.ToUpper(),
                ADDRESS = req.ADDRESS.ToUpper(),
                PHONENUMBER = req.PHONENUMBER,
                EVENTNAME = req.EVENTNAME.ToUpper(),
                GENDER = req.GENDER.ToUpper(),
                UNIQUEID = uniqueId
            };

            var InsertUser = dbCalls.InsertNewUser(insertuser);

            if (InsertUser == null)
            {
                return new Response<string>(req.EMAIL, message: $"User Registration failed.");
            }

            MailReq Req = new MailReq
            {
                from = _config["APISettings:From"],
                to = Email,
                cc = _config["APISettings:CopyAddress"],
                subject = _config["APISettings:RegSubject"],
                displayName = _config["APISettings:RegDisplayName"]
            };
            var Sendmail = apiImpl.Sendmail(Req);
            if (Sendmail == null ||Sendmail.succeeded != true) 
            {
                return new Response<string>(req.EMAIL, message: $"Mail was not sent");
            }
            return new Response<string>(req.EMAIL, message: $"User Registered successfully.");
        }

        public GenerateAccountResp GenerateAccNo(GenerateAccReq req)
        {
            var Email = req.Email.ToUpper();
            var amount = req.amount;
            var channelcode = req.channelcode;
            var authorization = req.authorization;
            var operationname = "AFFPAYMENTS";

            var CheckWithUniqueId = dbCalls.CheckWithUniqueId(Email);
            if (CheckWithUniqueId == null)
            {
                GenerateAccountResp resp = new GenerateAccountResp()
                {
                    response_code = "99",
                    response_message = "Incorrect Email or payment id"
                };
                Log.Debug($"Incorrect Email or payment id");
                return resp;
            }

            var checkIfAuthorized = dbCalls.CheckIfAuthorized(channelcode, authorization, operationname);
            if (checkIfAuthorized == null)
            {
                
                GenerateAccountResp resp = new GenerateAccountResp()
                {
                    response_code = "99",
                    response_message = "Not authorized to call this service"
                };
                Log.Debug($"Not authorized to call this service");
                return resp;

            }

            GenerateAccountReq gen = new GenerateAccountReq
            {
                customer_id = $"{CheckWithUniqueId.response_data.UNIQUEID}",
                merchant_id = "",
                customer_name = "",
                customer_email = Email,
                customer_phone = "08163919169",
                transaction_amount = amount,
                channel_code = "BREEZEAPP",
                
            };

            var GenAcc = apiImpl.GenerateAccount(gen);
            if (GenAcc == null)
            {
                GenerateAccountResp resp = new GenerateAccountResp()
                {
                    response_code = "99",
                    response_message = "Error occured when generating account number"
                };
                Log.Debug($"Error occured when generating account number");
                return resp;
            }

            if (GenAcc.response_code == "00")
            {
                var accountNo = GenAcc.response_data.virtual_acct_no;
                var requestId = GenAcc.response_data.request_id;

                var updateRequestId = dbCalls.UpdateRequestId(accountNo, requestId, Email);
                if (updateRequestId)
                {
                    Log.Debug(GenAcc.response_message);
                    return GenAcc;
                }
                else
                {
                    GenerateAccountResp resp = new GenerateAccountResp()
                    {
                        response_code = "99",
                        response_message = "Error occured when Updating DB"
                    };
                    Log.Debug($"Error occured when Updating DB");
                    return resp;
                }
            }
            else
            {
                return GenAcc;
            }
        }


        public async Task<CallbackResp<string>> CallbackService(CallbackReq req)
        {

            var Transaction_ref = req.transaction_reference;
            var session_id = req.session_id;

            var CheckTransExist = dbCalls.CheckTransExists(Transaction_ref);
            if (CheckTransExist == null)
            {
                Log.Debug($"Duplicate Transaction");
                return new CallbackResp<string>(Transaction_ref, response_message: $"Duplicate Transaction");
            }



            var CheckDuplicateCallback = dbCalls.CheckDuplicateTransref(Transaction_ref);
            if (CheckDuplicateCallback != null)
            {
                Log.Debug($"Duplicate Transaction");
                return new CallbackResp<string>(Transaction_ref, response_message: $"Duplicate Transaction");
            }

            var updateTransRef = dbCalls.UpdateTransRef(Transaction_ref, session_id);

            if (updateTransRef)
            {
                MailReq Req = new MailReq
                {
                    from = _config["APISettings:From"],
                    to = CheckTransExist.response_data.EMAIL,
                    cc = _config["APISettings:CopyAddress"],
                    subject = _config["APISettings:PaySubject"],
                    mailMessage = $"<head>" +
                               ",
                    displayName = _config["APISettings:PayDisplayName"]
                };
                var Sendmail = apiImpl.Sendmail(Req);
                if (Sendmail == null || Sendmail.succeeded != true)
                {
                    return new CallbackResp<string>(Transaction_ref, response_message: $"Mail was not sent");
                }

                Log.Debug($"Successful Request");
                return new CallbackResp<string>(Transaction_ref, response_message: $"Successful Request");
            }
            else
            {
                Log.Debug($"Request failed");
                return new CallbackResp<string>(Transaction_ref, response_message: $"Request failed");
            }
        }


    }
}
