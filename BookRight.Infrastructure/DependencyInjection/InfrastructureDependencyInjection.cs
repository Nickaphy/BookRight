using BookRight.Application.Repositories;
using BookRight.Domain.Common;
using BookRight.Facade.Querries.PractitionerQuerries;
using BookRight.Facade.Querries.TreatmentTypeQuerries;
using BookRight.Infrastructure.Persistece.Repository;
using BookRight.Infrastructure.Persistence;
using BookRight.Infrastructure.Persistence.QuerryHandlers;
using BookRight.Infrastructure.Persistence.Repositories;
using BookRight.Facade.Querries.CustomerQuerries;
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
            services.AddDbContextFactory<AppDbContext>(lifetime: ServiceLifetime.Scoped);

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

            // ----
            //Querries
            // ----
            services.AddScoped<ICustomerQuerries, CustomerQuerries>();
            services.AddScoped<ITreatmentTypeQuerry, TreatmentTypeImpl>();
            services.AddScoped<IPractitionerQuerries, PractitionerImpl>();
            return services;
        }
    }
}