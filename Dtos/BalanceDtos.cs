namespace task2.Dtos;

public record UserBalance(Guid UserId, string Currency, decimal Net);

public record DebtEdge(Guid FromUserId, Guid ToUserId, decimal Amount, string Currency);