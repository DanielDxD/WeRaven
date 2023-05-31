namespace WeRaven.Api.Models
{
    public class CommentLike
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PostComment Comment { get; set; }
        public Guid CommentId { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
