// Unit tests for Domain logic
// Tests pure business rules without database
// No EF Core, no repositories
using BookRight.Domain.Exceptions;


//ARRANGE
//Helper method to create a practitioner with specific clinic assignments

public class PractitionerTests
{
    private static Practitioner CreatePractitioner(
        AuthorizationType authType = AuthorizationType.Physiotherapist)
    {
        return new Practitioner(
            name: "Test Practitioner",
            email: "HansAndersen@gmail.com",
            phoneNumber: "12345678",
            authorizationCode: "AUTH123",
            authorizationType: authType,
            clinicId: Guid.NewGuid());

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

            // Act
            var practitioner = new Practitioner(
                name,
                email,
                phoneNumber,
                authorization,
                authType,
                clinicId);

            // Assert
            Assert.Equal(name, practitioner.Name);
            Assert.Equal(email, practitioner.Email);
            Assert.Equal(phoneNumber, practitioner.PhoneNumber);
            Assert.Equal(authorization, practitioner.AuthorizationCode);
            Assert.Equal(authType, practitioner.AuthorizationType);
            Assert.Equal(clinicId, practitioner.ClinicId);

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
        public void Sets_ClinicId_Correctly()
        {
            // Arrange
            var clinicId = Guid.NewGuid();

            // Act
            var practitioner = new Practitioner(
                "Jane Doe", "jane@clinic.dk", "12345678",
                "AUTH-001", AuthorizationType.Physiotherapist, clinicId);

            // Assert
            Assert.Equal(clinicId, practitioner.ClinicId);
        }

        [Fact]
        public void Starts_With_No_ClinicDays()
        {
            // Arrange & Act
            var practitioner = CreatePractitioner();

            // Assert
            Assert.Empty(practitioner.ClinicDays);
        }


    }


    public class AssignToClinic
    {
        [Fact]
        public void Assigns_Practitioner_To_Clinic_On_New_Date()
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var clinicId = Guid.NewGuid();
            var date = new DateTime(2025, 6, 10);

            // Act
            practitioner.AssignToClinic(clinicId, date);

            // Assert
            Assert.Equal(1, practitioner.ClinicDays.Count);    //equals 1 clinic day added
            Assert.Equal(clinicId, practitioner.ClinicDays[0].ClinicId);  
            Assert.Equal(date.Date, practitioner.ClinicDays[0].Date);
        }

        [Fact]
        public void Throws_DomainException_When_Already_Assigned_On_Same_Date()
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
        public void Throws_DomainException_Message_Mentions_Date_Conflict()
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var date = new DateTime(2025, 6, 10);
            practitioner.AssignToClinic(Guid.NewGuid(), date);

            // Act
            var ex = Record.Exception(() => practitioner.AssignToClinic(Guid.NewGuid(), date));

            // Assert
            Assert.IsType<DomainException>(ex);
            Assert.Contains("already assigned", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Allows_Assignment_To_Different_Date()
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
        public void Same_Date_Different_Time_Is_Considered_Duplicate()
        {
            // Arrange
            var practitioner = CreatePractitioner();
            var morning = new DateTime(2025, 6, 10, 8, 0, 0);
            var afternoon = new DateTime(2025, 6, 10, 15, 0, 0);
            practitioner.AssignToClinic(Guid.NewGuid(), morning);

            // Act
            var act = () => practitioner.AssignToClinic(Guid.NewGuid(), afternoon);

            // Assert — time difference does not bypass the date-only check
            Assert.Throws<DomainException>(act);
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

       
    }
}


//ACT 


//ASSERT



// Focus:
// - booking overlap rules
// - time validation
// - clinic capacity rules
// - practitioner assignment rules
// - discount rules