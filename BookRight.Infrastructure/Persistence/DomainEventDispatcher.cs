using BookRight.Domain.Common;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Infrastructure.Persistence
{
    
        public class DomainEventDispatcher : IDomainEventDispatcher
        {
            private readonly IPublisher _publisher;

            public DomainEventDispatcher(IPublisher publisher)
            {
                _publisher = publisher;
            }

            public async Task Dispatch(
                IDomainEvent domainEvent,
                CancellationToken cancellationToken = default)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }
        }
    }

