using BookRight.Application.Repositories;
using BookRight.Domain.Common;
using BookRight.Infrastructure.Persistece.Repository;
using BookRight.Infrastructure.Persistence;
using BookRight.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookRight.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            // -----
            // Database
            // -----
            services.AddDbContext<AppDbContext>();
            
            // -----
            // Repositories
            // -----
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPractitionerRepository, PractitionerRepository>();
            services.AddScoped<IClinicRepository, ClinicRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<ITreatmentTypeRepository, TreatmentTypeRepository>();
            return services;
        }
    }
}