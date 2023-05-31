using Microsoft.EntityFrameworkCore;
using WeRaven.Api.Models;

namespace WeRaven.Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Auth> Auths { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Flash> Flashes { get; set; }
        public DbSet<FriendFolder> FriendFolders { get; set; }
        public DbSet<FriendFolderUser> FriendFolderUsers { get; set; }
        public DbSet<Media> Medias { get; set; } 
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostMarkup> PostMarkups { get; set; }
        public DbSet<PostMedia> PostMedias { get; set; }
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions)
        { }
    }
}
