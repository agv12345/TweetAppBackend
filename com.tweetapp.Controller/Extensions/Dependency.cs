using com.tweetapp.Model.Model;
using Microsoft.Extensions.DependencyInjection;
namespace TweetApp.Controller.Extensions;

using com.tweetapp.Services.Interfaces;
using com.tweetapp.Services.Services;
using TweetApp.Repository.Repository;

public static class Dependency
{
    public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
    {
        //services.AddScoped<IServices, UserService>();
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<ITweetService,TweetServices>();
        services.AddScoped<ITweetRepository, TweetRepository<TweetDetails>>();
        services.AddScoped<IUserRepository, UserRepository<UserDetails>>();
        
        
        // services.AddScoped<IUserService, UserService>();
        // services.AddScoped<, UserRepo>();
        // services.AddScoped<ITweetRepo, TweetRepo>();
        // services.AddScoped<ITweetService, TweetService>();
        // //services.AddTransient<ExceptionHandlerMiddleware>();

        return services;
    }
}