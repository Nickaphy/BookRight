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

    }
    public DbSet<PractitionerClinicDay> PractitionerClinicDays { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<TreatmentType> Treatments { get; set; }
    public DbSet<Practitioner> Practitioners { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=BookRight;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.ApplyConfiguration(new ClinicConfiguration());
        modelBuilder.ApplyConfiguration(new TreatmentConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());*/

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Booking>(booking =>
        {
            booking.OwnsOne(b => b.BasePrice, money =>
            {
                money.Property(m => m.Amount)
                     .HasColumnName("BasePrice")
                     .HasColumnType("decimal(18,2)");
            });

            booking.OwnsOne(b => b.FinalPrice, money =>
            {
                money.Property(m => m.Amount)
                     .HasColumnName("FinalPrice")
                     .HasColumnType("decimal(18,2)");
            });

            booking.OwnsOne(b => b.TimeRange, tr =>
            {
                tr.Property(t => t.Start).HasColumnName("StartTime");
                tr.Property(t => t.End).HasColumnName("EndTime");
            });
        });

            modelBuilder.Entity<TreatmentType>()
            .OwnsOne(t => t.BasePrice, money =>
            {
                money.Property(m => m.Amount)
            .HasColumnName("BasePrice")
            .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Clinic>()
            .OwnsMany(c => c.OpeningHours, oh =>
            {
                oh.WithOwner().HasForeignKey("ClinicId");
                oh.Property(o => o.WeekDay);
                oh.Property(o => o.OpeningTime);
                oh.Property(o => o.ClosingTime);
                oh.HasKey("ClinicId", "WeekDay");
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
            var events = aggregate.DomainEvents.ToList(); // 1. kopiér
            aggregate.ClearDomainEvents();                // 2. ryd FØR dispatch

            foreach (var domainEvent in events)           // 3. dispatch fra kopien
            {
                await _domainEventDispatcher.Dispatch(domainEvent, cancellationToken);
            }
        }

        return result;
    }
}