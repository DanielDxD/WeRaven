namespace WeRaven.Api.Models.Requests
{
    public class VerifyRequest
    {
        public string Email { get; set; }
        public int Code { get; set; }
    }
}
