namespace task2.Dtos;

public record CreateUserRequest(string Email, string FullName, string Password);

public record UserResponse(Guid Id, string Email, string FullName, string DefaultCurrency);