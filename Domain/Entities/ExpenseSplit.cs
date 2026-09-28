namespace task2.Domain.Entities;

public class ExpenseSplit
{
    public Guid Id { get; set; }
    public Guid ExpenseId { get; set; }
    public Expense Expense { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public decimal AmountOwed { get; set; }
    public decimal? ShareValue { get; set; }
}