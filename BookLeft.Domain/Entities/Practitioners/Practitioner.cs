// Represents a practitioner
using BookRight.Domain.Common;
using BookRight.Domain.Exceptions;
using System.Text.RegularExpressions;

//Lucas - ville pushe op.. han ville se hvordan treatmenttype kunne få en relation til practitioner,
//og hvordan practitioner kunne have en liste af treatmenttypes som de var autoriseret til at udføre.
namespace BookRight.Domain.Entities.Practitioners;

public class Practitioner : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public string AuthorizationCode { get; private set; } = null!;
    public AuthorizationType AuthorizationType { get; private set; }
    
    private List<PractitionerClinicDay> _clinicDays = new();   //starter som en tom liste, disse to gør at aggregatet Clinic days er skrive beskyttet gennem practitioner ONLY.
    public IReadOnlyList<PractitionerClinicDay> ClinicDays => _clinicDays.AsReadOnly();

    //public List<TreatmentType> TreatmentTypes { get; private set; } = new(); 
    //Lucas - ville gerne have en liste af treatmenttypes som practitioner var autoriseret til at udføre,
    //men vi bryder DDD reglerne ved at have en direkte reference til treatmenttype i practitioner, da det skaber en stærk kobling mellem de to aggregates.
    //I stedet kunne vi have en liste af treatmenttype ids, som practitioner er autoriseret til at udføre, og så lave en metode i booking aggregate root der tjekker om practitioner har den nødvendige autorisation for den valgte treatmenttype.


    //public List<Guid> TreatmentTypeIds { get; private set; } = new();
    //fordi practitioner samles med treatmenttype i Authorization, kan man gøre det sådan i booking aggregatet..
    //practitioner.HasAuthorizationForTreatment(treatmentType.NeedsAuthorisation);

    //public Guid ClinicId { get; private set; } //Lucas ændret, fordi denne gør at man har en direkte reference til clinic i practitioner,
    //                                              hvilket skaber en stærk kobling mellem de to aggregates.
    //                                              I stedet har vi en liste af PractitionerClinicDay,
    //                                              som indeholder både clinicId og date, og så kan vi lave metoder i
    //                                              practitioner til at tjekke om practitioner er tilknyttet en clinic på en given dato.

    private Practitioner() { }

    public Practitioner(string name, 
                        string email, 
                        string phoneNumber, 
                        string authorizationCode, 
                        AuthorizationType authorizationType 
                        //Guid clinicId //Lucas ændret, se properties.
                        )
    {
        Name = name;
        Email = email.Trim();
        PhoneNumber = phoneNumber;
        AuthorizationCode = authorizationCode;
        AuthorizationType = authorizationType;
        //ClinicId = clinicId; //Lucas ændret
        Validate();
      
}  
    public static Practitioner Create(string name, 
        string email, 
        string phoneNumber, 
        string authorizationCode, 
        AuthorizationType authorizationType)
    {
        var practitioner = new Practitioner(name, email, phoneNumber, authorizationCode, authorizationType);
        return practitioner;
    }
   
    public void Validate() //evt overføres til exception  klasse, med tekst mapper.
        {
        if (!Name.All(c => char.IsLetter(c) || c == ' '))
            throw new DomainException ("Name must only contain letters and spaces.");
        if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))                   //^ betyder start af string, at @ inde i [] betyder at man ikke må have @ i starten, og \s betyder ikke whitespace.  +$ betyder slut af streng.. så hvis man laver whitespace går den ikke.
            throw new DomainException ("Email is not valid.");
        if (!Regex.IsMatch(PhoneNumber, @"^\+?[\d]{8,15}$"))                        //^start af streng, \+ betyder at der må være en streng, \d tilladt digits (0-9) {8,15} der må være 8 - 15 tegn.
            throw new DomainException("Phone number is not valid.");
        if (string.IsNullOrWhiteSpace(AuthorizationCode))
            throw new DomainException("Authorization code cannot be empty.");
    }


    //metode der viser om en practitioner er tilknyttet en klinik på en given dato,
    //og hvis ikke tilknytter den practitioner til klinikken på den dato.
    //Hvis practitioner allerede er tilknyttet en klinik på den dato, kaster den en exception.

    public void AssignToClinic(Guid clinicId, DateTime date)
    {
        if (date.Date < DateTime.Today)
            throw new DomainException("Cannot assign a practitioner to a clinic on a past date.");

        if (clinicId == Guid.Empty)
            throw new DomainException("ClinicId cannot be empty.");
        bool alreadyAssigned = _clinicDays.Any(cd => cd.Date == date.Date && cd.ClinicId != clinicId);

        if (alreadyAssigned) 
                throw new DomainException("Something went wrong, either the practitioner is already assigned to a different clinic " +
                                            "on this date or the clinicId is invalid.");

        _clinicDays.Add(new PractitionerClinicDay(Id, clinicId, date));
    }

    public void RemoveFromClinic(Guid clinicId, DateTime date)
    {
        if (date.Date < DateTime.Today)
            throw new DomainException("Cannot remove a practitioner from a clinic on a past date.");

        var clinicDay = _clinicDays.FirstOrDefault(cd => cd.ClinicId == clinicId && cd.Date == date.Date);
        if (clinicDay != null)
        {
            _clinicDays.Remove(clinicDay);
        }
        else
        {
            throw new DomainException("Practitioner is not assigned to this clinic on the specified date.");
        }
    }

    public void HasAuthorizationForTreatment(AuthorizationType authorizationtype)
    {

       if (AuthorizationType != authorizationtype)
            throw new DomainException($"Practitioner does not have the required authorization type: {authorizationtype}.");
        

    }

    public void PractitionerIsBookedOnDate(DateTime date)
    {
        bool isBooked = _clinicDays.Any(cd => cd.Date == date.Date);
        if (isBooked)
            throw new DomainException("Practitioner is already booked on this date.");
    }
    public void UpdateName(string name)
    {
        Name = name;
        Validate();
    }

    public void UpdateEmail(string email)
    {
        Email = email.Trim();
        Validate();
    }

    public void UpdatePhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
        Validate();
    }

    public void UpdateAuthorization(string authorizationCode, AuthorizationType authorizationType)
    {
        AuthorizationCode = authorizationCode;
        AuthorizationType = authorizationType;
        Validate();
    }





}
// Rules:
// - Has authorization type
// - Can only perform allowed treatments
