﻿namespace TweetApp.Repository.Validations
{
    using FluentValidation;
    using com.tweetapp.Model.Model;

    /// <summary>
    /// TweetValidator Class
    /// </summary>
    public class TweetValidator : AbstractValidator<TweetDetails>
    {
        public TweetValidator(TweetDetails tweet)
        {
            RuleFor(x => x.TweetData)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Tweet Message cannot be blank.")
                .Length(0, 200)
                .WithMessage("Tweet Message cannot be more than 200 characters.");
        }
    }
}
