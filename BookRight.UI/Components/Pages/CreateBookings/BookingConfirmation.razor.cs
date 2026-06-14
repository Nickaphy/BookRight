using BookRight.Facade.Commands.Booking;
using BookRight.Facade.Dtos.CommandDto.BookingCommand;
using BookRight.Facade.Dtos.QuerryDto.BookingPricePreviewDto;
using BookRight.Facade.Dtos.QuerryDto.ClinicQuerry;
using BookRight.Facade.Dtos.QuerryDto.CustomerDtos;
using BookRight.Facade.Dtos.QuerryDto.PractitionerQuerry;
using BookRight.Facade.Dtos.QuerryDto.TreatmentTypeDtos;
using BookRight.Facade.Querries.BookingQuerries;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace BookRight.UI.Components.Pages.CreateBookings
{
    public partial class BookingConfirmation : ComponentBase
    {
        [Inject] private IBookingPricingFacade PricingFacade { get; set; } = default!;
        [Inject] private ICreateBookingUseCase CreateBookingUseCase { get; set; } = default!;
        [Parameter] public CustomerDto Customer { get; set; } = default!;
        [Parameter] public TreatmentTypeDto TreatmentType { get; set; } = default!;
        [Parameter] public PractitionerDto Practitioner { get; set; } = default!;
        [Parameter] public ClinicDto Clinic { get; set; } = default!;
        [Parameter] public PractitionerAvailableSlotDto Slot { get; set; } = default!;
        [Parameter] public EventCallback OnNewBooking { get; set; }
        

        private BookingPricePreviewDto? _pricePreview;
        private bool _isLoadingPrice = true;
        private bool _isSubmitting = false;
        private bool _bookingConfirmed = false;
        private string? _errorMessage;

        protected override async Task OnParametersSetAsync()
        {
            // Reload the price preview whenever any parameter changes
            // (e.g. user navigates back and picks a different slot).
            _pricePreview = null;
            _isLoadingPrice = true;
            _errorMessage = null;
            _bookingConfirmed = false;

            try
            {
                _pricePreview = await PricingFacade.GetPricePreviewAsync(
                    Customer.Id,
                    TreatmentType.Id,
                    Slot.Start);
            }
            catch (Exception ex)
            {
                _errorMessage = $"Kunne ikke beregne pris: {ex.Message}";
            }
            finally
            {
                _isLoadingPrice = false;
            }
        }

        private async Task ConfirmBookingAsync()
        {
            _isSubmitting = true;
            _errorMessage = null;

            try
            {
                await CreateBookingUseCase.CreateBookingAsync(new CreateBookingRequest(
                    CustomerId: Customer.Id,
                    PractitionerId: Practitioner.id,
                    ClinicId: Clinic.Id,
                    TreatmentTypeId: TreatmentType.Id,
                    StartTime: Slot.Start,
                    IsTeam: TreatmentType.MaxParticipants > 1));

                _bookingConfirmed = true;
            }
            catch (Exception ex)
            {
                _errorMessage = $"Booking mislykkedes: {ex.Message}";
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        private async Task OnNewBookingClicked()
        {
            _bookingConfirmed = false;
            await OnNewBooking.InvokeAsync();
        }

        private static string FormatDate(DateTime dt)
        {
            var da = new CultureInfo("da-DK");
            return $"{da.DateTimeFormat.GetDayName(dt.DayOfWeek)} den " +
                   $"{dt.Day}. {da.DateTimeFormat.GetMonthName(dt.Month)} {dt.Year}";
        }
    }
}