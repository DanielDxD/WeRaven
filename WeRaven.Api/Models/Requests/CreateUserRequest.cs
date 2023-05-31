namespace WeRaven.Api.Models.Requests
{
    public class CreateUserRequest
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Gender { get; set; } = null;
        public string Birthdate { get; set; }
    }
}
