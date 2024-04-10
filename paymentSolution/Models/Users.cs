using Microsoft.AspNetCore.Mvc.ModelBinding;
using paymentSolution.Models.Responsex;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace paymentSolution.Models
{

    public class Users
    {
        public string response_code { get; set; }
        public string response_message { get; set; }
        public ResponseData response_data { get; set; }

        public class ResponseData
        {

            [DataType(DataType.Text)]
            public string FULLNAME { get; set; }
            [Required, EmailAddress]
            public string EMAIL { get; set; }
            public string ADDRESS { get; set; }
            [DataType(DataType.PhoneNumber)]
            public string PHONENUMBER { get; set; }
            [DataType(DataType.Text)]
            public string EVENTNAME { get; set; }
            [DataType(DataType.Text)]
            public string GENDER { get; set; }
            [DataType(DataType.Text)]
            public string UNIQUEID { get; set; }
            [DataType(DataType.Text)]
            public string PAID { get; set; }
        }
        
    }
}
