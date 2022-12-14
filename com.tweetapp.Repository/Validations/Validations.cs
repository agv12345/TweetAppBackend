using TweetApp.Repository.ExceptionModels;
using TweetApp.Repository.Exceptions;
using FluentValidation;
namespace TweetApp.Repository.Validations;

public class Validations
{
    public static void EnsureValidTweet<TRequest>(TRequest request, IValidator<TRequest> validator)
    {
        var validationError = new DomainExceptions("Invalid Request", System.Net.HttpStatusCode.BadRequest);
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            validationError.Errors.AddRange(
                validationResult.Errors.Select(
                    validationFailure => new Error(validationFailure.ErrorMessage)
                ));
            throw validationError;
        }
    }
}