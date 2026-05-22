
using BookRight.Domain.Common;
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace BookRight.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(@"Server=.\SQLEXPRESS;Database=BookRightTest1;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        return new AppDbContext(options, new NoOpDomainEventDispatcher());
    }
}

public class NoOpDomainEventDispatcher : IDomainEventDispatcher
{
    public Task Dispatch(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
