namespace WeRaven.Api.Models
{
    public class PostLike
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Post Post { get; set; }
        public Guid PostId { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
