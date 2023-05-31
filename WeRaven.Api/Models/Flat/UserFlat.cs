using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using WeRaven.Api.Models.Helpers;

namespace WeRaven.Api.Models.Flat
{
    public class UserFlat
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePhoto { get; set; } = "none.png";
        public string Bio { get; set; } = "";
        public string? Gender { get; set; } = null;
        public List<Guid> Followers { get; set; } = new();
        public List<Guid> Following { get; set; } = new();
        public List<Guid> Blocklist { get; set; } = new();
        public bool IsPrivate { get; set; } = false;
        public OSettings Settings { get; set; } = new();
        public DateTime Birthdate { get; set; }
        public bool Confirmed { get; set; } = false;
        public bool Verified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
