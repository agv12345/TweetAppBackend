using com.tweetapp.Services.Interfaces;
using FluentValidation.Internal;
using Microsoft.Extensions.Configuration;
using com.tweetapp.Model.Model;
using com.tweetapp.Model.Model.ViewModels;
using TweetApp.Repository.Repository;
using TweetApp.Repository.Validations;

namespace com.tweetapp.Services.Services;

public class TweetServices : ITweetService
{
    private readonly ITweetRepository _tweetRepository;

    //private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public TweetServices(ITweetRepository tweetRepository, IConfiguration configuration)
    {
        _tweetRepository = tweetRepository;
        _configuration = configuration;

    }

    public async Task<List<TweetDetails>> GetTweetsByUser(string username)
    {
        return _tweetRepository.GetTweetByUserId(username).Result;
        
    }

    public async Task<TweetDetails> PostTweet(postTweetViewModel postTweetDetails, string emailId)
    {
        var tweetDetails = new TweetDetails
        {
            TweetData = postTweetDetails.TweetData,
            TweetTime = DateTime.Now
        };
        Validations.EnsureValidTweet(tweetDetails, new TweetValidator(tweetDetails));

        //var user = _unitOfWork.User.GetUserProfile(emailId).Result;
        //tweetDetails.User = user;
        return _tweetRepository.PostTweet(tweetDetails, emailId);
    }

    public bool DeleteTweetById(string tweetId, string emailId)
    {
        var res=  _tweetRepository.DeleteTweetById(emailId, tweetId);
        return res.Result;
        
    }

    public async Task<List<TweetDetails>> GetAllTweets()
    {
        return _tweetRepository.GetAllTweets().Result;
    }

    public int LikesCount(int tweetId)
    {
        var likes = _tweetRepository.GetLikesCount(tweetId);

        return likes;
    }

    public TweetLikes LikeTweetById(string tweetId, string username)
    {
        return _tweetRepository.TweetLikeById(username, tweetId).Result;
    }

    public async Task<TweetDetails> EditTweet(string emailId, string tweetId, UpdateTweet tweetDetails)
    {
        var tweetUp = _tweetRepository.UpdateTweetById(tweetId, tweetDetails, emailId);

        return tweetUp.Result;
    }

    public async Task<List<TweetReply>> GetTweetReply(int tweetID)
    {
        var res =  await _tweetRepository.GetReplies(tweetID);
        return res;
    }
    public async Task<ReplyResponse<int>> ReplyTweet(string username, string id, TweetMessage message)
    {
        int tweetId = Int32.Parse(id);
        var res = await _tweetRepository.TweetReply(username, tweetId, message);
        ReplyResponse<int> reply = new ReplyResponse<int>();
        if (res > 0)
        {
            reply.Message = "Successfully replied";
            reply.Success = true;
            reply.Status = 200;
            reply.Data = 1;
        }

        else
        {
            reply.Message = "Something went wrong";
            reply.Success = false;
            reply.Status = 404;
            reply.Data = -1;
        }
        return reply;
}
}