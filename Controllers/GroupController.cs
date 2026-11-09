using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task2.Services.Interfaces;

namespace task2.Controllers;

[Authorize] // Тепер для доступу потрібен токен
[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] string name, [FromQuery] string currency)
    {
        // Отримуємо Guid користувача з JWT-токена
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var group = await _groupService.CreateGroupAsync(userId, name, currency);
        return Ok(group);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyGroups()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var groups = await _groupService.GetUserGroupsAsync(userId);
        return Ok(groups);
    }
}