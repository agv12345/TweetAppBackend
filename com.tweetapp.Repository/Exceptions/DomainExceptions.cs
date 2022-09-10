using System.Net;
using TweetApp.Repository.ExceptionModels;

namespace TweetApp.Repository.Exceptions;

public class DomainExceptions : Exception
{
    public string ErrorMessage { get; }
    public HttpStatusCode HttpStatusCode { get; }

    public List<Error> Errors { get; } = new List<Error>();

    public DomainExceptions(string message, HttpStatusCode httpStatusCode, List<Error> errors = null): base(message)
    {
        this.ErrorMessage = message;
        this.HttpStatusCode = httpStatusCode;
        if (errors != null)
        {
            Errors.AddRange(errors);
        }
    }
}