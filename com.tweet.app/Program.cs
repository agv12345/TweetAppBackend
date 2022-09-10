
using TweetApp.Services.Interfaces;

public class Program
{
    static void Main(string[] args)
    {
        // ServiceCollection services = new ServiceCollection();
        // Configure(services);
        // IServiceProvider provider = services.BuildServiceProvider();
        // var service = provider.GetService<IUserIntro>();
        // service.IntroPageNonLoggedUser();
        
        var serviceCollection = new ServiceCollection();

         IConfiguration configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
             .AddJsonFile("appsettings.json").Build();
        
         serviceCollection.AddSingleton<IConfiguration>(configuration);
        //serviceCollection.AddDbContext<TweetDbContext>(o =>
            //o.UseSqlServer(configuration.GetConnectionString("TweetAppDbConn")));
        //serviceCollection.AddSingleton<IUserRepository, UserRepository>();
        //serviceCollection.AddSingleton<ITweetRepository, TweetRepository>();
        //serviceCollection.AddScoped<IUserService, UserService>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var welcome = serviceProvider.GetService<IUserService>();
        
        welcome.IntroAllUsers();
        
    }
    
    
    
}