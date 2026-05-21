//d.07.05.2024
//Lasse 
//Denne klasse er ansvarlig for at håndtere annullering af en booking.
//Den validerer først, om bookingen og kunden findes,
//og derefter kalder den cancel metoden på booking entiteten for at ændre status til annulleret.
//Til sidst opdateres bookingen i databasen og ændringerne gemmes.
using BookRight.Application.Repositories;


using BookRight.Facade.Commands.Booking;
using BookRight.Facade.Dtos.BookingCommand;

namespace BookRight.Application.Commands.BookingCommands;

public class CancelBookingCommandHandler : ICancelBookingFacade
{   
    private readonly IBookingRepository _bookingRepo; //Repository for booking, bruges til at validere om bookingen findes
    private readonly ICustomerRepository _customerRepo; //Repository for customer, bruges til at validere om kunden findes
    
    public CancelBookingCommandHandler(        
        IBookingRepository bookingRepo, 
        ICustomerRepository customerRepo)
    {       
        _bookingRepo = bookingRepo;
        _customerRepo = customerRepo;
    }

    public async Task ExecuteAsync(CancelBookingRequest request)
    {
        var booking = await _bookingRepo.GetByIdAsync(request.BookingId)
            ?? throw new Exception($"Booking with ID {request.BookingId} not found.");

        booking.Cancel(); // Kalder cancel metoden på booking entiteten for at ændre status til annulleret

        await _bookingRepo.UpdateAsync(booking); // Opdaterer bookingen i databasen
        await _bookingRepo.SaveChangesAsync(); // Gemmer ændringerne i databasen
    }
}
