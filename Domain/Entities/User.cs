using Microsoft.AspNetCore.Identity;

namespace task2.Domain.Entities;

// Успадковуємо IdentityUser<Guid>, щоб зберегти ваш тип Id
public class User : IdentityUser<Guid>
{
    // Id, Email та PasswordHash вже є в IdentityUser, тому ми їх видалили

    public string FullName { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public string DefaultCurrency { get; set; } = "USD";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Ваші навігаційні властивості
    public ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();
    public ICollection<Expense> ExpensesPaid { get; set; } = new List<Expense>();
    public ICollection<ExpenseSplit> ExpenseSplits { get; set; } = new List<ExpenseSplit>();
}