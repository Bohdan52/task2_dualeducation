using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using task2.Data;
using task2.Domain.Enums;
using task2.Services.Implementations;
using Xunit;

namespace task2.Tests;

public class GroupServiceTests
{
    private readonly SplitwiseDbContext _context;
    private readonly GroupService _groupService;

    public GroupServiceTests()
    {
        // Налаштовуємо віртуальну базу даних (унікальна назва для кожного тесту)
        var options = new DbContextOptionsBuilder<SplitwiseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new SplitwiseDbContext(options);
        _groupService = new GroupService(_context);
    }

    [Fact]
    public async Task CreateGroupAsync_ValidData_CreatesGroupAndMember()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var groupName = "Test Group";
        var currency = "USD";

        // Act
        var result = await _groupService.CreateGroupAsync(userId, groupName, currency);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(groupName, result.Name);

        // Перевіряємо, що група дійсно збережена у віртуальній БД
        var groupInDb = await _context.Groups.FirstOrDefaultAsync(g => g.Id == result.Id);
        Assert.NotNull(groupInDb);

        // Перевіряємо, що творця автоматично додано як учасника (Admin)
        var memberInDb = await _context.GroupMembers
            .FirstOrDefaultAsync(m => m.GroupId == result.Id && m.UserId == userId);

        Assert.NotNull(memberInDb);
        Assert.Equal(MemberRole.Admin, memberInDb.Role);
    }

    [Fact]
    public async Task GetUserGroupsAsync_UserHasGroups_ReturnsGroupsList()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Створюємо дві групи для цього користувача
        await _groupService.CreateGroupAsync(userId, "Group 1", "USD");
        await _groupService.CreateGroupAsync(userId, "Group 2", "EUR");

        // Act
        var result = await _groupService.GetUserGroupsAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count()); // Очікуємо, що повернеться рівно 2 групи
    }
}