// Registers Infrastructure services in dependency injection
// Example: DbContext and repository implementations
// Called from UI startup later



// This class will register Infrastructure dependencies.
// It connects Application abstractions to Infrastructure implementations.
//
// Example later:
// IBookingRepository -> BookingRepository
// ICustomerRepository -> CustomerRepository
// BookRightDbContext -> SQL Server database
//
// This keeps Application independent from EF Core and database details.
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookRight.Application.Repositories;
using BookRight.Infrastructure.Persistence.Repositories;

namespace BookRight.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext with SQL Server provider
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register repository implementations
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}
