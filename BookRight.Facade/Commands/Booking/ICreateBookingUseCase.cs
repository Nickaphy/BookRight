using BookRight.Facade.Dtos;
using BookRight.Facade.Dtos.BookingCommand;
using global::BookRight.Facade.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Commands.Booking
{
    // Facade interface
    // This is the contract used by the Blazor UI
    // UI should depend on this interface instead of Application handlers directly
    // Keeps UI simple and separated from business logic



    /*
    CreateBooking.razor
    ?
    IBookingFacade
    ?
    CreateBookingCommand
    ?
    CreateBookingCommandHandler
    ?
    Booking domain model
    ?
    IBookingRepository
    ?
    BookingRepository
    ?
    BookRightDbContext
    ?
    SQL Server
    */

    // Erik´s work.


    // Facade interface
    // This is the contract used by the Blazor UI
    // UI should depend on this interface instead of Application handlers directly
    // Keeps UI simple and separated from business logic



    /*
    CreateBooking.razor
    ?
    IBookingFacade
    ?
    CreateBookingCommand
    ?
    CreateBookingCommandHandler
    ?
    Booking domain model
    ?
    IBookingRepository
    ?
    BookingRepository
    ?
    BookRightDbContext
    ?
    SQL Server
    */

    // Erik´s work.

    public interface ICreateBookingUseCase
    {
        Task<Guid> CreateBookingAsync(
            CreateBookingRequest request,
            CancellationToken cancellationToken = default);
    }

    // UI calls ICreateBooking -> Facade tranlates request into a command -> Application executes the Use Case.
}
