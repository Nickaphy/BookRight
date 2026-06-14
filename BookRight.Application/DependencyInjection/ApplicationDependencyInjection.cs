using BookRight.Application.QuerryHandlers.Pricing;
using BookRight.Application.UseCases.BookingCommands;
using BookRight.Application.UseCases.CampaignUseCases;
using BookRight.Application.UseCases.ClinicUseCases;
using BookRight.Application.UseCases.CustomerCommands;
using BookRight.Application.UseCases.PractitionerUseCases;
using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Application.UseCases.Services.DiscountStrategy;
using BookRight.Application.UseCases.Services.PriceCalculator;
using BookRight.Domain.Common;
using BookRight.Domain.DomainServices.BookingConfictChecker;
using BookRight.Domain.Entities.Bookings;
using BookRight.Facade.Commands.Booking;
using BookRight.Facade.Commands.Campaign;
using BookRight.Facade.Commands.Clinic;
using BookRight.Facade.Commands.CustomerCommands;
using BookRight.Facade.Commands.Practitioner;
using BookRight.Facade.Querries.BookingQuerries;
using Microsoft.Extensions.DependencyInjection;

namespace BookRight.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Customer 
            services.AddScoped<ICreateCustomer, CreateCustomerHandler>();
            services.AddScoped<IUpdateCustomer, UpdateCustomerHandler>();
            services.AddScoped<IDeleteCustomer, DeleteCustomerHandler>();

            // Practitioner 
            services.AddScoped<ICreatePractitioner, CreatePractitionerHandler>();
            services.AddScoped<IUpdatePractitioner, UpdatePractitionerHandler>();
            services.AddScoped<IDeletePractitioner, DeletePractitionerHandler>();
            services.AddScoped<IAssignPractitionerToClinic, AssignPractitionerToClinicHandler>();

            // Clinic
            services.AddScoped<ICreateClinic, CreateClinicHandler>();
            services.AddScoped<IUpdateClinic, UpdateClinicHandler>();
            services.AddScoped<IDeleteClinic, DeleteClinicHandler>();

            // Campaign
            services.AddScoped<ICreateCampaign, CreateCampaignHandler>();
            services.AddScoped<IUpdateCampaign, UpdateCampaignHandler>();
            services.AddScoped<IDeleteCampaign, DeleteCampaignHandler>();

            // Booking
            services.AddScoped<ICreateBookingUseCase, CreateBookingCommandHandler>();
            services.AddScoped<ICancelBookingFacade, CancelBookingCommandHandler>();
            services.AddScoped<IMarkAsNoShowUseCase, MarkAsNoShowCommandHandler>();
            services.AddScoped<ICompleteBookingUseCase, CompleteBookingCommandHandler>();
            services.AddScoped<IMarkAsNoShowUseCase, MarkAsNoShowCommandHandler>();

            // Discount strategies
            // Each strategy is registered against the IDiscountStrategy interface
            // so DiscountService receives all of them via IEnumerable<IDiscountStrategy>.
            services.AddScoped<IDiscountStrategy, LoyaltyLevelNone>();
            services.AddScoped<IDiscountStrategy, LoyaltyLevelBronze>();
            services.AddScoped<IDiscountStrategy, LoyaltyLevelSilver>();
            services.AddScoped<IDiscountStrategy, LoyaltyLevelGold>();
            services.AddScoped<IDiscountStrategy, BirthMonthDiscount>();

            //Services 
            services.AddScoped<IBookingConflictChecker, BookingConflictChecker>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IPriceCalculator, PriceCalculator>();

            // Pricing preview facade
            services.AddScoped<IBookingPricingFacade, BookingPricingFacadeHandler>();


            services.AddScoped<IDomainEventHandler<BookingCompletedEvent>, UpdateCustomerLoyaltyLevelHandler>();

            return services;
        }
    }
}