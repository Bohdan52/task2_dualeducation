using task2.Dtos.Auth;

namespace task2.Services.Interfaces;

public interface IAuthService
{
	Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
	Task<AuthResponseDto> LoginAsync(LoginDto dto);
}