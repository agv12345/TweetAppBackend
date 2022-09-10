using com.tweetapp.Model.Model;
using com.tweetapp.Model.Model.ViewModels;
using com.tweetapp.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using TweetApp.Controller.Controller;
using TweetApp.Repository.Repository;

namespace com.tweetapp.test
{
    [TestFixture]
    public class Tests
    {
        private Mock<ITweetRepository> _tweetQuery;
        private Mock<ITweetService> _iTweetService;
        private Mock<ILogger<TweetsController>> _logger;

        private TweetsController Controller;
        [SetUp]
        public void Setup()
        {
            _tweetQuery = new Mock<ITweetRepository>();

            _iTweetService = new Mock<ITweetService>();

            Controller = new TweetsController(_iTweetService.Object, _logger.Object);
        }
        private readonly List<TweetDetails> _tweetDetails = new List<TweetDetails>()
        {

        };
        private readonly List<TweetLikes> tweetLikes = new List<TweetLikes>()
        {

        };

       
        private readonly ResponseVM<string> postData = new ResponseVM<string>()
        {
            Data = "testing",
            Success = true,
            Message = "Succcess"
        };

        [Test]
        public async Task GetTweetsByUserName_Test()
        {
            _iTweetService.Setup(x => x.GetTweetsByUser(It.IsAny<string>())).Returns(Task.FromResult(_tweetDetails));
            var response =  Controller.GetUserTweets(It.IsAny<string>());
            Assert.NotNull(response);
           
        }

        [Test]
        public async Task GetAllTweets_Test()
        {
            _iTweetService.Setup(x => x.GetAllTweets()).Returns(Task.FromResult(_tweetDetails));
            var response =  Controller.GetAllTweets();

            Assert.NotNull(response);
            
        }


        [Test]
        public async Task DeleteTweet_Test()
        {
            _iTweetService.Setup(x => x.DeleteTweetById(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var response = Controller.DeleteTweet(It.IsAny<string>(), It.IsAny<string>());

            Assert.NotNull(response);
           // Assert.True();

        }
            


       
    }
}