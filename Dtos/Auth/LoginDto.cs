using System.ComponentModel.DataAnnotations;

namespace task2.Dtos.Auth;

public class LoginDto
{
    [Required]
    public string UserNameOrEmail { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}