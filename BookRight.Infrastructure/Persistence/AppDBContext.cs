using BookRight.Domain.Common;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Campaigns;
using BookRight.Domain.Entities.Clinics;
using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Domain.Entities.Treatments;
using Microsoft.EntityFrameworkCore;

namespace BookRight.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher domainEventDispatcher)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
        if (Database.CanConnect())
            DataSeeder.Seed(this);
    }

    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<TreatmentType> Treatments { get; set; }

    public DbSet<Practitioner> Practitioners { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=BookRightTest;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.ApplyConfiguration(new ClinicConfiguration());
        modelBuilder.ApplyConfiguration(new TreatmentConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());*/

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Booking>()
            .OwnsOne(b => b.FinalPrice, money =>
            {
                money.Property(m => m.Amount).HasColumnName("FinalPrice");
            });

        modelBuilder.Entity<Booking>()
            .OwnsOne(b => b.BasePrice, money =>
            {
                money.Property(m => m.Amount).HasColumnName("BasePrice");
            });
        modelBuilder.Entity<Booking>()
            .OwnsOne(b => b.TimeRange, tr =>
            {
                tr.Property(t => t.Start).HasColumnName("StartTime");
                tr.Property(t => t.End).HasColumnName("EndTime");
            });
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // 1. Hent events FØR vi gemmer
        var aggregates = ChangeTracker.Entries<AggregateRoot>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity)
            .ToList();

        // 2. Gem data
        var result = await base.SaveChangesAsync(cancellationToken);

        // 3. Dispatch events EFTER data er gemt
        foreach (var aggregate in aggregates)
        {
            foreach (var domainEvent in aggregate.DomainEvents)
            {
                await _domainEventDispatcher.Dispatch(domainEvent, cancellationToken);
            }

            aggregate.ClearDomainEvents();
        }

        return result;
    }
} 
