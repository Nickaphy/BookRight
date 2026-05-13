using BookRight.Domain.Common;

namespace BookRight.Domain.Entities.Clinics;

public class Clinic : AggregateRoot
{
    public string Name { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Zipcode { get; private set; }
    public int AmountTreatmentRooms { get; private set; }

    public List<ClinicOpeningHour> OpeningHours { get; private set; }

    public int MaxSimultaneousBookings => AmountTreatmentRooms;

    private Clinic() { }

    public Clinic(
        string name,
        int amountTreatmentRooms,
        string street,
        string city,
        string zipcode
        )
    {
        if (amountTreatmentRooms <= 0)
            throw new ArgumentException("A clinic must have at least one treatment room.");
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Clinic must have a name.");
        
        if (zipcode.Length != 4 )
            throw new ArgumentException("Zipcode must be 4 digits.", nameof(zipcode));
        
        if (zipcode.All(!char.IsDigit))
            throw new ArgumentException("Zipcode must not contain only digits.", nameof(zipcode));
            
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty.", nameof(city));

        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty.", nameof(street));

        Name = name;
        AmountTreatmentRooms = amountTreatmentRooms;
        OpeningHours = new List<ClinicOpeningHour>();
        Street = street;
        City = city;
        Zipcode = zipcode;
    }
}
