using Microsoft.EntityFrameworkCore;
using task2.Data;
using task2.Domain.Entities;
using task2.Domain.Enums;
using task2.Services.Interfaces;

namespace task2.Services.Implementations;

public class GroupService : IGroupService
{
    private readonly SplitwiseDbContext _context;

    public GroupService(SplitwiseDbContext context)
    {
        _context = context;
    }

    public async Task<Group> CreateGroupAsync(Guid userId, string name, string currency)
    {
        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = name,
            Currency = currency,
            CreatedById = userId,
            Type = GroupType.Home
        };

        _context.Groups.Add(group);

        // Додаємо творця в учасники
        _context.GroupMembers.Add(new GroupMember
        {
            Id = Guid.NewGuid(),
            GroupId = group.Id,
            UserId = userId,
            Role = MemberRole.Admin
        });

        await _context.SaveChangesAsync();
        return group;
    }

    public async Task<IEnumerable<Group>> GetUserGroupsAsync(Guid userId)
    {
        return await _context.GroupMembers
            .Where(gm => gm.UserId == userId)
            .Select(gm => gm.Group)
            .ToListAsync();
    }
}