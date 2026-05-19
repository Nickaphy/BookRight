using BookRight.Domain.Common;
using BookRight.Domain.Exceptions;
using BookRight.Domain.ValueObjects;

namespace BookRight.Domain.Entities.Clinics;

public class Clinic : AggregateRoot
{
    public string Name { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Zipcode { get; private set; }
    public int AmountTreatmentRooms { get; private set; }

    private List<ClinicOpeningHour> _openingHours = new();                      //skrivebeskyttet liste over åbningstider, da det ikke skal være muligt at tilføje eller fjerne åbningstider direkte
    public IReadOnlyList<ClinicOpeningHour> OpeningHours => _openingHours.AsReadOnly(); 

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

        Name = name;
        AmountTreatmentRooms = amountTreatmentRooms;
        Street = street;
        City = city;
        Zipcode = zipcode;
        _openingHours = new List<ClinicOpeningHour>();
        Validate();
    }
    public static Clinic Create(
        string name,
        int amountTreatmentRooms,
        
        string street,
        string city,
        string zipcode,
        ClinicOpeningHour[] openingHours
    )
    {
        var clinic = new Clinic(name, amountTreatmentRooms, street, city, zipcode);
        clinic.ReplaceOpeningHours(openingHours.ToList()); //tilføjer de angivne åbningstider til klinikken, gennem ReplaceOpeningHours metoden som også validerer åbningstiderne.
        return clinic;
    }
    

    public void Validate()
    {
        if (AmountTreatmentRooms <= 0)
            throw new DomainException("A clinic must have at least one treatment room.");
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException("Clinic must have a name.");
        if (Zipcode.Length != 4)
            throw new DomainException("Zipcode must be 4 digits.");
        if (!Zipcode.All(char.IsDigit))
            throw new DomainException("Zipcode must only contain digits.");
        if (string.IsNullOrWhiteSpace(City))
            throw new DomainException("City cannot be empty.");
        if (string.IsNullOrWhiteSpace(Street))
            throw new DomainException("Street cannot be empty.");
    }
    public void ReplaceOpeningHours(List<ClinicOpeningHour> openingHours)
        {
            if (openingHours == null || !openingHours.Any())
                throw new DomainException("Opening hours cannot be null or empty.");
            if (openingHours.GroupBy(oh => oh.WeekDay).Any(g => g.Count() > 1))                 //sorts the opening hours by week day and checks if there are multiple opening hours for the same week day
                throw new DomainException("Duplicate opening hours for the same day of the week are not allowed.");
            _openingHours = openingHours;
        }



    public void UpdateClinic(
    string name,
    int amountTreatmentRooms,
    string street,
    string city,
    string zipcode,
    IEnumerable<ClinicOpeningHour> openingHours)
    {
        Name = name;
        AmountTreatmentRooms = amountTreatmentRooms;
        Street = street;
        City = city;
        Zipcode = zipcode;
        Validate();
        ReplaceOpeningHours(openingHours.ToList());
    }


    







    /* //NICE TO HAVE
    public void UpdateAmountTreatmentRooms(int amountTreatmentRooms)
    {
        if (amountTreatmentRooms <= 0)
            throw new DomainException("A clinic must have at least one treatment room.");
        AmountTreatmentRooms = amountTreatmentRooms;
    }
    public void UpdateAddress(string street, string city, string zipcode)
    {
        Street = street;
        City = city;
        Zipcode = zipcode;
        Validate();
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Clinic must have a name.");
        Name = name;
    }

    

    public void AddOpeningHour(ClinicOpeningHour openingHour)
    {
        bool weekDayAlreadyExists = OpeningHours.Any(oh => oh.WeekDay == openingHour.WeekDay);

        if (weekDayAlreadyExists)
            throw new DomainException($"Opening hours for {openingHour.WeekDay} already exist.");

        OpeningHours.Add(openingHour);
    }

    public void RemoveOpeningHour(DayOfWeek dayOfWeek)
    {
        var openingHour = OpeningHours.FirstOrDefault(oh => oh.WeekDay == dayOfWeek);
        if (openingHour == null)
            throw new DomainException($"No opening hours found for {dayOfWeek}.");
        OpeningHours.Remove(openingHour);
    }
    */

}
