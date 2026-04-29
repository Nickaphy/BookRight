// EF Core DbContext
// Represents the database session
// Contains DbSet properties later
// Uses Domain entities to create the database schema with Code First
// Belongs in Infrastructure because Domain must not know EF Core



// DbContext represents the database session.
//
// Responsibilities:
// - Expose DbSet<T> for each aggregate/entity
// - Configure mappings using Fluent API
// - Act as bridge between Domain and database
//
// IMPORTANT:
// Domain does NOT know this class.
// This class depends on Domain, not the other way around.
//
// Uses Code First approach:
// - Domain entities define the model
// - EF Core generates the database schema


/*
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BookLeft.Infrastructure.Persistence;

public class BookLeftDbContext : DbContext
{
    // Constructor (kommer senere)
    public BookLeftDbContext(DbContextOptions<BookLeftDbContext> options)
        : base(options)
    {
    }

    // 👇 This is the most important comment:

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // OnModelCreating will configure relationships and constraints
        // using Fluent API.
        //
        // Examples:
        // - One-to-many relationships
        // - Value objects (owned types)
        // - Indexes for performance
        // - Constraints (unique, required)
        //
        // Keeps database-specific logic OUT of Domain

        base.OnModelCreating(modelBuilder);
    }
}

This could contain coding like this:

modelBuilder.Entity<Booking>()
    .HasOne(b => b.Clinic)
    .WithMany()
    .HasForeignKey("ClinicId");

- We are using OnModelCreating when we don't think that EF-Core can guess what we are looking for, 
OR.. if we just want to be explicit... And we want to be explicit, so we will set up
the data-base, this way.


*/