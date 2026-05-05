# BookRight — Claude Code Context

## ⚠️ Team workflow — read this first
This project is built by a team. Each member owns specific areas of the codebase.
**Do not touch files outside the area you are explicitly told to work on.**
If a change requires touching a file owned by someone else, stop and ask first.
Always ask for clarification before making assumptions about unfinished or missing parts.
When in doubt — ask, don't guess.

---

## Project overview
BookRight Klinik & Wellness ApS — a Danish clinic booking system for receptionists.
Three clinics in the Vejle area offering fysioterapi, sportsmassage, akupunktur, kostvejledning and holdtræning.
Internal administration system. One user role: receptionist.

**Tech stack**
- C# / .NET 10
- Blazor (UI)
- Entity Framework Core (Code First)
- SQL database

---

## Architecture — Clean Architecture (Onion)
Strict dependency rule: dependencies point inward only. Domain knows nothing about infrastructure or UI.

```
BookRight.Domain          → Entities, Value Objects, Enums, Interfaces
BookRight.Application     → Use Cases, Commands, Queries (CQS)
BookRight.Facade          → DTOs (C# records), Facade interfaces
BookRight.Infrastructure  → EF Core, Repositories, DbContext
BookRight.UI              → Blazor components, Dependency Injection
```

**Dependency rule is non-negotiable.**
Domain has zero dependencies on any outer layer.
Infrastructure depends on Domain, never the other way around.

---

## Domain model

### Base classes (BookRight.Domain.Common)
```
Entity                → provides Guid Id, generated in constructor
AggregateRoot : Entity → base for all aggregate roots, will hold domain events later
```

### Aggregate roots (inherit from AggregateRoot)
- `Clinic` — treatment rooms, opening hours, address
- `Customer` — personal info, loyalty level, booking history
- `Booking` — coordinates treatment, practitioner, customer, price
- `Campaign` — time-limited discount campaigns

### Entities (inherit from Entity)
- `ClinicOpeningHour` — opening hours per weekday per clinic
- `Treatment` — treatment type, duration, base price
- `Practitioner` — name, authorization type, linked clinics

### Value Objects (BookRight.Domain.ValueObjects)
- `Money` — holds a decimal Amount, currency is always DKK
- `PriceCalculation` — receives all pricing inputs, calculates final price
- `Address` — Street, City, Zipcode (shared by Clinic and Customer)

### Enums (BookRight.Domain.Enums)
- `LoyaltyLevel` — Standard, Bronze, Silver, Gold
- `DiscountType` — None, Loyalty, Birthday, Campaign
- `TreatmentType` — Fysioterapi, Sportsmassage, Akupunktur, Kostvejledning, Holdtræning
- `AuthorizationType` — Fysioterapeut, Massør, Akupunktør, Kostvejleder

---

## Pricing rules
Base prices per treatment type and duration:
- Fysioterapi: 30 min 395 kr / 45 min 589 kr / 60 min 745 kr
- Sportsmassage: 30 min 350 kr / 60 min 699 kr
- Akupunktur: 45 min 550 kr
- Kostvejledning: første gang 60 min 799 kr / opfølgning 30 min 450 kr
- Holdtræning: 60 min 150 kr pr. deltager (max 6)

Evening and weekend bookings: +15% supplement on base price.

Discount types (best single discount wins — no stacking):
- Bronze loyalty (3.000–10.000 kr last 12 months): 5%
- Silver loyalty (10.001–25.000 kr last 12 months): 10%
- Gold loyalty (25.000+ kr last 12 months): 15%
- Birthday month: 25% on one treatment
- Campaign: variable percent, time-limited, per treatment type

PriceCalculation is called from Booking once Treatment, Customer and Campaign are resolved.

---

## Key patterns and principles

**SOLID — strictly enforced**
- Single Responsibility: every class does one thing
- Open/Closed: extend via new classes, not by modifying existing ones
- Liskov Substitution: subtypes must be substitutable for their base types
- Interface Segregation: small focused interfaces
- Dependency Inversion: depend on abstractions, not concretions

**Design patterns in use**
- Repository pattern (Infrastructure)
- Strategy pattern (discount calculation — required by exam)
- Dependency Injection throughout
- CQS — Commands change state, Queries return data, never both
- Factory method on Value Objects (e.g. PriceCalculation.Create(...))

**Domain rules**
- Entities have private setters — immutability enforced
- Guard clauses in constructors — invalid state is impossible
- AggregateRoot owns its ID via Entity base class — never passed in from outside
- Value Objects are immutable — no setters, created via static factory methods
- EF Core private parameterless constructor on every entity and value object

---

## What to always check before making changes
1. Does this file belong to the area I was asked to work on?
2. Does this change add a dependency that violates the dependency rule?
3. Does this break immutability or bypass guard clauses?
4. Does this touch a shared file (DbContext, migrations, appsettings)? If yes — ask first.
5. Is there missing context (e.g. a related entity not built yet)? If yes — ask before assuming.

---

## Git workflow
- `main` is protected — never push directly
- One branch per feature/area: `feature/clinic`, `feature/booking` etc.
- Always `git pull origin main` before starting a session
- Merge main into your feature branch regularly to stay in sync
- Pull Requests required to merge into main
- Commit messages must be meaningful: `"Add guard clauses to Customer constructor"` not `"update stuff"`
