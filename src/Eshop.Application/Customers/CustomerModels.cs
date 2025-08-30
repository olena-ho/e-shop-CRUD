namespace Eshop.Application.Customers;

public record CustomerResponse(int Id, string FirstName, string LastName, string Email, string? Phone);

public record CustomerRequestDto(string FirstName, string LastName, string Email, string? Phone);

