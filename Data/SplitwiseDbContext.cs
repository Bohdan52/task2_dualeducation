using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using task2.Domain.Entities;

namespace task2.Data;

public class SplitwiseDbContext : DbContext
{
    public SplitwiseDbContext(DbContextOptions<SplitwiseDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<ExpenseSplit> ExpenseSplits => Set<ExpenseSplit>();
    public DbSet<Settlement> Settlements => Set<Settlement>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ---------- User ----------
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).HasMaxLength(255).IsRequired();
            e.Property(u => u.FullName).HasMaxLength(100).IsRequired();
            e.Property(u => u.DefaultCurrency).HasMaxLength(3).IsRequired();
        });

        // ---------- Group ----------
        modelBuilder.Entity<Group>(e =>
        {
            e.Property(g => g.Name).HasMaxLength(100).IsRequired();
            e.Property(g => g.Currency).HasMaxLength(3).IsRequired();
            e.HasOne(g => g.CreatedBy).WithMany()
                .HasForeignKey(g => g.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- GroupMember ----------
        modelBuilder.Entity<GroupMember>(e =>
        {
            e.HasIndex(gm => new { gm.GroupId, gm.UserId }).IsUnique();

            e.HasOne(gm => gm.Group).WithMany(g => g.Members)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(gm => gm.User).WithMany(u => u.GroupMemberships)
                .HasForeignKey(gm => gm.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Expense ----------
        modelBuilder.Entity<Expense>(e =>
        {
            e.Property(x => x.Amount).HasPrecision(18, 2);
            e.Property(x => x.Description).HasMaxLength(255).IsRequired();
            e.Property(x => x.Currency).HasMaxLength(3).IsRequired();
            e.Property(x => x.Category).HasMaxLength(50);

            e.HasOne(x => x.PaidBy).WithMany(u => u.ExpensesPaid)
                .HasForeignKey(x => x.PaidById)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.CreatedBy).WithMany()
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Group).WithMany(g => g.Expenses)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasIndex(x => new { x.GroupId, x.Date });
            e.HasIndex(x => x.PaidById);

            e.HasQueryFilter(x => x.DeletedAt == null);
        });

        // ---------- ExpenseSplit ----------
        modelBuilder.Entity<ExpenseSplit>(e =>
        {
            e.HasIndex(es => new { es.ExpenseId, es.UserId }).IsUnique();
            e.Property(es => es.AmountOwed).HasPrecision(18, 2);
            e.Property(es => es.ShareValue).HasPrecision(12, 4);

            e.HasOne(es => es.Expense).WithMany(x => x.Splits)
                .HasForeignKey(es => es.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(es => es.User).WithMany(u => u.ExpenseSplits)
                .HasForeignKey(es => es.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Settlement ----------
        modelBuilder.Entity<Settlement>(e =>
        {
            e.Property(s => s.Amount).HasPrecision(18, 2);
            e.Property(s => s.Currency).HasMaxLength(3).IsRequired();

            e.HasOne(s => s.FromUser).WithMany()
                .HasForeignKey(s => s.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(s => s.ToUser).WithMany()
                .HasForeignKey(s => s.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(s => s.Group).WithMany()
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasIndex(s => new { s.GroupId, s.Date });

            e.ToTable(t => t.HasCheckConstraint("ck_settlement_not_self",
                "\"FromUserId\" <> \"ToUserId\""));
        });

        // ---------- Comment ----------
        modelBuilder.Entity<Comment>(e =>
        {
            e.Property(c => c.Text).HasMaxLength(2000).IsRequired();

            e.HasOne(c => c.Expense).WithMany(x => x.Comments)
                .HasForeignKey(c => c.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(c => c.User).WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(modelBuilder);
    }

    // Автоматично проставляє CreatedAt / UpdatedAt
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                switch (entry.Entity)
                {
                    case User u: u.CreatedAt = now; u.UpdatedAt = now; break;
                    case Group g: g.CreatedAt = now; g.UpdatedAt = now; break;
                    case Expense ex: ex.CreatedAt = now; ex.UpdatedAt = now; break;
                    case GroupMember gm: gm.JoinedAt = now; break;
                    case Comment c: c.CreatedAt = now; break;
                    case Settlement s: s.CreatedAt = now; break;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                switch (entry.Entity)
                {
                    case User u: u.UpdatedAt = now; break;
                    case Group g: g.UpdatedAt = now; break;
                    case Expense ex: ex.UpdatedAt = now; break;
                }
            }
        }

        return base.SaveChangesAsync(ct);
    }
}