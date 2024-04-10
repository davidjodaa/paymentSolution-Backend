using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace paymentSolution.Models
{
    public class AuthenticationResp
    {
        public string CHANNEL_CODE { get; set; }
        public string CHANNEL_ALLOWED { get; set; }
    }
}
