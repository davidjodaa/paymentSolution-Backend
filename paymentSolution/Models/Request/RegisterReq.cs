using System.ComponentModel.DataAnnotations;

namespace paymentSolution.Models.Request
{
    public class RegisterReq
    {
        [Required]
        public string FULLNAME { get; set; }

        [Required]
        [EmailAddress]
        public string EMAIL { get; set; }

        [Required]
        public string PHONENUMBER { get; set; }
        public string ADDRESS { get; set; }

        [Required]
        public string GENDER { get; set; }

        [Required]
        public string EVENTNAME { get; set; }

        [Required]
        public string CHANNELCODE { get; set; }

        [Required]
        public string AUTHENTICATION { get; set; }
    }

   
    
}
