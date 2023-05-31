using System.ComponentModel.DataAnnotations.Schema;
using WeRaven.Api.Models.Helpers;

namespace WeRaven.Api.Models
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public User User { get; set; } 
        public Guid UserId { get; set; }
        public FriendFolder? FriendFolder { get; set; }
        public Guid? FriendFolderId { get; set; }
        public string Content { get; set; } = "";
        [Column(TypeName = "jsonb")]
        public OLocation? Location { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        [ForeignKey("PostId")]
        public List<PostMarkup> PostMarkups { get; set; } = new();
        [ForeignKey("PostId")]
        public List<PostMedia> PostMedias { get; set; } = new();
        [ForeignKey("PostId")]
        public List<PostLike> PostLikes { get; set; } = new();
    }
}
