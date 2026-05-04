// Represents a customer
namespace Bookright.Domain.Customers;

public class Customer : Entity
{
    public string Name { get; private set; } = null!;
    public string ContactInfo { get; private set; } = null!;
    public string LoyaltyLevel { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? Address { get; private set; }
    public string? Note { get; private set;  } = null!;


    private Customer() { }

        public Customer(Guid id, string name, string contactInfo, string loyaltyLevel, DateTime dateOfBirth, string? address = null, string? note = null)
        {   
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(contactInfo))
                throw new ArgumentException("ContactInfo cannot be empty.", nameof(contactInfo));

            if (string.IsNullOrWhiteSpace(loyaltyLevel))
                throw new ArgumentException("LoyaltyLevel cannot be empty.", nameof(loyaltyLevel));

            if (dateOfBirth > DateTime.Now)
                throw new ArgumentException("DateOfBirth cannot be in the future.", nameof(dateOfBirth));

            if (address != null) 
            {
                if (string.IsNullOrWhiteSpace(address))
                    throw new ArgumentException("Address cannot be empty if provided.", nameof(address));
            }

            if (note != null) 
            {
                if (string.IsNullOrWhiteSpace(note))
                    throw new ArgumentException("Note cannot be empty if provided.", nameof(note));
            }

            Id = Guid.NewGuid();
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