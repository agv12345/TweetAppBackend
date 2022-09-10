using com.tweetapp.Model.Model;
using Microsoft.EntityFrameworkCore;

namespace com.tweetapp.Model.Context
{
    public class TweetAppDbContext:DbContext

    {
        public TweetAppDbContext()
        {
            
        }
        public TweetAppDbContext(DbContextOptions<TweetAppDbContext> options):base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Initial Catalog=TweetAppComp3; Data Source=localhost, 1435; Persist Security Info=True;User ID=sa;Password=Testing123");
            }
        }
        public DbSet<UserDetails> UserDetails { get; set; }

        public DbSet<TweetDetails> Tweets { get; set; }

        public DbSet<TweetReplies> TweetReplies { get; set; }
        public DbSet<TweetLikes> TweetLikes { get; set; }
    
    }

}
