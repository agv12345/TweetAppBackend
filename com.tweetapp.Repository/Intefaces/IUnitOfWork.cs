namespace TweetApp.Repository.Repository;

public interface IUnitOfWork
{
    IUserRepository User { get; }
    ITweetRepository Tweet { get; }

    Task Save();
}