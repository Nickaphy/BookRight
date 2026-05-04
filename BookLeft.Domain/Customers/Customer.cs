// Represents a customer
namespace Bookright.Domain.Customers;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string ContactInfo { get; private set; } = null!;
    public string LoyaltyLevel { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? Address { get; private set; }
    public string? Note { get; private set;  } = null!;


    private Customer() { }

        public Customer(Guid id, string name, string contactInfo, string loyaltyLevel, DateTime dateOfBirth, string? address = null, string? note = null)
        {   
            Id = id;
            Name = name;
            ContactInfo = contactInfo;
            LoyaltyLevel = loyaltyLevel;
            DateOfBirth = dateOfBirth;
            Address = address;
            Note = note;
    }
}

// Rules:
// - Has booking history
// - Used for loyalty calculation
// - May have preferred practitioner



// IMPORTANT:
// Loyalty calculation may be affected by race conditions
// when multiple bookings are created concurrently

// This will later require:
// - concurrency control
// - correct transaction handling
// - possibly optimistic concurrency with RowVersion