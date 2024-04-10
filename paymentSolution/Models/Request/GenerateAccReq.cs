namespace paymentSolution.Models.Request
{
    public class GenerateAccReq
    {
        public string Email { get; set; }
        public string amount { get; set; }
        public string channelcode { get; set; }
        public string authorization { get; set; }
    }
}
