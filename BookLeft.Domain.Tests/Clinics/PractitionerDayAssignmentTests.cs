// Unit tests for Domain logic
// Tests pure business rules without database
// No EF Core, no repositories
using BookRight.Domain.Entities.Clinics;
using BookRight.Domain.Exceptions;


//ARRANGE
//Helper method to create a practitioner with specific clinic assignments

public class PractitionerTests
{
    private static Practitioner CreatePractitioner(
        AuthorizationType authType = AuthorizationType.Physiotherapist)
    {
        var practitioner = new Practitioner(
            name: "Test Practitioner",
            email: "HansAndersen@gmail.com",
            phoneNumber: "12345678",
            authorizationCode: "AUTH123",
            authorizationType: authType);

        

        return practitioner;

    }
    

    public class ConstructorTests
    {
        [Fact]
        public void Creates_Practitioner_With_Valid_Data()
        {
            // Arrange
            var name = "Test Practitioner";
            var email = "HansAndersen@gmail.com";
            var phoneNumber = "12345678";
            var authorization = "AUTH123";
            var authType = AuthorizationType.Physiotherapist;
            var clinicId = Guid.NewGuid();
            var date = new DateTime(2025, 6, 10);

            // Act
            var practitioner = new Practitioner(
                name,
                email,
                phoneNumber,
                authorization,
                authType);

            practitioner.AssignToClinic(clinicId, date);

            // Assert
            Assert.Equal(name, practitioner.Name);
            Assert.Equal(email, practitioner.Email);
            Assert.Equal(phoneNumber, practitioner.PhoneNumber);
            Assert.Equal(authorization, practitioner.AuthorizationCode);
            Assert.Equal(authType, practitioner.AuthorizationType);
            Assert.Equal(clinicId, practitioner.ClinicDays[0].ClinicId);
            Assert.Equal(date.Date, practitioner.ClinicDays[0].Date);
        }

        [Fact]
        public void Sets_AuthorizationType_Correctly()
        {
            // Arrange
            var authType = AuthorizationType.Masseur;
            // Act
            var practitioner = CreatePractitioner(authType);
            // Assert
            Assert.Equal(authType, practitioner.AuthorizationType);
        }

        [Fact]
        public void Throw_DomainException_Invalid_AuthorizationType()
        {
            // Arrange
            var practitioner = CreatePractitioner(AuthorizationType.Masseur);
            
            //Act & Assert
            Assert.Throws<DomainException>(() => practitioner.HasAuthorizationForTreatment(AuthorizationType.Physiotherapist));
        }

        [Fact]
        public void Sets_ClinicId_Correctly()
        {
            // Arrange
            var clinicId = Guid.NewGuid();
            var date = new DateTime(2025, 6, 10);
            var practitioner = new Practitioner(
                "Jane Doe", "jane@clinic.dk", "12345678",
                "AUTH-001", AuthorizationType.Physiotherapist);

            // Act
            practitioner.AssignToClinic(clinicId, date);

            // Assert
            Assert.Equal(clinicId, practitioner.ClinicDays[0].ClinicId);
            Assert.Equal(date.Date, practitioner.ClinicDays[0].Date);
        }

        [Fact]
        public void Throws_DomainException_When_ClinicId_Is_Empty()
        {
            // Arrange
            var clinicId = Guid.Empty;
            var date = new DateTime(2025, 6, 10);
            var practitioner = new Practitioner(
                "Jane Doe", "jane@clinic.dk", "12345678",
                "AUTH-001", AuthorizationType.Physiotherapist);

             
            // Act & Assert
            Assert.Throws<DomainException>(() => practitioner.AssignToClinic(clinicId, date)); //have to go through the method to trigger the validation.
        }
            

        [Fact]
        public void Starts_With_No_ClinicDays()
        {
            // Arrange 
            var practitioner = CreatePractitioner();

            // Act & Assert 
            Assert.Empty(practitioner.ClinicDays);
        }
    }


    public class AssignToClinic
    {
        [Fact]
        public void Assigns_Practitioner_To_Clinic_On_New_Date() //MINDER MEGET OM EN LÆNGERE OPPE
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var clinicId = Guid.NewGuid();
            var date = new DateTime(2025, 6, 10);

            // Act
            practitioner.AssignToClinic(clinicId, date);

            // Assert
            Assert.Single(practitioner.ClinicDays);    //equals 1 clinic day added
            Assert.Equal(clinicId, practitioner.ClinicDays[0].ClinicId);  //equals the clinicId we assigned, on the first clinic day in the list
            Assert.Equal(date.Date, practitioner.ClinicDays[0].Date);   //equals the date we assigned, on the first clinic day in the list
        }

        [Fact]
        public void Throws_DomainException_When_Already_Assigned_On_Same_Date_Different_Clinic()
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var clinicId = Guid.NewGuid();
            var date = new DateTime(2025, 6, 10);
            practitioner.AssignToClinic(clinicId, date);

            // Act
            var act = () => practitioner.AssignToClinic(Guid.NewGuid(), date);

            // Assert
            Assert.Throws<DomainException>(act);
        }
        [Fact]
        public void Same_Date_Different_Clinic_Is_Considered_Duplicate()
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var clinicId = Guid.NewGuid();
            practitioner.AssignToClinic(clinicId, new DateTime(2025, 6, 10, 8, 0, 0));

            // Act
            var act = () => practitioner.AssignToClinic(Guid.NewGuid(), new DateTime(2025, 6, 10, 15, 0, 0));

            // Assert 
            Assert.Throws<DomainException>(act);
        }
        [Fact]
        public void Stores_Date_Only_Part_Ignoring_Time()
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var dateWithTime = new DateTime(2025, 6, 10, 14, 30, 0); // 14:30

            // Act
            practitioner.AssignToClinic(Guid.NewGuid(), dateWithTime);

            // Assert — PractitionerClinicDay strips the time component
            Assert.Equal(dateWithTime.Date, practitioner.ClinicDays[0].Date);
        }

        [Fact]
        public void Allows_Same_Clinic_Assignment_To_Different_Date()
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var clinicId = Guid.NewGuid();
            var monday = new DateTime(2025, 6, 9);
            var tuesday = new DateTime(2025, 6, 10);
            practitioner.AssignToClinic(clinicId, monday);

            // Act
            practitioner.AssignToClinic(clinicId, tuesday);

            // Assert
            Assert.Equal(2, practitioner.ClinicDays.Count);
            Assert.Contains(practitioner.ClinicDays, cd => cd.ClinicId == clinicId);
        }

        [Fact]
        public void Can_Assign_To_Multiple_Different_Clinics_On_Different_Dates()
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var clinic1 = Guid.NewGuid();
            var clinic2 = Guid.NewGuid();

            // Act
            practitioner.AssignToClinic(clinic1, new DateTime(2025, 6, 9));
            practitioner.AssignToClinic(clinic2, new DateTime(2025, 6, 10));

            // Assert
            Assert.Equal(2, practitioner.ClinicDays.Count);
            Assert.Contains(practitioner.ClinicDays, cd => cd.ClinicId == clinic1);
            Assert.Contains(practitioner.ClinicDays, cd => cd.ClinicId == clinic2);
        }

        [Fact]
        public void Can_Reassign_After_Remove_On_Todays_Date()
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var clinicId = Guid.NewGuid();
            var date = DateTime.Today;
            practitioner.AssignToClinic(clinicId, date);
            practitioner.RemoveFromClinic(clinicId, date);

            // Act
            practitioner.AssignToClinic(clinicId, date);

            // Assert
            Assert.Single(practitioner.ClinicDays);
            Assert.Equal(clinicId, practitioner.ClinicDays[0].ClinicId);
        }
        [Fact]
        public void Throws_DomainException_When_Assigning_To_Past_Date()
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var clinicId = Guid.NewGuid();
            var yesterday = DateTime.Today.AddDays(-1);

            // Act & Assert
            Assert.Throws<DomainException>(() => practitioner.AssignToClinic(clinicId, yesterday));
        }
        [Fact]
        public void Throws_DomainException_When_Not_Assigned()
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var clinicId = Guid.NewGuid();
            var date = DateTime.Today;

            // Act & Assert
            Assert.Throws<DomainException>(() => practitioner.RemoveFromClinic(clinicId, date));
        }
    }
   
}
/*public class PractitionerClinicDayEncapsulationTests //denne test er for at se om PractitionerClinicDay er ordentligt sat op.
   {
       [Fact]
       public void PractitionerClinicDay_Cannot_Be_Created_Outside_Practitioner()
       {
           // Denne test kompilerer KUN hvis konstruktøren er public.
           // Hvis du ændrer konstruktøren til internal/private, fejler den at kompilere
           // — hvilket er præcis hvad du vil.

           // Arrange & Act
           var clinicDay = new PractitionerClinicDay(Guid.NewGuid(), Guid.NewGuid(), DateTime.Today);

           // Assert — vi burde aldrig nå hertil
           Assert.Fail("PractitionerClinicDay should not be directly instantiatable outside of Practitioner.");
       }
   }*/

//ACT 


//ASSERT



// Focus:
// - booking overlap rules
// - time validation
// - clinic capacity rules
// - practitioner assignment rules
// - discount rules