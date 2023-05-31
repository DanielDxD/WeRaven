using System.ComponentModel.DataAnnotations.Schema;

namespace WeRaven.Api.Models
{
    public class FriendFolder
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public User User { get; set; } 
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; } = "#483eb7";
        [ForeignKey("FriendFolderId")]
        public List<Post> Posts { get; set; } = new();
        [ForeignKey("FriendFolderId")]
        public List<FriendFolderUser> FriendFolderUsers { get; set; } = new();
        [ForeignKey("FriendFolderId")]
        public List<Flash> Flashes { get; set; } = new();
    }
}
