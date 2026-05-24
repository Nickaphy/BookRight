using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Clinics;
using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Domain.Entities.Treatments;
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Treatments.Any())
            {
                var treatments = new List<TreatmentType>
            {
                new TreatmentType("Fysioterapi 30 min.",                30, new Money(395), AuthorizationType.Physiotherapist, 1),
                new TreatmentType("Fysioterapi 45 min.",                45, new Money(589), AuthorizationType.Physiotherapist, 1),
                new TreatmentType("Fysioterapi 60 min.",                60, new Money(745), AuthorizationType.Physiotherapist, 1),
                new TreatmentType("Sportsmassage 30 min.",              30, new Money(350), AuthorizationType.Masseur,         1),
                new TreatmentType("Sportsmassage 60 min.",              60, new Money(699), AuthorizationType.Masseur,         1),
                new TreatmentType("Akupunktur 45 min.",                 45, new Money(550), AuthorizationType.Acupuncturist,   1),
                new TreatmentType("Kostvejledning førstegangskonsultation", 60, new Money(799), AuthorizationType.Nutritionist, 1),
                new TreatmentType("Kostvejledning opfølgning",          30, new Money(450), AuthorizationType.Nutritionist,    1),
                new TreatmentType("Holdtræning/genoptræning",           60, new Money(150), AuthorizationType.Physiotherapist, 6),
            };

                context.Treatments.AddRange(treatments);
                context.SaveChanges();
            }
            if (!context.Customers.Any())
            {
                var customers = new List<Customer>
            {
                Customer.Create("Anders Nielsen",    "12345678", "anders@mail.dk",    LoyaltyLevel.None, new DateTime(1985, 3, 15),  null, "Østergade 1",    "København", "1100"),
                Customer.Create("Sofie Madsen",      "23456789", "sofie@mail.dk",     LoyaltyLevel.None, new DateTime(1990, 6, 22),  null, "Vestergade 5",   "Aarhus",    "8000"),
                Customer.Create("Mikkel Jensen",     "34567890", "mikkel@mail.dk",    LoyaltyLevel.None, new DateTime(1978, 11, 3),  null, "Nørregade 12",   "Odense",    "5000"),
                Customer.Create("Laura Hansen",      "45678901", "laura@mail.dk",     LoyaltyLevel.None, new DateTime(1995, 1, 8),   null, "Søndergade 7",   "Aalborg",   "9000"),
                Customer.Create("Thomas Christensen","56789012", "thomas@mail.dk",    LoyaltyLevel.None, new DateTime(1982, 7, 19),  null, "Hovedgaden 3",   "Esbjerg",   "6700"),
                Customer.Create("Emma Pedersen",     "67890123", "emma@mail.dk",      LoyaltyLevel.None, new DateTime(1993, 4, 25),  null, "Strandvejen 9",  "København", "1100"),
                Customer.Create("Jonas Andersen",    "78901234", "jonas@mail.dk",     LoyaltyLevel.None, new DateTime(1988, 9, 14),  null, "Kirkegade 2",    "Aarhus",    "8000"),
                Customer.Create("Maja Larsen",       "89012345", "maja@mail.dk",      LoyaltyLevel.None, new DateTime(1975, 12, 30), null, "Skolegade 18",   "Odense",    "5000"),
                Customer.Create("Christian Møller",  "90123456", "christian@mail.dk", LoyaltyLevel.None, new DateTime(1980, 2, 11),  null, "Østergade 24",   "Aalborg",   "9000"),
                Customer.Create("Ida Thomsen",       "01234567", "ida@mail.dk",       LoyaltyLevel.None, new DateTime(1997, 5, 6),   null, "Vestergade 33",  "Esbjerg",   "6700"),
                Customer.Create("Rasmus Eriksen",    "11223344", "rasmus@mail.dk",    LoyaltyLevel.None, new DateTime(1991, 8, 17),  null, "Nørregade 6",    "København", "1100"),
                Customer.Create("Nora Kristiansen",  "22334455", "nora@mail.dk",      LoyaltyLevel.None, new DateTime(1986, 10, 28), null, "Søndergade 14",  "Aarhus",    "8000"),
                Customer.Create("Frederik Poulsen",  "33445566", "frederik@mail.dk",  LoyaltyLevel.None, new DateTime(1979, 3, 9),   null, "Hovedgaden 41",  "Odense",    "5000"),
                Customer.Create("Anna Jakobsen",     "44556677", "anna@mail.dk",      LoyaltyLevel.None, new DateTime(1994, 6, 3),   null, "Strandvejen 16", "Aalborg",   "9000"),
                Customer.Create("Oliver Olsen",      "55667788", "oliver@mail.dk",    LoyaltyLevel.None, new DateTime(1983, 1, 21),  null, "Kirkegade 8",    "Esbjerg",   "6700"),
                Customer.Create("Freja Sørensen",    "66778899", "freja@mail.dk",     LoyaltyLevel.None, new DateTime(1996, 4, 12),  null, "Skolegade 3",    "København", "1100"),
                Customer.Create("Noah Rasmussen",    "77889900", "noah@mail.dk",      LoyaltyLevel.None, new DateTime(1987, 7, 7),   null, "Østergade 19",   "Aarhus",    "8000"),
                Customer.Create("Mathilde Jørgensen","88990011", "mathilde@mail.dk",  LoyaltyLevel.None, new DateTime(1992, 11, 16), null, "Vestergade 27",  "Odense",    "5000"),
                Customer.Create("Victor Petersen",   "99001122", "victor@mail.dk",    LoyaltyLevel.None, new DateTime(1976, 2, 4),   null, "Nørregade 31",   "Aalborg",   "9000"),
                Customer.Create("Cecilie Kristensen","10111213", "cecilie@mail.dk",   LoyaltyLevel.None, new DateTime(1989, 9, 23),  null, "Søndergade 22",  "Esbjerg",   "6700"),
            };

                context.Customers.AddRange(customers);
                context.SaveChanges();
            }
            if (!context.Clinics.Any())
            {
                var openingHours = new[]
                {
                 new ClinicOpeningHour(DayOfWeek.Monday,    new TimeOnly(8, 0), new TimeOnly(17, 0)),
                 new ClinicOpeningHour(DayOfWeek.Tuesday,   new TimeOnly(8, 0), new TimeOnly(17, 0)),
                 new ClinicOpeningHour(DayOfWeek.Wednesday, new TimeOnly(8, 0), new TimeOnly(17, 0)),
                 new ClinicOpeningHour(DayOfWeek.Thursday,  new TimeOnly(8, 0), new TimeOnly(17, 0)),
                 new ClinicOpeningHour(DayOfWeek.Friday,    new TimeOnly(8, 0), new TimeOnly(16, 0)),
                };

                var klinik1 = Clinic.Create("Vejle Klinik", 3, "Østergade 12", "Vejle", "7100", openingHours);
                var klinik2 = Clinic.Create("Egtved Klinik", 4, "Søndergade 5", "Egtved", "8000", openingHours);
                var klinik3 = Clinic.Create("Vejle 2 Klinik", 2, "Vestergade 22", "Vejle", "7100", openingHours);

                context.Clinics.AddRange(klinik1, klinik2, klinik3);
                context.SaveChanges();


                if (context.Practitioners.Any()) return;

                var practitioners = new List<Practitioner>
                {
                    Practitioner.Create("Anders Nielsen", "anders@klinik.dk", "12345678", "AUTH001", AuthorizationType.Physiotherapist),
                    Practitioner.Create("Sofie Madsen", "sofie@klinik.dk", "23456789", "AUTH002", AuthorizationType.Masseur),
                    Practitioner.Create("Mikkel Jensen", "mikkel@klinik.dk", "34567890", "AUTH003", AuthorizationType.Acupuncturist),
                    Practitioner.Create("Laura Hansen", "laura@klinik.dk", "45678901", "AUTH004", AuthorizationType.Nutritionist),
                    Practitioner.Create("Thomas Christensen", "thomas@klinik.dk", "56789012", "AUTH005", AuthorizationType.Physiotherapist),
                    Practitioner.Create("Emma Pedersen", "emma@klinik.dk", "67890123", "AUTH006", AuthorizationType.Masseur),
                    Practitioner.Create("Jonas Andersen", "jonas@klinik.dk", "78901234", "AUTH007", AuthorizationType.Acupuncturist),
                    Practitioner.Create("Maja Larsen", "maja@klinik.dk", "89012345", "AUTH008", AuthorizationType.Nutritionist),
                    Practitioner.Create("Christian Møller", "christian@klinik.dk", "90123456", "AUTH009", AuthorizationType.Physiotherapist),
                    Practitioner.Create("Ida Thomsen", "ida@klinik.dk", "01234567", "AUTH010", AuthorizationType.Masseur),
                    Practitioner.Create("Rasmus Eriksen", "rasmus@klinik.dk", "11223344", "AUTH011", AuthorizationType.Acupuncturist),
                    Practitioner.Create("Nora Kristiansen", "nora@klinik.dk", "22334455", "AUTH012", AuthorizationType.Nutritionist),
                };

                context.Practitioners.AddRange(practitioners);
                context.SaveChanges();
            }

            if (!context.PractitionerClinicDays.Any())
            {
                var practitioners = context.Practitioners.ToList();
                var clinics = context.Clinics.ToList();
                var now = DateTime.Now;

                var clinicDays = new List<PractitionerClinicDay>();

                foreach (var practitioner in practitioners)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        var date = now.AddDays(i).Date;
                        if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                        {
                            clinicDays.Add(new PractitionerClinicDay(
                                practitioner.Id,
                                clinics[0].Id,
                                date));
                        }
                    }
                }

                context.PractitionerClinicDays.AddRange(clinicDays);
                context.SaveChanges();
            }

            if (!context.Bookings.Any())
            {
                var customers = context.Customers.ToList();
                var practitioners = context.Practitioners.ToList();
                var clinics = context.Clinics.ToList();
                var treatments = context.Treatments.ToList();

                var rng = new Random(42);
                var now = DateTime.Now;
                var bookings = new List<Booking>();


                for (int i = 0; i < 30; i++)
                {
                    var start = now.AddDays(rng.Next(1, 7)).Date.AddHours(rng.Next(8, 16));
                    var end = start.AddHours(1);
                    var treatment = treatments[rng.Next(treatments.Count)];
                    var basePrice = new Money(treatment.BasePrice.Amount);

                    var booking = Booking.Create(
                        customerId: customers[rng.Next(customers.Count)].Id,
                        practitionerId: practitioners[rng.Next(practitioners.Count)].Id,
                        clinicId: clinics[rng.Next(clinics.Count)].Id,
                        treatmentTypeId: treatment.Id,
                        timeRange: new TimeRange(start, end),
                        basePrice: basePrice);

                    booking.SetFinalPrice(new Money(treatment.BasePrice.Amount), DiscountType.None);
                    bookings.Add(booking);
                }

                context.Bookings.AddRange(bookings);
                context.SaveChanges();
            }
        }
    }
}
