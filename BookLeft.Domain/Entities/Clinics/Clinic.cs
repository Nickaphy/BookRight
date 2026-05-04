namespace BookRight.Domain.Entities.Clinics;

public class Clinic
{
    public Guid ClinicId { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public int AmountTreatmentRooms { get; private set; }

    public List<ClinicOpeningHour> OpeningHours { get; private set; }

    public int MaxSimultaneousBookings => AmountTreatmentRooms;

    private Clinic() { }

    public Clinic(
        string name,
        string address,
        int amountTreatmentRooms)
    {
        if (amountTreatmentRooms <= 0)
            throw new ArgumentException("A clinic must have at least one treatment room.");
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Clinic must have a name.");

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Clinic must have an address.");

        ClinicId = Guid.NewGuid();
        Name = name;
        Address = address;
        AmountTreatmentRooms = amountTreatmentRooms;
        OpeningHours = new List<ClinicOpeningHour>();
    }
}