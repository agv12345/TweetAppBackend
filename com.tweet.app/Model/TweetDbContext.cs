using com.tweet.app.Model;

namespace Model
{

    public class TweetDbContext : DbContext
    {

        public TweetDbContext(string getValue)
        {
            
        }
        public TweetDbContext(DbContextOptions<TweetDbContext> options):base(options)
        {
        }

        public DbSet<UserDetails> UserDetails { get; set; }

        public DbSet<Tweet> Tweets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // IConfiguration configuration = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            //     .AddJsonFile("appsettings.json").Build();
            //
            //  var connectionString = configuration.GetConnectionString("TweetAppDbConn");
            //  optionsBuilder.UseSqlServer(connectionString);
            
             
             // if (!optionsBuilder.IsConfigured)
            // {
            //     optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TwitterApp");
            // }
        }
        //
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDetails>(
                    entity =>
                    {
                        entity.HasKey(e =>
                            new
                            {
                                e.UserId
                            });
                        entity.Property(e => e.UserId).ValueGeneratedOnAdd();
                
                        entity.Property(e => e.FirstName).IsRequired().HasMaxLength(70);
                
                        entity.Property(e => e.LastName).HasMaxLength(70);
                
                        entity.Property(e => e.Password).IsRequired();
                        entity.Property(e => e.Gender)
                            .IsRequired()
                            .HasMaxLength(10);
                
                        entity.Property(e => e.EmailId).IsRequired();
                        entity.Property(e => e.DOB).HasColumnType("datetime");
                
                        entity.ToTable("Users");
                
                        entity.HasMany(t => t.Tweets).WithOne(u => u.User);
                    });
                
                modelBuilder.Entity<Tweet>(entity =>
                {
                    entity.HasKey(e => e.TweetID);
                    entity.ToTable("Tweet");
                
                    entity.Property(e => e.TweetID).ValueGeneratedOnAdd();
                    entity.Property(e => e.TweetData).HasMaxLength(200);
                
                    entity.Property(e => e.TweetTime).HasColumnType("datetime");
                
                    entity.HasOne(u => u.User).WithMany(t => t.Tweets)
                        .HasForeignKey(u => u.UserIdFK);
                });
           // OnModelCreatingPartial(modelBuilder);
            
            
        }
        //
        // private void OnModelCreatingPartial(ModelBuilder modelBuilder)
        // {
        //     throw new NotImplementedException();
        // }

    }
}