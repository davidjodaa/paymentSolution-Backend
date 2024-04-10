namespace paymentSolution.Models.Request
{
    public class CallbackReq
    {
        public string customer_id { get; set; }
        public string virtual_account_no { get; set; }
        public string virtual_account_name { get; set; }
        public string transaction_amount { get; set; }
        public string transaction_reference { get; set; }
        public string bo_referenceno { get; set; }
        public string source_account { get; set; }
        public string source_account_name { get; set; }
        public string source_bank { get; set; }
        public string source_bank_code { get; set; }
        public string narration { get; set; }
        public string transaction_date { get; set; }
        public string tran_response_code { get; set; }
        public string tran_response_message { get; set; }
        public string channel_code { get; set; }
        public string session_id { get; set; }
        public string transaction_type { get; set; }
    }
}
