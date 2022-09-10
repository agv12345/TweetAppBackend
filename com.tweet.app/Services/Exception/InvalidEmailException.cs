namespace com.tweet.app.Services.Exception;

public class InvalidEmailException : System.Exception
{
    public InvalidEmailException()
    {
    }

    public InvalidEmailException(string emailId) : base($"Invalid Email Id: {emailId}")
    {
    }
}