// Command object
// Represents an intention to change system state
// Example: create or cancel a booking



/*
CreateBooking.razor
↓
IBookingFacade
↓
CreateBookingCommand
↓
CreateBookingCommandHandler
↓
Booking domain model
↓
IBookingRepository
↓
BookingRepository
↓
BookRightDbContext
↓
SQL Server
*/

// Eriks work

namespace BookRight.Application.Commands.BookingCommands;

public sealed record CreateBookingCommand(
    Guid CustomerId,
    Guid PractitionerId,
    Guid ClinicId,
    Guid TreatmentTypeId,
    DateTime StartTime
);


/*public class CreateBookingCommand : ICreateBookingCommand
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPractiotionerRepository _practiotionerRepository;
    private readonly ICampaignRepository _campaignRepository;
    private readonly IClinicRepository _clinicRepository;


    public CreateBookingCommand(IBookingRepository bookingRepository,
        ICustomerRepository customerRepository,
        IPractiotionerRepository practiotionerRepository,
        ICampaignRepository campaignRepository,
        IClinicRepository clinicRepository)
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _practiotionerRepository = practiotionerRepository;
        _campaignRepository = campaignRepository;
        _clinicRepository = clinicRepository;
    }

    public async Task(CreateBookingCommandRequest request)
    {

        // Here we have some exceptions, that we eventually will make a didicated Exception handler, to take care of.
    
        _ = await _customerRepository.GetAsync(request._customerId)
            ?? throw new NotFoundException("Customer was not found.");
        _ = await _practitionerRepository.GetAsync(request._practiotionerId)
            ?? throw new NotFoundException("Practitioner was not found.");
        _ = await _campaignRepository.GetAsync(request._campaignId)
            ?? throw new NotFoundException("Campaign was not found.");
        _ = await _clinicRepository.GetAsync(request._clinicId)
            ?? throw new NotFoundException("Clinic was not found.");
 

    }
}
*/
