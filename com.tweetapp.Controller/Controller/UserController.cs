using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using com.tweetapp.Model.Model;
using com.tweetapp.Model.Model.ViewModels;
using TweetApp.Repository.Exceptions;
using com.tweetapp.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authorization;

namespace TweetApp.Controller.Controller
{
    [Route("api/v1.0/tweets")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public IConfiguration _configuration;


        public UserController(IUserService userService, ILogger<UserController> logger, IConfiguration config)
        {
            _configuration = config;
            _userService = userService;
            _logger = logger;
        }


        [Route("users/all")]
        [HttpGet]
        public ActionResult GetAllUser()
        {
            var result = _userService.GetAllUsers().Result;
            _logger.LogInformation("GetAll User Details", result);
            return Ok(result);
        }
        
        [Route("user/search/{username}")]
        [HttpGet]
        public ActionResult SearchByUser(string username)
        {
            var result = _userService.GetByUserName(username).Result;
            return Ok(result);
        }
        
        
        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register([FromBody] UserRegistrationViewModel user)
        {
            _logger.LogInformation("Staring user Registration");
            if (user == null)
            {
                _logger.LogError("Error! Cannot register");
                throw new DomainExceptions("Invalid Request", System.Net.HttpStatusCode.BadRequest);
            }
            var result = _userService.Register(user).Result;
            return Ok(result);
        }
        
        [Route("{userName}/forgot")]
        [HttpPut]
        public ActionResult Forgot([FromRoute] string userName, [FromBody] UserPasswordUpdate update)
        {   
            // UserLogin userLogin = new UserLogin
            // {
            //     EmailId =  userName,
            //     Password = oldPassword
            // };
            // string userName = update.EmailId;
            string oldPassword = update.OldPassword;
            string newPassword = update.NewPassword;
            
            var res = _userService.ResetPassword(userName, oldPassword, newPassword).Result;
            if (res)
            {
                return Ok("Password Updated Successfully");
            }
            return NotFound("Username/password doesn't match");
        }

        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> token(UserLogin _userData)
        {
            if (_userData != null && _userData.EmailId != null && _userData.Password != null)
            {
                var user = _userService.LoginAuthentication(_userData.EmailId, _userData.Password).Result;

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("DisplayName", user.FirstName+user.LastName),
                        new Claim("UserName", user.EmailId),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(40),
                        signingCredentials: signIn);

                    var ntoken = new JwtSecurityTokenHandler().WriteToken(token);
                    registeredUsers registeredUsers = new registeredUsers
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        profileString = user.profileString,
                        Username = user.EmailId
                    };

                    var res = new ResponseVM<registeredUsers>()
                    {
                        Token = ntoken,
                        Data = registeredUsers,
                        Success = true,
                        Message = "Successfully login"
                    };
                    using (var producer =
              new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "kafka:9092" }).Build())
                    {
                        try
                        {
                            Console.WriteLine(producer.ProduceAsync("Tweet", new Message<Null, string> { Value = res.Data.FirstName + " logged in." })
                                .GetAwaiter()
                                .GetResult());


                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Oops, something went wrong: {e}");
                        }

                    };
                    return Ok(res);
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
