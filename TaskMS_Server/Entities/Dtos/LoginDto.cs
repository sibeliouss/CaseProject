namespace Entities.Dtos;

public record LoginDto(
    string UserNameOrEmail,
    string Password,
    bool RememberMe = false);