using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using paymentSolution.Implementation;
using paymentSolution.Models.Request;
using System.Net;
//using Twilio.Exceptions;

namespace paymentSolution.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {

        private readonly IRepository _repository;
        private readonly IDatabaseCalls _databasecalls;
        private readonly IApiImplementation _apiimplementation;

        public RegistrationController(IRepository repository, IDatabaseCalls databasecalls, IApiImplementation apiImplementation)
        {
            _repository = repository;
            _databasecalls = databasecalls;
            _apiimplementation = apiImplementation;
        }

        [HttpGet("GetAllRegistrants")]
        public async Task<IActionResult> GetAllUsers(string channelCode, string authorization)
        {
            try
            {
                var resp =  _repository.GetAllUsers(channelCode, authorization);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                // throw new ApiException($"Username '{req.UserName}' is already taken.");
            }
            return null;

        }

        [HttpPost("GetFemaleResgistrantsCount")]
        public async Task<IActionResult> GetCountOfFemaleRegistrants(string channelCode, string authorization)
        {
            try
            {
                var resp = _repository.GetFemaleUsers(channelCode, authorization);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                // throw new ApiException($"Username '{req.UserName}' is already taken.");
            }
            return null;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterReq req)
        {
            try
            {
                var resp = _repository.RegisterUser(req);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                // throw new ApiException($"Username '{req.UserName}' is already taken.");
            }
            return null;

        }

        [HttpPost("GenerateAccountNumber")]
        public async Task<IActionResult> GenerateAccountNo(GenerateAccReq req)
        {
            try
            {
                var resp = _repository.GenerateAccNo(req);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                // throw new ApiException($"Username '{req.UserName}' is already taken.");
            }
            return null;

        }

        [HttpPost("CallbackService")]
        public async Task<IActionResult> CallbackService(CallbackReq req)
        {
            try
            {
                var resp = _repository.CallbackService(req);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                // throw new ApiException($"Username '{req.UserName}' is already taken.");
            }
            return null;

        }

        [HttpPost("ClusterdrawCreateacc")]
        public async Task<IActionResult> clusterCreateacc(ClusterdrawReq req)
        {
            try
            {
                var resp = _repository.clusterCreateAcc(req);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                // throw new ApiException($"Username '{req.UserName}' is already taken.");
            }
            return null;

        }


    }
}
