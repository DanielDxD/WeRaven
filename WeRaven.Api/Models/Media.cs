using System.ComponentModel.DataAnnotations.Schema;

namespace WeRaven.Api.Models
{
    public class Media
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Url { get; set; }
        public string MimeType { get; set; }
        public string? Alt { get; set; } = null;
        [ForeignKey("MediaId")]
        public List<PostMedia> PostMedias { get; set; } = new();
        [ForeignKey("MediaId")]
        public List<Flash> Flashes { get; set; } = new();
    }
}
