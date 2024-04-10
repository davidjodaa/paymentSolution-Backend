using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paymentSolution.Models
{
    public class MailResp
    {
        public bool succeeded { get; set; }
        public string data { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }
}
