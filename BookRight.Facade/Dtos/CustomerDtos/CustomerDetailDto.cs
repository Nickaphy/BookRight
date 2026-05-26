namespace BookRight.Facade.Dtos.CustomerDtos;

public record CustomerDetailDto(
    Guid Id,
    string Name,
    string PhoneNumber,
    string Email,
    DateTime DateOfBirth,
    string? Street,
    string City,
    string Zipcode,
    string? Note,
    CustomerLoyaltyLevel LoyaltyLevel,
    decimal TotalSpentLastYear);