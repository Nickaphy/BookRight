using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Domain.Common
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}
