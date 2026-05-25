using BookRight.Application.Commands.BookingCommands;
using BookRight.Application.Commands.CustomerCommands;
using BookRight.Application.Services;
using BookRight.Application.UseCases.CampaignUseCases;
using BookRight.Application.UseCases.ClinicUseCases;
using BookRight.Application.UseCases.PractitionerUseCases;
using BookRight.Application.UseCases.Services;
using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Application.UseCases.Services.PriceCalculator;
using BookRight.Facade.Commands.Booking;
using BookRight.Facade.Commands.Campaign;
using BookRight.Facade.Commands.Clinic;
using BookRight.Facade.Commands.CustomerCommands;
using BookRight.Facade.Commands.Practitioner;
using Microsoft.Extensions.DependencyInjection;

namespace BookRight.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // -----
            // Customer
            // -----
            services.AddScoped<ICreateCustomer, CreateCustomerHandler>();
            services.AddScoped<IUpdateCustomer, UpdateCustomerHandler>();
            services.AddScoped<IDeleteCustomer, DeleteCustomerHandler>();

            // -----
            // Practitioner
            // -----
            services.AddScoped<ICreatePractitioner, CreatePractitionerHandler>();
            services.AddScoped<IUpdatePractitioner, UpdatePractitionerHandler>();
            services.AddScoped<IDeletePractitioner, DeletePractitionerHandler>();
            services.AddScoped<IAssignPractitionerToClinic, AssignPractitionerToClinicHandler>();

            // -----
            // Clinic
            // -----
            services.AddScoped<ICreateClinic, CreateClinicHandler>();
            services.AddScoped<IUpdateClinic, UpdateClinicHandler>();
            services.AddScoped<IDeleteClinic, DeleteClinicHandler>();

            // -----
            // Campaign
            // -----
            services.AddScoped<ICreateCampaign, CreateCampaignHandler>();
            services.AddScoped<IUpdateCampaign, UpdateCampaignHandler>();
            services.AddScoped<IDeleteCampaign, DeleteCampaignHandler>();

            // -----
            // Booking
            // -----
            services.AddScoped<ICreateBookingUseCase, CreateBookingCommandHandler>();
            services.AddScoped<ICancelBookingFacade, CancelBookingCommandHandler>();
            services.AddScoped<ICompleteBookingUseCase, CompleteBookingCommandHandler>();



            // -----
            // Services
            // -----
            services.AddScoped<IBookingConflictChecker, BookingConflictChecker>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IPriceCalculator, PriceCalculator>();

            return services;
        }
    }
}