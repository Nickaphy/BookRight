using BookRight.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Domain.Common;
using BookRight.Domain.Entities.Bookings;

namespace BookRight.Application.Commands.CustomerCommands
{
    public class UpdateCustomerLoyaltyLevelHandler : IDomainEventHandler<BookingCompletedEvent>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICustomerRepository _customerRepository;

        public async Task Handle(BookingCompletedEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var totalSpent = await _bookingRepository
                .GetTotalSpentLastYearAsync(domainEvent.CustomerId, cancellationToken);

            var customer = await _customerRepository
                .GetCustomerByIdAsync(domainEvent.CustomerId, cancellationToken);

            customer.UpdateLoyaltyLevel(totalSpent);

            await _customerRepository.UpdateCustomerAsync(customer, cancellationToken);  
            await _customerRepository.SaveAsync(cancellationToken);
        }
    }
}
