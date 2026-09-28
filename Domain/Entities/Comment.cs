namespace task2.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public Guid ExpenseId { get; set; }
    public Expense Expense { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}