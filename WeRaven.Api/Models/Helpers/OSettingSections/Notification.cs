using WeRaven.Api.Models.Helpers.OSettingSections.Others;

namespace WeRaven.Api.Models.Helpers.OSettingSections
{
    public class Notification
    {
        public bool PauseNotifications { get; set; } = false;
        public SilentMode? SilentMode { get; set; } = null;
        public bool FlashLikes { get; set; } = true;
        public bool LikeAndCommentsWithMe { get; set; } = true;
        public bool PhotosWithYou { get; set; } = true; 
        public bool Comments { get; set; } = true; 
        public bool FixedAndLinkesInComments { get; set; } = true; 
        public bool FollowRequest { get; set; } = true;
        public bool AcceptedRequest { get; set; } = true; 
        public bool AccountSuggestion { get; set; } = false;
        public bool BioMentions { get; set; } = true;
        public bool ContactRequest { get; set; } = true;
        public bool Messages { get; set; } = true;
        public bool GroupRequest { get; set; } = true;
        public bool Calls { get; set; } = true;
        public bool Lives { get; set; } = true;
        public bool NewQuickVi { get; set; } = true;
        public bool EmailNotifications { get; set; } = true;
    }
}
