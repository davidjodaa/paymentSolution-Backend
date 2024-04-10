using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace PartyPal.Models.Request
{
    public class InsertUserReq
    {
        public string FULLNAME { get; set; }
        public string EMAIL { get; set; }
        public string PHONENUMBER { get; set; }
        public string ADDRESS { get; set; }
        public string GENDER { get; set; }
        public string EVENTNAME { get; set; }
        public string UNIQUEID { get; set; }
    }
}
