using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task2.Data;

namespace task2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly SplitwiseDbContext _db;

    public TestController(SplitwiseDbContext db) => _db = db;

    /// <summary>Перевірка, що API запущений.</summary>
    [HttpGet]
    public IActionResult Get() => Ok(new
    {
        status = "API is working!",
        time = DateTime.UtcNow
    });

    /// <summary>Перевірка, що БД підключена і міграції застосовані.</summary>
    [HttpGet("db")]
    public async Task<IActionResult> CheckDb(CancellationToken ct)
    {
        try
        {
            var canConnect = await _db.Database.CanConnectAsync(ct);
            var usersCount = await _db.Users.CountAsync(ct);
            var groupsCount = await _db.Groups.CountAsync(ct);
            var expensesCount = await _db.Expenses.CountAsync(ct);

            return Ok(new
            {
                canConnect,
                users = usersCount,
                groups = groupsCount,
                expenses = expensesCount,
                migrations = await _db.Database.GetAppliedMigrationsAsync(ct)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "DB connection failed",
                error = ex.Message
            });
        }
    }
}