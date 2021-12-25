using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using serssly.Models;

namespace serssly.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Feed> Feeds => Set<Feed>();

        public DbSet<UserFeed> UserFeeds => Set<UserFeed>();

        public DbSet<FeedItem> FeedItems => Set<FeedItem>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserFeed>()
                .HasKey(uf => new { uf.UserId, uf.FeedId })
                .IsClustered();

            builder.Entity<UserFeed>()
                .HasOne(uf => uf.Feed)
                .WithMany(f => f.UserFeeds)
                .HasForeignKey(uf => uf.FeedId);

            builder.Entity<FeedItem>()
                .HasOne(fi => fi.Feed)
                .WithMany(f => f.FeedItems)
                .HasForeignKey(fi => fi.FeedId);
        }
    }
}
