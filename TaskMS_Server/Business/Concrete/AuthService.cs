using Business.Abstract;
using Business.Services;
using Business.Validations;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrete;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtService _jwtService;
    private readonly AppDbContext _context;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        JwtService jwtService,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _context = context;
    }

    public async Task<string> LoginAsync(LoginDto request)
    {
        User? appUser = await _userManager.FindByNameAsync(request.UserNameOrEmail);
        if (appUser is null)
        {
            appUser = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            if (appUser is null)
            {
                throw new Exception("Kullanıcı bulunamadı!");
            }
        }

        var result = await _signInManager.CheckPasswordSignInAsync(appUser, request.Password, true);

        if (result.IsLockedOut)
        {
            TimeSpan? timeSpan = appUser.LockoutEnd - DateTime.UtcNow;
            if (timeSpan is not null)
                throw new Exception($"Kullanıcı art arda 3 kere yanlış şifre girişinden dolayı sistem {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika kilitlenmiştir.");
        }

        if (result.IsNotAllowed)
        {
            throw new Exception("Mail adresiniz onaylı değil!");
        }

        if (!result.Succeeded)
        {
            throw new Exception("Şifreniz yanlış!");
        }

        var roles =
            _context.AppUserRoles
                .Where(p => p.UserId == appUser.Id)
                .Include(p => p.Role)
                .Select(s => s.Role!.Name)
                .ToList();

        string token = _jwtService.CreateToken(appUser, roles, request.RememberMe);
        return token;
    }

    
}
