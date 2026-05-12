//11.05.2026
//Lasse
//Denne record repræsenterer en kommando for at annullere en booking.
//Den indeholder kun en egenskab, BookingId, som er nødvendig for at identificere den booking, der skal annulleres.
//Denne record bruges som input til CancelBookingUseCase, som håndterer logikken for at annullere bookingen.

using BookRight.Domain.Common;
using System.Runtime.CompilerServices;

public record CancelBookingCommand
{
    public Guid BookingId { get; }

    public CancelBookingCommand(Guid bookingId)
    {
        BookingId = bookingId;
    }

}