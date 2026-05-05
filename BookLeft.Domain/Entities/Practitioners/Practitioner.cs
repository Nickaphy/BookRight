// Represents a practitioner
using BookRight.Domain.Common;
using BookRight.Domain.Exceptions;
using BookRight.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
//Lucas - ville pushe op.. han ville se hvordan treatmenttype kunne få en relation til practitioner,
//og hvordan practitioner kunne have en liste af treatmenttypes som de var autoriseret til at udføre.

public class Practitioner : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public string AuthorizationCode { get; private set; } = null!;
    public AuthorizationType AuthorizationType { get; private set; }
    public IReadOnlyList<PractitionerClinicDay> ClinicDays => _clinicDays.AsReadOnly();
    private List<PractitionerClinicDay> _clinicDays = new();   //disse to gør at aggregatet Clinic days er skrive beskyttet gennem practitioner ONLY.
    public List<TreatmentTypes> TreatmentTypes { get; private set; } = new();
    public Guid ClinicId { get; private set; }

    private Practitioner() { }

    public Practitioner(string name, 
                        string email, 
                        string phoneNumber, 
                        string authorizationCode, 
                        AuthorizationType authorizationType, 
                        Guid clinicId)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        AuthorizationCode = authorizationCode;
        AuthorizationType = authorizationType;
        ClinicId = clinicId;

    }
    //metode der viser om en practitioner er tilknyttet en klinik på en given dato,
    //og hvis ikke tilknytter den practitioner til klinikken på den dato.
    //Hvis practitioner allerede er tilknyttet en klinik på den dato, kaster den en exception.
    public void AssignToClinic(Guid clinicId, DateTime date)
    {
        bool alreadyAssigned = _clinicDays.Any(cd => cd.Date == date);
        if (alreadyAssigned) 
                throw new DomainException("Practitioner is already assigned to a clinic on this date.");

        _clinicDays.Add(new PractitionerClinicDay(Id, clinicId, date));
    }

    public void HasAuthorizationForTreatment(AuthorizationType authorizationtype, TreatmentTypes treatmentType)
    {

       if (AuthorizationType != authorizationtype)
            throw new DomainException($"Practitioner does not have the required authorization type: {authorizationtype}.");
        if (!TreatmentTypes.Contains(treatmentType))
            throw new DomainException($"Practitioner is not authorized to perform treatment type: {treatmentType}.");

    }

    public void PractitionerIsBookedOnDate(DateTime date)
    {
        bool isBooked = _clinicDays.Any(cd => cd.Date == date);
        if (isBooked)
            throw new DomainException("Practitioner is already booked on this date.");
    }



}
// Rules:
// - Has authorization type
// - Can only perform allowed treatments