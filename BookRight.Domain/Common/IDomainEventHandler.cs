using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Domain.Common
{
    public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
    {
        Task Handle(TEvent domainEvent, CancellationToken cancellationToken = default);
    }
}
