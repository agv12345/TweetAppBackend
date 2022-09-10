using com.tweetapp.Model.Model;
using com.tweetapp.Model.Model.ViewModels;

namespace TweetApp.Domain.Validators
{
    using FluentValidation;

    /// <summary>
    /// TweetValidator Class
    /// </summary>
    public class TweetMessageValidator : AbstractValidator<TweetDetails>
    {
        public TweetMessageValidator(postTweetViewModel tweetMessage)
        {
            RuleFor(x => x.TweetData)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Tweet Reply cannot be blank.")
                .Length(0, 200)
                .WithMessage("Tweet Message cannot be more than 200 characters.");
        }
    }
}
