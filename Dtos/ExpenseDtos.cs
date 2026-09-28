using task2.Domain.Enums;

namespace task2.Dtos;

public record SplitInputDto(
    Guid UserId,
    decimal? Amount = null,
    decimal? Percent = null,
    decimal? Shares = null
);

public record CreateExpenseRequest(
    Guid? GroupId,
    string Description,
    decimal Amount,
    string Currency,
    string? Category,
    DateOnly Date,
    Guid PaidById,
    SplitType SplitType,
    List<SplitInputDto> Participants
);

public record ExpenseSplitResponse(
    Guid UserId,
    decimal AmountOwed,
    decimal? ShareValue
);

public record ExpenseResponse(
    Guid Id,
    Guid? GroupId,
    string Description,
    decimal Amount,
    string Currency,
    string? Category,
    DateOnly Date,
    Guid PaidById,
    Guid CreatedById,
    SplitType SplitType,
    DateTime CreatedAt,
    List<ExpenseSplitResponse> Splits
);