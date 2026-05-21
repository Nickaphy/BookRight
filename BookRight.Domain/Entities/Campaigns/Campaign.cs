// Represents a marketing campaign

// Rules:
// - Has start and end date
// - Applies to selected treatment types

using BookRight.Domain.Common;

namespace BookRight.Domain.Campaigns;

public class Campaign : AggregateRoot
{
    public string Name { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public int DiscountPercent { get; private set; }

    
    private Campaign() { }

    public Campaign(string name,
        DateOnly startDate,
        DateOnly endDate,
        int discountPercent
        )
    {
        Validate(name, startDate, endDate, discountPercent);
        
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        DiscountPercent = discountPercent;
    }
    
    public void Update(string name, DateOnly startDate, DateOnly endDate, int discountPercent)
    {
        Validate(name, startDate, endDate, discountPercent);
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        DiscountPercent = discountPercent;
    }
    
    private static void Validate(string name, DateOnly startDate, DateOnly endDate, int discountPercent)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be after end date.", nameof(startDate));
        if (discountPercent < 0 || discountPercent > 100)
            throw new ArgumentException("Discount percent must be between 0 and 100.", nameof(discountPercent));
    }

}
