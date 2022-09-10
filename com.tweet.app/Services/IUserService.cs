namespace com.tweet.app.Services;

public interface IUserService
{
    // most methods are declared for use in other classes.
    public void Welcome();
    public void IntroAllUsers();
    public bool UserRegistration();
    public void UserMenu();

    public void ForgetPassword();

    public void ResetPassword();

    public void PostTweet(string emailId);
    public void GetMyTweets(string emailId);
    public void GetAllTweets();

    public void UserLogout(string emailId);
}