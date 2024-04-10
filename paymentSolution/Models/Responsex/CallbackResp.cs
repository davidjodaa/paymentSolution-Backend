namespace paymentSolution.Models.Responsex
{
    public class CallbackResp<T>
    {
        public CallbackResp()
        {
        }
        public CallbackResp(T response_code, string response_message = null)
        {
            Succeeded = true;
            Response_message = response_message;
            Response_code = response_code;
        }

        public CallbackResp(string response_message, bool succeeded)
        {
            Succeeded = succeeded;
            Response_message = response_message;
        }
        public CallbackResp(string response_message)
        {
            Succeeded = true;
            Response_message = response_message;
        }
        public bool Succeeded { get; set; }
        public string Response_message { get; set; }
        public List<string> Errors { get; set; }
        public T Response_code { get; set; }
    }
}
