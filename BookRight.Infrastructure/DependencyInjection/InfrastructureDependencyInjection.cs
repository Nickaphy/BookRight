using BookRight.Application.Repositories;
using BookRight.Domain.Common;
using BookRight.Facade.Querries.BookingQuerries;
using BookRight.Facade.Querries.ClinicQuerries;
using BookRight.Facade.Querries.CustomerQuerries;
using BookRight.Facade.Querries.PractitionerQuerries;
using BookRight.Facade.Querries.TreatmentTypeQuerries;
using BookRight.Infrastructure.Persistece.Repository;
using BookRight.Infrastructure.Persistence;
using BookRight.Infrastructure.Persistence.QuerryHandlers;
using BookRight.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BookRight.Application.QuerryHandlers.Pricing;

namespace BookRight.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            // ─── Database ────────────────────────────────────────────────────────
            services.AddDbContextFactory<AppDbContext>(
                options =>
                {
                    options.UseSqlServer(
                        @"Server=(localdb)\MSSQLLocalDB;Database=BookRight;Trusted_Connection=True;TrustServerCertificate=True",
                        sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure();
                        });
                },
                ServiceLifetime.Scoped);

            // Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPractitionerRepository, PractitionerRepository>();
            services.AddScoped<IClinicRepository, ClinicRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<ITreatmentTypeRepository, TreatmentTypeRepository>();

            // Queries
            services.AddScoped<ICustomerQuerries, CustomerQuerries>();
            services.AddScoped<ITreatmentTypeQuerry, TreatmentTypeImpl>();
            services.AddScoped<IPractitionerQuerries, PractitionerImpl>();
            services.AddScoped<IClinicQuerries, ClinicImpl>();
            services.AddScoped<IBookingQueries, BookingQueries>();
            services.AddScoped<IClinicReportQueries, ClinicReportImpl>();
            return services;
        }
    }
}