using com.tweetapp.Model.Model;
using com.tweetapp.Model.Model.ViewModels;

namespace TweetApp.Repository.Repository;

/// <summary>
/// 
/// </summary>
public interface ITweetRepository : IRepository<TweetDetails>
{
   
    /// <summary>
    /// GetAllTweet
    /// </summary>
    /// <returns></returns>
    Task<List<TweetDetails>> GetAllTweets();
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tweet"></param>
    /// <param name="emailId"></param>
    /// <returns></returns>
    
    
    TweetDetails PostTweet(TweetDetails tweet, string emailId);
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="emailId"></param>
    /// <returns></returns>
    Task<List<TweetDetails>> GetTweetByUserId(string emailId);

    Task<int> UpdateTweet(UserDetails userDetails,UpdateTweet tweetDetails);

    Task<bool> DeleteTweet(string emailId, TweetDetails tweetDetails);
    Task<bool> DeleteTweetById(string emailId, string tweetID);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tweetDetails"></param>
    void Update(TweetDetails tweetDetails);

    Task<bool> TweetLike(string emailId, TweetDetails tweetDetails);
    Task<TweetLikes> TweetLikeById(string emailId, string tweetId);
    Task<int> TweetReply(string emailId, int Id, TweetMessage message);
    Task<TweetDetails> UpdateTweetById(string tweetId, UpdateTweet tweetDetails, string emailId);


    public int GetLikesCount(int tweetId);

    Task <List<TweetReply>> GetReplies(int tweetId);
}