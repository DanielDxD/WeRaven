using WeRaven.Api.Models.Helpers.OSettingSections;

namespace WeRaven.Api.Models.Helpers
{
    public class OSettings
    {
        public Notification Notification { get; set; } = new();
        public List<Guid> HideFlashAndLive { get; set; } = new();
    }
}
