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
                // Mix of loyalty levels so the discount engine can be tested
                Customer.Create("Lars Frederiksen",      "20110001", "lars.frederiksen@mail.dk",      LoyaltyLevel.None, new DateTime(1984, 1, 12),  null, "Birkevej 4",        "Vejle",      "7100"),
                Customer.Create("Camilla Holm",          "20110002", "camilla.holm@mail.dk",          LoyaltyLevel.None, new DateTime(1992, 2, 8),   null, "Parkvej 12",        "Kolding",    "6000"),
                Customer.Create("Sebastian Koch",        "20110003", "sebastian.koch@mail.dk",        LoyaltyLevel.None, new DateTime(1988, 3, 27),  null, "Sønderbrogade 9",   "Odense",     "5000"),
                Customer.Create("Julie Winther",         "20110004", "julie.winther@mail.dk",         LoyaltyLevel.None, new DateTime(1995, 4, 2),   null, "Skovbakken 2",      "Aarhus",     "8000"),
                Customer.Create("Henrik Dahl",           "20110005", "henrik.dahl@mail.dk",           LoyaltyLevel.None, new DateTime(1979, 5, 18),  null, "Nørregade 21",      "Esbjerg",    "6700"),
                Customer.Create("Maria Iversen",         "20110006", "maria.iversen@mail.dk",         LoyaltyLevel.None, new DateTime(1990, 6, 11),  null, "Øster Alle 15",     "Fredericia", "7000"),
                Customer.Create("Peter Broberg",         "20110007", "peter.broberg@mail.dk",         LoyaltyLevel.None, new DateTime(1981, 7, 6),   null, "Havnevej 8",        "Horsens",    "8700"),
                Customer.Create("Katrine Lund",          "20110008", "katrine.lund@mail.dk",          LoyaltyLevel.None, new DateTime(1996, 8, 29),  null, "Engvej 18",         "Aalborg",    "9000"),
                Customer.Create("Nicolai Bruun",         "20110009", "nicolai.bruun@mail.dk",         LoyaltyLevel.None, new DateTime(1987, 9, 4),   null, "Møllegade 5",       "Silkeborg",  "8600"),
                Customer.Create("Sara Kjær",             "20110010", "sara.kjaer@mail.dk",            LoyaltyLevel.None, new DateTime(1993, 10, 14), null, "Bakkevej 17",       "Randers",    "8900"),
            
                Customer.Create("Emil Toft",             "20110011", "emil.toft@mail.dk",             LoyaltyLevel.None, new DateTime(1985, 11, 3),  null, "Vestergade 33",     "Vejle",      "7100"),
                Customer.Create("Louise Birk",           "20110012", "louise.birk@mail.dk",           LoyaltyLevel.None, new DateTime(1991, 12, 25), null, "Kystvejen 1",       "Kolding",    "6000"),
                Customer.Create("Rikke Mortensen",       "20110013", "rikke.mortensen@mail.dk",       LoyaltyLevel.None, new DateTime(1982, 1, 7),   null, "Åboulevarden 20",   "Aarhus",     "8000"),
                Customer.Create("Mads Vestergaard",      "20110014", "mads.vestergaard@mail.dk",      LoyaltyLevel.None, new DateTime(1994, 2, 19),  null, "Lindevej 11",       "Odense",     "5000"),
                Customer.Create("Trine Bach",            "20110015", "trine.bach@mail.dk",            LoyaltyLevel.None, new DateTime(1989, 3, 15),  null, "Skolegade 6",       "Esbjerg",    "6700"),
                Customer.Create("Jakob Hviid",           "20110016", "jakob.hviid@mail.dk",           LoyaltyLevel.None, new DateTime(1977, 4, 21),  null, "Fælledvej 2",       "Fredericia", "7000"),
                Customer.Create("Line Storm",            "20110017", "line.storm@mail.dk",            LoyaltyLevel.None, new DateTime(1998, 5, 13),  null, "Torvegade 14",      "Horsens",    "8700"),
                Customer.Create("Casper Riis",           "20110018", "casper.riis@mail.dk",           LoyaltyLevel.None, new DateTime(1986, 6, 1),   null, "Stationsvej 9",     "Aalborg",    "9000"),
                Customer.Create("Mille Bohn",            "20110019", "mille.bohn@mail.dk",            LoyaltyLevel.None, new DateTime(1990, 7, 9),   null, "Strandvej 44",      "Silkeborg",  "8600"),
                Customer.Create("Andreas Nygaard",       "20110020", "andreas.nygaard@mail.dk",       LoyaltyLevel.None, new DateTime(1983, 8, 30),  null, "Granvej 19",        "Randers",    "8900"),
            
                Customer.Create("Signe Fisker",          "20110021", "signe.fisker@mail.dk",          LoyaltyLevel.None, new DateTime(1995, 9, 12),  null, "Elmosevej 3",       "Vejle",      "7100"),
                Customer.Create("Magnus Olesen",         "20110022", "magnus.olesen@mail.dk",         LoyaltyLevel.None, new DateTime(1988, 10, 22), null, "Nygade 16",         "Kolding",    "6000"),
                Customer.Create("Helene Gade",           "20110023", "helene.gade@mail.dk",           LoyaltyLevel.None, new DateTime(1991, 11, 5),  null, "Bredgade 8",        "Odense",     "5000"),
                Customer.Create("Frederik Brandt",       "20110024", "frederik.brandt@mail.dk",       LoyaltyLevel.None, new DateTime(1980, 12, 28), null, "Skovvej 27",        "Aarhus",     "8000"),
                Customer.Create("Josefine Lind",         "20110025", "josefine.lind@mail.dk",         LoyaltyLevel.None, new DateTime(1997, 1, 18),  null, "Højmarksvej 7",     "Esbjerg",    "6700"),
                Customer.Create("Martin Quist",          "20110026", "martin.quist@mail.dk",          LoyaltyLevel.None, new DateTime(1984, 2, 2),   null, "Birkelunden 10",    "Fredericia", "7000"),
                Customer.Create("Nanna Schou",           "20110027", "nanna.schou@mail.dk",           LoyaltyLevel.None, new DateTime(1992, 3, 24),  null, "Teglgårdsvej 5",    "Horsens",    "8700"),
                Customer.Create("Tobias Munk",           "20110028", "tobias.munk@mail.dk",           LoyaltyLevel.None, new DateTime(1985, 4, 17),  null, "Lærkevej 13",       "Aalborg",    "9000"),
                Customer.Create("Clara Foldager",        "20110029", "clara.foldager@mail.dk",        LoyaltyLevel.None, new DateTime(1993, 5, 26),  null, "Hedetoften 1",      "Silkeborg",  "8600"),
                Customer.Create("Alexander Bech",        "20110030", "alexander.bech@mail.dk",        LoyaltyLevel.None, new DateTime(1989, 6, 20),  null, "Skippergade 9",     "Randers",    "8900"),
            
                Customer.Create("Victoria Hald",         "20110031", "victoria.hald@mail.dk",         LoyaltyLevel.None, new DateTime(1996, 7, 7),   null, "Mosevej 6",         "Vejle",      "7100"),
                Customer.Create("Simon Vang",            "20110032", "simon.vang@mail.dk",            LoyaltyLevel.None, new DateTime(1982, 8, 11),  null, "Kløvervej 22",      "Kolding",    "6000"),
                Customer.Create("Laura Kjeldsen",        "20110033", "laura.kjeldsen@mail.dk",        LoyaltyLevel.None, new DateTime(1994, 9, 3),   null, "Ørnevej 4",         "Odense",     "5000"),
                Customer.Create("Benjamin Høyer",        "20110034", "benjamin.hoyer@mail.dk",        LoyaltyLevel.None, new DateTime(1981, 10, 15), null, "Rosenvej 18",       "Aarhus",     "8000"),
                Customer.Create("Celine Kragh",          "20110035", "celine.kragh@mail.dk",          LoyaltyLevel.None, new DateTime(1990, 11, 9),  null, "Havnegade 12",      "Esbjerg",    "6700"),
                Customer.Create("Jon Frederiksen",       "20110036", "jon.frederiksen@mail.dk",       LoyaltyLevel.None, new DateTime(1987, 12, 1),  null, "Ahornvej 2",        "Fredericia", "7000"),
                Customer.Create("Mia Husted",            "20110037", "mia.husted@mail.dk",            LoyaltyLevel.None, new DateTime(1998, 1, 27),  null, "Lundtoften 14",     "Horsens",    "8700"),
                Customer.Create("Filip Neergaard",       "20110038", "filip.neergaard@mail.dk",       LoyaltyLevel.None, new DateTime(1986, 2, 16),  null, "Bøgevej 30",        "Aalborg",    "9000"),
                Customer.Create("Alberte Skov",          "20110039", "alberte.skov@mail.dk",          LoyaltyLevel.None, new DateTime(1992, 3, 8),   null, "Møllebakken 5",     "Silkeborg",  "8600"),
                Customer.Create("Oscar Sejr",            "20110040", "oscar.sejr@mail.dk",            LoyaltyLevel.None, new DateTime(1983, 4, 23),  null, "Enghaven 17",       "Randers",    "8900"),
            };

                context.Customers.AddRange(customers);
                context.SaveChanges();
            }
            if (!context.Clinics.Any())
            {
                // IMPORTANT: each clinic needs its OWN set of ClinicOpeningHour instances.
                // Sharing the same object instances across multiple clinics causes EF Core to
                // overwrite the shadow ClinicId FK on each instance as it processes them —
                // so only the LAST clinic ends up with opening-hour rows in the database.
                // The fix: a local function that always returns fresh object instances.
                static ClinicOpeningHour[] MakeHours() => new[]
                {
                    new ClinicOpeningHour(DayOfWeek.Monday,    new TimeOnly(8, 0), new TimeOnly(17, 0)),
                    new ClinicOpeningHour(DayOfWeek.Tuesday,   new TimeOnly(8, 0), new TimeOnly(17, 0)),
                    new ClinicOpeningHour(DayOfWeek.Wednesday, new TimeOnly(8, 0), new TimeOnly(17, 0)),
                    new ClinicOpeningHour(DayOfWeek.Thursday,  new TimeOnly(8, 0), new TimeOnly(17, 0)),
                    new ClinicOpeningHour(DayOfWeek.Friday,    new TimeOnly(8, 0), new TimeOnly(16, 0)),
                };

                var klinik1 = Clinic.Create("Vejle Klinik", 3, "Østergade 12", "Vejle", "7100", MakeHours());
                var klinik2 = Clinic.Create("Egtved Klinik", 4, "Søndergade 5", "Egtved", "8000", MakeHours());
                var klinik3 = Clinic.Create("Vejle 2 Klinik", 2, "Vestergade 22", "Vejle", "7100", MakeHours());

                context.Clinics.AddRange(klinik1, klinik2, klinik3);
                context.SaveChanges();
            }

            if (!context.Practitioners.Any())
            {
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

            // Always keep clinic days covering a rolling year from today.
            // On each startup we only ADD forward-looking days — we never
            // delete past rows. Past PractitionerClinicDay rows are harmless
            // and keeping them preserves a complete historical schedule record
            // (useful if anything ever needs to look up where a practitioner
            // was working on the day of an old booking).
            var today = DateTime.Today;
            var oneYearAhead = today.AddDays(365);

            // Find the furthest date already covered (if any).
            var latestSeeded = context.PractitionerClinicDays.Any()
                ? context.PractitionerClinicDays.Max(pc => pc.Date)
                : today.AddDays(-1);

            // Insert only the days not yet present.
            if (latestSeeded < oneYearAhead)
            {
                var practitioners = context.Practitioners.ToList();
                var clinics = context.Clinics.ToList();
                var newClinicDays = new List<PractitionerClinicDay>();

                // Each practitioner has a unique weekly schedule pattern.
                // Patterns are defined as 5 values (Mon–Fri) where:
                //   0 = primary clinic,  1 = secondary clinic
                //
                // This gives realistic variety:
                //   - some work all week at one clinic
                //   - some split 1+4, 2+3, 3+2, or 4+1 days
                //   - the split days are not always Mon–Wed / Thu–Fri
                //
                // The primary/secondary clinic pair rotates across practitioners
                // so every clinic gets an even spread of staff.
                //
                // A practitioner is never at two clinics on the same day,
                // satisfying the PractitionerClinicDay domain rule.
                var schedulePatterns = new int[][]
                {
                    new[] { 0, 0, 0, 0, 0 },  //  0: all 5 days at primary only
                    new[] { 1, 1, 1, 1, 1 },  //  1: all 5 days at secondary only
                    new[] { 0, 0, 0, 1, 1 },  //  2: Mon-Wed primary,  Thu-Fri secondary
                    new[] { 0, 1, 1, 1, 1 },  //  3: Mon primary,      Tue-Fri secondary
                    new[] { 0, 0, 1, 1, 1 },  //  4: Mon-Tue primary,  Wed-Fri secondary
                    new[] { 0, 0, 0, 0, 1 },  //  5: Mon-Thu primary,  Fri secondary
                    new[] { 1, 1, 0, 0, 0 },  //  6: Mon-Tue secondary, Wed-Fri primary
                    new[] { 1, 1, 1, 0, 0 },  //  7: Mon-Wed secondary, Thu-Fri primary
                    new[] { 1, 0, 0, 0, 0 },  //  8: Mon secondary,    Tue-Fri primary
                    new[] { 1, 0, 0, 0, 1 },  //  9: Mon+Fri secondary, Tue-Thu primary
                    new[] { 0, 1, 1, 0, 0 },  // 10: Tue-Wed secondary, Mon+Thu+Fri primary
                    new[] { 1, 0, 0, 1, 1 },  // 11: Mon+Thu+Fri secondary, Tue-Wed primary
                };

                for (int practIndex = 0; practIndex < practitioners.Count; practIndex++)
                {
                    var practitioner = practitioners[practIndex];
                    var n = clinics.Count;
                    var primary = clinics[practIndex % n];
                    var secondary = clinics[(practIndex + 1) % n];
                    var pattern = schedulePatterns[practIndex % schedulePatterns.Length];

                    for (var date = latestSeeded.AddDays(1); date <= oneYearAhead; date = date.AddDays(1))
                    {
                        var dayIndex = date.DayOfWeek switch
                        {
                            DayOfWeek.Monday => 0,
                            DayOfWeek.Tuesday => 1,
                            DayOfWeek.Wednesday => 2,
                            DayOfWeek.Thursday => 3,
                            DayOfWeek.Friday => 4,
                            _ => -1   // skip weekends
                        };

                        if (dayIndex < 0) continue;

                        var clinic = pattern[dayIndex] == 0 ? primary : secondary;

                        newClinicDays.Add(new PractitionerClinicDay(
                            practitioner.Id,
                            clinic.Id,
                            date));
                    }
                }

                context.PractitionerClinicDays.AddRange(newClinicDays);
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

                // All customers start with LoyaltyLevel.None in the DB.
                // We distribute them across spending tiers by index so that
                // CustomerQuerries and BookingPricingFacadeHandler — which both
                // calculate loyalty live from booking history — will resolve each
                // customer to the correct level at runtime.
                //
                // Thresholds (from Customer.UpdateLoyaltyLevel):
                //   Gold   >= 10 000 DKK  → target 10 500
                //   Silver >=  5 000 DKK  → target  5 500
                //   Bronze >=  1 000 DKK  → target  1 500
                //   None   <   1 000 DKK  → no bookings (total = 0)
                //
                // Distribution across 20 customers (by index % 4):
                //   0 → Gold    (indices 0, 4, 8, 12, 16)
                //   1 → Silver  (indices 1, 5, 9, 13, 17)
                //   2 → Bronze  (indices 2, 6, 10, 14, 18)
                //   3 → None    (indices 3, 7, 11, 15, 19)

                for (int i = 0; i < customers.Count; i++)
                {
                    var customer = customers[i];

                    decimal targetTotal = (i % 4) switch
                    {
                        0 => 10000m,   // Gold tier   (>= 10 000)
                        1 => 5000m,   // Silver tier (>=  5 000)
                        2 => 1000m,   // Bronze tier (>=  1 000)
                        _ => 0m    // None: no history needed
                    };

                    decimal runningTotal = 0m;

                    while (runningTotal < targetTotal)
                    {
                        var treatment = treatments[rng.Next(treatments.Count)];
                        var practitioner = practitioners[rng.Next(practitioners.Count)];
                        var clinic = clinics[rng.Next(clinics.Count)];

                        var start = now.AddDays(-rng.Next(1, 365)).Date
                                       .AddHours(rng.Next(8, 16));
                        var end = start.AddMinutes(treatment.DurationMinutes);

                        var booking = Booking.Create(
                            customerId: customer.Id,
                            practitionerId: practitioner.Id,
                            clinicId: clinic.Id,
                            treatmentTypeId: treatment.Id,
                            timeRange: new TimeRange(start, end),
                            basePrice: new Money(treatment.BasePrice.Amount));

                        booking.SetFinalPrice(new Money(treatment.BasePrice.Amount), DiscountType.None);

                        bookings.Add(booking);
                        runningTotal += treatment.BasePrice.Amount;
                    }
                }

                context.Bookings.AddRange(bookings);
                context.SaveChanges();
            }
        }
    }
}
