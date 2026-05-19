using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Intrinsics.X86;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookRight.Facade.Dtos.BookingCommand
{
    // Data Transfer Object
    // Used to move data between Facade and UI
    // UI should not work directly with Domain entities
    // DTOs keep the UI separated from internal business rules

    /*Blazor UI
    → calls Facade interfaces

    Facade
    → hides Application complexity

    Application
    → executes commands and queries

    Domain
    → protects business rules*/

    // Erik´s work

    

    

    public sealed record CreateBookingRequest(
        Guid CustomerId,
        Guid PractitionerId,
        Guid ClinicId,
        Guid TreatmentTypeId,
        DateTime StartTime
        
    );

    //CreateBookingRequest
    //= data from UI to Facade.

    //CreateBookingCommand
    //= data from Facade to Application.
}
