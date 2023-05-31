namespace WeRaven.Api.Models
{
    public class FriendFolderUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public FriendFolder FriendFolder { get; set; }
        public Guid FriendFolderId { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
