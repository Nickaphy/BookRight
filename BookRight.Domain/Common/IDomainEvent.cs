using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace BookRight.Domain.Common
{
    public interface IDomainEvent : INotification
    {
    }
}
