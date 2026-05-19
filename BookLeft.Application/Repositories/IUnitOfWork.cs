using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Repositories
{
    public interface IUnitOfWork
    {
        // Commits all changes staged by repositories during this request
        // to the database in a single atomic transaction.
        //
        // Call this ONCE at the end of a Use Case handler, after all
        // repository operations have been staged.
        //
        // The number of database rows affected (mirrors EF Core's return value).

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
