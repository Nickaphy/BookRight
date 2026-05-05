// Links Practitioner to Clinic per day
using BookRight.Domain.Common;

public class PractitionerClinicDay : Entity
{
    public Guid PractitionerId { get; init; }
    public Guid ClinicId { get; init; }
    public DateTime Date { get; init; }

    private PractitionerClinicDay() { }

    public PractitionerClinicDay(Guid practitionerId, Guid clinicId, DateTime date)
    {
        PractitionerId = practitionerId;
        ClinicId = clinicId;
        Date = date.Date; // Ensure only the date part is stored
    }
}
// CRITICAL RULE:
// A practitioner can only be assigned to ONE clinic per day