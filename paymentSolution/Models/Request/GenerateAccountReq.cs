namespace paymentSolution.Models.Request
{
    public class GenerateAccountReq
    {
        public string customer_id { get; set; }
        public string merchant_id { get; set; }
        public string customer_name { get; set; }
        public string customer_email { get; set; }
        public string customer_phone { get; set; }
        public string transaction_amount { get; set; }
        public string channel_code { get; set; }
        public string authorization { get; set; }
    }
}
