using BookRight.Domain.Common;
using BookRight.Domain.Enums;
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
    public void Update(string name, string phoneNumber, string email,
                   string? street, string city, string zipcode,
                   DateTime dateOfBirth)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
        Street = street;
        City = city;
        Zipcode = zipcode;
        DateOfBirth = dateOfBirth;
        Validate();
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

    public static Customer Create(
    string name,
    string phoneNumber,
    string email,
    LoyaltyLevel loyaltyLevel,
    DateTime dateOfBirth,
    string? note,
    string? street,
    string city,
    string zipcode)
    {
        return new Customer(name, 
                            phoneNumber, 
                            email, 
                            loyaltyLevel,
                            dateOfBirth, 
                            note, 
                            street, 
                            city, 
                            zipcode);
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
