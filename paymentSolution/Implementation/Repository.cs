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


                var operationname = "AFFPAYMENTS";

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
            var operationname = "AFFPAYMENTS";

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
                customer_id = $"AFF{CheckWithUniqueId.response_data.UNIQUEID}",
                merchant_id = "AFF",
                customer_name = "AFF",
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
                                $"<meta charset=\"utf-8\" /> <meta name=\"x-apple-disable-message-reformatting\" /> <meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\" /> " +
                                $"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" /> " +
                                $"<meta name=\"format-detection\" content=\"telephone=no, date=no, address=no, email=no\" /> " +
                                $"<title>AFF Registration Notification</title>" +
                                $" <link href=\"https://fonts.googleapis.com/css?family=Montserrat:ital,wght@0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,200;1,300;1,400;1,500;1,600;1,700\" rel=\"stylesheet\" media=\"screen\" />" +
                                $"<style>.hover-underline:hover </style></head>" +
                                $"<body style=\" margin: 0; padding: 0; width: 100%; word-break: break-word; -webkit-font-smoothing: antialiased; --bg-opacity: 100; background-color: #ffffff; background-color: #ffffff; \">" +
                                $" <div style=\"display: none\"> Thank you for using BreezePay </div> <div role=\"article\" aria-roledescription=\"email\" aria-label=\"Thank you for using BreezePay uD83DuDC4B\" lang=\"en\">" +
                                $"<table style=\" font-family: Montserrat, -apple-system, ''Segoe UI'' , sans-serif; width: 100%; \" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"> <tr>" +
                                $"<td align=\"center\" style=\" --bg-opacity: 1; background-color: #ffffff; background-color: rgba( 236,239, 241, var(--bg-opacity) ); font-family: Montserrat, -apple-system, ''Segoe UI'' , sans-serif; \" bgcolor=\"rgba(236, 239, 241, var(--bg-opacity))\"> " +
                                $"<table class=\"sm-w-full\" style=\" font-family: ''Montserrat'' , Arial, sans-serif; width: 600px; \" width=\"600\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">" +
                                $"<tr><td class=\"sm-py-32 sm-px-24\" style=\" font-family: Montserrat, -apple-system, ''Segoe UI'', sans-serif; padding: 12px; text-align: center; \" align=\"center\"> <a href=\"https://africafintechfoundry.ng\"> " +
                                $"<img src=\"https://res.cloudinary.com/dvqls9yc9/image/upload/v1701436225/bq2dabyqe7vmcaby0fiu.png\" width=\"250\" alt=\"BreezePay\" style=\" border: 0; max-width: 100%;line-height: 100%; vertical-align: middle; \" /> " +
                                $"</a> </td></tr> <tr> <td align=\"center\" class=\"sm-px-24\" style=\" font-family: ''Montserrat'' , Arial,sans-serif; \">" +
                                $"<table style=\" font-family: ''Montserrat'' , Arial, sans-serif; width: 100%; \"width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"><tr>" +
                                $"<td class=\"sm-px-24\" style=\" --bg-opacity: 1; background-color: #ffffff; background-color: rgba( 255, 255, 255, var(--bg-opacity) );" +
                                $" border-radius: 4px; font-family: Montserrat, -apple-system, ''Segoe UI'' , sans-serif; font-size: 14px; line-height: 24px; padding: 12px; text-align: left; --text-opacity: 1; color: #626262; " +
                                $"color: rgba( 98, 98, 98,var(--text-opacity) ); \" bgcolor=\"rgba(255, 255, 255, var(--bg-opacity))\"align=\"left\"> <p style=\" font-weight: 600; font-size: 18px; margin-bottom: 0; \"> Hello, </p>" +
                                
                                $"<p>Your payment for FDA seminar has been successfully recieved, you will get an Email stating the next steps to move forward.</p>\r\n " +
                                $"<p></p>\r\n " +
                                $"<p>Thank you very much and God bless</p>\r\n " +

                                $"<p style=\"margin: 12px 0\">{DateTime.Now}</p> <p style=\"margin: 12px 0\">Cheers</p> <p style=\"margin: 12px 0\"> <strong>From AFF Team</strong> </p>" +
                                $"<div ><a href=\"https://africafintechfoundry.ng\"><img src=\"https://res.cloudinary.com/dvqls9yc9/image/upload/v1701436225/bq2dabyqe7vmcaby0fiu.png\"width=\"50\"" +
                                $" alt=\"BreezePay\" style=\" border: 0; max-width: 20%;line-height: 20%; vertical-align: right; \" /> </a></div><table style=\"font-family: ''Montserrat'' , Arial, sans-serif; width: 100%;\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"role=\"presentation\">" +
                                $"<tr><td style=\" font-family: ''Montserrat'' , Arial, sans-serif;padding-top: 32px; padding-bottom: 32px; \"><div style=\" --bg-opacity: 1; background-color: #eceff1;background-color: rgba( 236, 239, 241, var( --bg-opacity )); height: 1px; line-height: 1px; \">" +
                                $" &zwnj; </div></td></tr> </table><p style=\"margin: 0 0 16px\"> If you have questions about this transaction, please call or chat us on" +
                                $"<a href=\"*\" class=\"hover-underline\"style=\" --text-opacity: 1; color: #1f376d; color: rgba( 115, 103,240, var( --text-opacity ) ); text-decoration: none; \"> " +
                                $"09168350086</a> or send an email to<a href=\"support@breezepay.io\" class=\"hover-underline\" style=\" --text-opacity: 1; color: #1f376d; color: rgba( 115, 103, 240, var( --text-opacity ) ); text-decoration: none; \"> " +
                                $"support@breezepay.io</a>. </p><p style=\"margin: 0 0 16px\"> Thank you for choosing BreezePay, <br />The BreezePay Team </p></td></tr><tr><td style=\" font-family: ''Montserrat'' , Arial, sans-serif; height: 20px; \" height=\"20\">" +
                                $"</td></tr></table></td></tr></table> </td></tr></table></div></body> </html>",
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
