namespace paymentSolution.Models.Responsex
{
    public class GenerateAccountResp
    {

        public string response_code { get; set; }
        public string response_message { get; set; }
        public ResponseData response_data { get; set; }
    }
    public class ResponseData
    {
        public string request_id { get; set; }
        public string virtual_acct_no { get; set; }
        public string virtual_acct_name { get; set; }
        public string expiry_datetime { get; set; }
    }
}

