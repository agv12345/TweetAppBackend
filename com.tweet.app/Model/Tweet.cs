namespace com.tweet.app.Model;

public class Tweet
{
    public int TweetID { get; set; }

    public string TweetData { get; set; }

    public int UserIdFK { get; set; }

    public virtual UserDetails User { get; set; }

    public DateTime TweetTime { get; set; }
}