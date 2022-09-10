using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using Confluent.Kafka;
using com.tweetapp.Model.Model;
using com.tweetapp.Model.Model.ViewModels;
using TweetApp.Repository.Exceptions;
using com.tweetapp.Services.Interfaces;
using Microsoft.DotNet.MSIdentity.Shared;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.AspNetCore.Authorization;

namespace TweetApp.Controller.Controller
{
    [Route("api/v1.0/tweets")]
    [Authorize]
    [ApiController]
    public class TweetsController : ControllerBase
    {

        private readonly ITweetService _tweetService;
        private readonly ILogger<TweetsController> _logger;


        public TweetsController(ITweetService tweetService, ILogger<TweetsController> logger)
        {
            _tweetService = tweetService;
            _logger = logger;
        }
        
        [Route("{username}")]
        [HttpGet]
        public ActionResult GetUserTweets(string username)
        {
            var result = _tweetService.GetTweetsByUser(username).Result;
            _logger.LogInformation("GetTweetsByUsers", result);
            if (result != null)
            {
                var tweetJ = JsonSerializer.Serialize(result);
                //var tweets = JsonSerializer.Deserialize<List<TweetDetails>>(result);
                return Ok(tweetJ);
            }

            return NotFound();
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("all")]
        [HttpGet]
        public ActionResult GetAllTweets()
        {
            var result = _tweetService.GetAllTweets().Result;
            _logger.LogInformation("GetAllTweets", result);
            List<tweetLikes> listTweets = new List<tweetLikes>(); 
            
            foreach (var tweet in result)
            {
                var count = _tweetService.LikesCount(tweet.TweetID);
                var tweetReply = _tweetService.GetTweetReply(tweet.TweetID).Result;
                var tweetsLike = new tweetLikes
                {
                    TweetDetails = tweet,
                    Likes = count,
                    Replies = tweetReply
                };
                listTweets.Add(tweetsLike);
                _logger.LogInformation("GetLikesCount", count);
                _logger.LogInformation("GetLikedTweet", tweetsLike);

            }
            //var tweetJ = JsonSerializer.Serialize(result);
            
            // var response = new ResponseVM<TweetDetails>
            // {
            //     Data = result,
            //     
            //         
            // }
            return Ok(listTweets);
        }
        
        [Route("{username}/add")]
        [HttpPost]
        public ActionResult PostTweet(string username, [FromBody] postTweetViewModel tweet)
        {
            if (tweet == null)
            {
                throw new DomainExceptions("Invalid Request", System.Net.HttpStatusCode.BadRequest);
            }
            var result = _tweetService.PostTweet(tweet,username).Result;
            _logger.LogInformation("PostTweet", result);

            using (var producer =
               new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "kafka:9092" }).Build())
            {
                try
                {
                    Console.WriteLine(producer.ProduceAsync("Tweet", new Message<Null, string> { Value = result.User.FirstName + " posted a tweet." })
                        .GetAwaiter()
                        .GetResult());


                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went wrong: {e}");
                }

            };
            return Ok(result);
        }
        
        
        [Route("{username}/update/{id}")]
        [HttpPut]
        public ActionResult UpdateTweet(string username, string id, [FromBody] UpdateTweet tweet)
        {
            if (tweet == null)
            {
                throw new DomainExceptions("Invalid Request", System.Net.HttpStatusCode.BadRequest);
            }
            var result = _tweetService.EditTweet(username,id,tweet).Result;
            _logger.LogInformation("Update a Tweet", result);

            if (result != null)
            {
                return Ok();
            }
            _logger.LogInformation("Update a Tweet - Tweet not found Cannot update", result);
            return NotFound("Cannot update");
        }
        
        [Route("{username}/like/{id}")]
        [HttpPut]
        public ActionResult LikeTweet(string id, string username)
        {
            var result = _tweetService.LikeTweetById(id,username);
            _logger.LogInformation("LikeTweet", result);

            if (result != null)
            {
                return Ok(result);
            }

            else
            {
                return Ok("cannot like a tweet twice");
            }
        }
        
        [Route("{username}/reply/{id}")]
        [HttpPost]
        public ActionResult ReplyTweet(string username, string id, [FromBody] TweetMessage message)
        {
            if (message == null)
            {
                throw new DomainExceptions("Invalid Request", System.Net.HttpStatusCode.BadRequest);
            }
            var result = _tweetService.ReplyTweet(username, id, message).Result;
            _logger.LogInformation("ReplyTweet", result);

            return Ok(result);
        }
        
        [Route("{username}/delete/{id}")]
        [HttpDelete]
        public ActionResult DeleteTweet(string id, string username)
        {
            _logger.LogInformation("Delete Tweet:", id, username);
            using (var producer =
                 new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "kafka:9092" }).Build())
            {
                try
                {
                    Console.WriteLine(producer.ProduceAsync("Tweet", new Message<Null, string> { Value = username + " Deleted a Tweet with id " + id })
                        .GetAwaiter()
                        .GetResult());


                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went wrong: {e}");
                }
            };
            return Ok(_tweetService.DeleteTweetById(id,username));
        }
        
        
        //
        // // GET: api/Tweets
        // [HttpGet]
        // public IEnumerable<string> Get()
        // {
        //     return new string[] { "value1", "value2" };
        // }
        //
        //
        //
        // // GET: api/Tweets/5
        // [HttpGet("{id}", Name = "Get")]
        // public string Get(int id)
        // {
        //     return "value";
        // }
        //
        // // POST: api/Tweets
        // [HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }
        //
        // // PUT: api/Tweets/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }
        //
        // // DELETE: api/Tweets/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}
