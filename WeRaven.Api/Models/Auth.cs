using WeRaven.Api.Tools;

namespace WeRaven.Api.Models
{
    public class Auth
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public User User { get; set; }
        public Guid UserId { get; set; }
        public int Code { get; set; } = MathTool.GenerateCode();
        public DateTime ExpiryAt { get; set; } = DateTime.UtcNow.AddHours(1);
    }
}
