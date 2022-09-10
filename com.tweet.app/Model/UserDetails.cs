namespace com.tweet.app.Model;

public class UserDetails
{
    /// <summary>
    /// User First name
    /// </summary>
    /// <param name=""></param>
    public string FirstName { get; set; }

    public int UserId { get; set; }

    public string LastName { get; set; }

    public string EmailId { get; set; }

    public DateTime DOB { get; set; }

    public string Password { get; set; }

    public ICollection<Tweet> Tweets { get; set; }

    public bool IsLoggedIn { get; set; }

    public string Gender { get; set; }
}