using System.Xml.Linq;
using task2.Domain.Enums;

namespace task2.Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string? Category { get; set; }
    public DateOnly Date { get; set; }
    public Guid PaidById { get; set; }
    public User PaidBy { get; set; } = null!;
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
    public string? ReceiptUrl { get; set; }
    public SplitType SplitType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<ExpenseSplit> Splits { get; set; } = new List<ExpenseSplit>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}