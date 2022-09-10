using com.tweetapp.Model.Context;
using com.tweetapp.Model.Model;

namespace TweetApp.Repository.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly TweetAppDbContext _dbContext;
    public IUserRepository User { get; private set; }
    public ITweetRepository Tweet { get; private set; }

    public UnitOfWork(TweetAppDbContext dbContext)
    {
        _dbContext = dbContext;

        User = new UserRepository<UserDetails>(_dbContext);
        Tweet = new TweetRepository<TweetDetails>(_dbContext);
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}