using BookRight.Domain.Common;

namespace Bookright.Domain.Customers;

public class Customer : AggregateRoot
{
    public string Name { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public LoyaltyLevel LoyaltyLevel { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? Note { get; private set;  }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Zipcode { get; private set; }


    private Customer() { }

        public Customer(string name, 
            string phoneNumber, 
            string email, 
            LoyaltyLevel loyaltyLevel, 
            DateTime dateOfBirth, 
            string? note,
            string? street,
            string city,
            string zipcode
            )
        {   
            //Not empty
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            //Not empty
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            
            //Not empty
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));
            
            //Only digits
            if (!phoneNumber.All(char.IsDigit))
                throw new ArgumentException("Phone number can only contain digits.", nameof(phoneNumber));

            //Must be between 8 and 11 digits
            if (phoneNumber.Length < 8 || phoneNumber.Length > 11)
                throw new ArgumentException("Phone number must be between 8 and 11 digits.", nameof(phoneNumber));

            if (dateOfBirth > DateTime.Now) 
                throw new ArgumentException("DateOfBirth cannot be in the future.", nameof(dateOfBirth));
            
            if (zipcode.Length != 4 )
                throw new ArgumentException("Zipcode must be 4 digits.", nameof(zipcode));
        
            if (zipcode.All(char.IsDigit))
                throw new ArgumentException("Zipcode must not contain only digits.", nameof(zipcode));
            
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty.", nameof(city));

            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be empty.", nameof(street));
                
            
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            LoyaltyLevel = loyaltyLevel;
            DateOfBirth = dateOfBirth;
            Note = note;
            Street = street;
            City = city;
            Zipcode = zipcode;
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