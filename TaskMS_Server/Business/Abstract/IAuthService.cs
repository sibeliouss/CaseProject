using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Business.Abstract;

public interface IAuthService
{
    Task<IActionResult> LoginAsync(LoginDto request, CancellationToken cancellationToken);
}