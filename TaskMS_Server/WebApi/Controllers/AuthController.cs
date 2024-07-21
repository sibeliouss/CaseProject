using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstract;

namespace WebApi.Controllers;

[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
    {
        return _authService.LoginAsync(request, cancellationToken);
    }
}