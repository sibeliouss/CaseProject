using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Business.Abstract;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto request);
}