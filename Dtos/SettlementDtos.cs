namespace task2.Dtos;

public record CreateSettlementRequest(
    Guid? GroupId,
    Guid FromUserId,
    Guid ToUserId,
    decimal Amount,
    string Currency,
    string? Note,
    DateOnly Date
);

public record SettlementResponse(
    Guid Id,
    Guid? GroupId,
    Guid FromUserId,
    Guid ToUserId,
    decimal Amount,
    string Currency,
    string? Note,
    DateOnly Date,
    DateTime CreatedAt
);