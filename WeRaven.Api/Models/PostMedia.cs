namespace WeRaven.Api.Models
{
    public class PostMedia
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Post Post { get; set; } 
        public Guid PostId { get; set; }
        public Media Media { get; set; }
        public Guid MediaId { get; set; }
    }
}
