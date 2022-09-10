using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Newtonsoft.Json;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Swashbuckle.AspNetCore.Filters;
using TweetApp.Controller.Extensions;
using com.tweetapp.Model.Context;

var builder = WebApplication.CreateBuilder(args);
    
    ConfigureLogs();


    builder.Host.UseSerilog();

    builder.Services.AddControllers()
    .AddJsonOptions(c =>
    {
        c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

    builder.Services.AddMyDependencyGroup();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
    {
        //builder.WithOrigins("http://localhost:3000")
        //    .AllowAnyMethod()
        //    .AllowAnyHeader();
        //builder.WithOrigins("http://localhost:3001")
        //  .AllowAnyMethod()
        //  .AllowAnyHeader();
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    }));

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "TWEET APP",
            Description = "Tweet API",
        });

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });

    builder.Services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();

    app.UseHttpsRedirection();
    //add later
    // app.UseMiddleware<ExceptionHandlerMiddleware>();

    app.UseCors("MyPolicy");
    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();




    // var services = new ServiceCollection();
    //
    //
    // //builder.Services.AddScoped<IUserRepository,UserRepository<UserDetails>>();
    // services.AddSingleton<IUserService<UserDetails>, UserService>();


#region helper
void ConfigureLogs()
{
    // Get the environment which the application is running on
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    // Get the configuration 
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    
    builder.Services.AddDbContext<TweetAppDbContext>(o =>
        o.UseSqlServer(configuration.GetConnectionString("TweetAppDbConn")));


    // Create Logger
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails() // Adds details exception
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureELS(configuration, env))
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureELS(IConfigurationRoot configuration, string env)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ELKConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        //IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-{env.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}
#endregion
