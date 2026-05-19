using BookRight.Domain.Common;
using System.Net.Http.Headers;
using BookRight.Domain.Exceptions;

namespace BookRight.Domain.Entities.Customers;

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
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
        LoyaltyLevel = loyaltyLevel;
        DateOfBirth = dateOfBirth;
        Note = note;
        Street = street;
        City = city;
        Zipcode = zipcode;
        Validate();
    }

    public static Customer Create(string name,
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
        var customer = new Customer(name, phoneNumber, email, loyaltyLevel, dateOfBirth, note, street, city, zipcode);
        return customer;
    }

    public void Validate()
    {
        //Not empty
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException("Name cannot be empty.");

        //Not empty
        if (string.IsNullOrWhiteSpace(Email))
            throw new DomainException("Email cannot be empty.");

        //Not empty
        if (string.IsNullOrWhiteSpace(PhoneNumber))
            throw new DomainException("Phone number cannot be empty.");

        //Only digits
        if (!PhoneNumber.All(char.IsDigit))
            throw new DomainException("Phone number can only contain digits.");

        //Must be between 8 and 11 digits
        if (PhoneNumber.Length < 8 || PhoneNumber.Length > 11)
            throw new DomainException("Phone number must be between 8 and 11 digits.");
        if (DateOfBirth > DateTime.Now)
            throw new DomainException("DateOfBirth cannot be in the future.");

        if (Zipcode.Length != 4)
            throw new DomainException("Zipcode must be 4 digits.");

        if (!Zipcode.All(char.IsDigit))
            throw new DomainException("Zipcode must contain only digits.");

        if (string.IsNullOrWhiteSpace(City))
            throw new DomainException("City cannot be empty.");

        if (string.IsNullOrWhiteSpace(Street))
            throw new DomainException("Street cannot be empty.");
    }

    public void UpdateLoyaltyLevel(LoyaltyLevel newLevel)
    {
        LoyaltyLevel = newLevel;
        Validate();
    }
    public void UpdateNote(string? newNote)
    {
        Note = newNote;
        Validate();
    }

    public void UpdatePhoneNumber(string newPhoneNumber)
    {
        PhoneNumber = newPhoneNumber;
        Validate();
    }

    public void UpdateEmail(string newEmail)
    {
        Email = newEmail;
        Validate();
    }

    public void UpdateStreet(string newStreet)
    {
        Street = newStreet;
        Validate();
    }

    public void UpdateCity(string newCity)
    {
        City = newCity;
        Validate();
    }

    public void UpdateZipcode(string newZipcode)
    {
        Zipcode = newZipcode;
        Validate();
    }

    public void UpdateName(string newName)
    {
        Name = newName;
        Validate();
    }

    public void updateDateOfBirth(DateTime newDateOfBirth)
    {
        DateOfBirth = newDateOfBirth;
        Validate();
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
