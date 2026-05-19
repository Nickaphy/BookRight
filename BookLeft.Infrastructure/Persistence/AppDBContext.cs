
using Bookright.Domain.Entities.Customers;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Clinics;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Domain.Entities.Treatments;
using Microsoft.EntityFrameworkCore;

namespace BookRight.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<TreatmentType> Treatments { get; set; }

    public DbSet<Practitioner> Practitioners { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.ApplyConfiguration(new ClinicConfiguration());
        modelBuilder.ApplyConfiguration(new TreatmentConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());*/

        base.OnModelCreating(modelBuilder);
    }
} 