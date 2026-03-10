namespace Loan_Procedure.Utils
{
    public class Response
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;

        public static Response Ok(string Message="")
        {
            return new Response { Status = true, Message = Message };
        }
        public static Response Fail(string Message="")
        {
            return new Response { Status = false, Message = Message };
        }
    }
}
