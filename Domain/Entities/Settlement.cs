namespace task2.Domain.Entities;

public class Settlement
{
    public Guid Id { get; set; }
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
    public Guid FromUserId { get; set; }
    public User FromUser { get; set; } = null!;
    public Guid ToUserId { get; set; }
    public User ToUser { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string? Note { get; set; }
    public DateOnly Date { get; set; }
    public DateTime CreatedAt { get; set; }
}