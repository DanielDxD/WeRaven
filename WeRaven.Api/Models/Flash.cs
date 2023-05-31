namespace WeRaven.Api.Models
{
    public class Flash
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public User User { get; set; }
        public Guid UserId { get; set; }
        public FriendFolder? FriendFolder { get; set; }
        public Guid? FriendFolderId { get; set; }
        public Media Media { get; set; }
        public Guid MediaId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
