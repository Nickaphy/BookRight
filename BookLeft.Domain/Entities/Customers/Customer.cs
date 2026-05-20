using BookRight.Domain.Common;
using BookRight.Domain.Exceptions;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace BookRight.Domain.Entities.Customers;

public class Customer : AggregateRoot
{
    public string Name { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public LoyaltyLevel LoyaltyLevel { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? Note { get; private set; }
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

        if (zipcode.Length != 4)
            throw new ArgumentException("Zipcode must be 4 digits.", nameof(zipcode));

        if (!zipcode.All(char.IsDigit))
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
    public void Validate()
    {
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
            throw new DomainException("Zipcode must not contain only digits.");

        if (string.IsNullOrWhiteSpace(City))
            throw new DomainException("City cannot be empty.");

        if (string.IsNullOrWhiteSpace(Street))
            throw new DomainException("Street cannot be empty.");
    }
    public void UpdateCustomerName(string name)
    {
        Name = name;
        Validate();
    }
    public void UpdateCustomerEmail(string email)
    {
        Email = email;
        Validate();
    }
    public void UpdateCustomerPhonenumber(string phonenumber)
    {
        PhoneNumber = phonenumber;
        Validate();
    }
    public void UpdateCustomerAdress(string city, string street, string zipcode)
    {
        City = city;
        Street = street;
        Zipcode = zipcode;
    }

    public void UpdateLoyaltyLevel(decimal totalSpentLastYear)
    {
        LoyaltyLevel = totalSpentLastYear switch
        {
            >= 10000 => LoyaltyLevel.Gold,
            >= 5000 => LoyaltyLevel.Silver,
            >= 1000 => LoyaltyLevel.Bronze,
            _ => LoyaltyLevel.None
        };
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
