using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete;

public class AuthDal : IAuthDal
{
    private readonly AppDbContext _context;

    public AuthDal(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> FindUserByNameOrEmailAsync(string userNameOrEmail)
    {
        return await _context.Users
            .Where(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail)
            .FirstOrDefaultAsync();
    }

    public async Task<List<string?>> GetRolesByUserIdAsync(Guid userId)
    {
        return await _context.AppUserRoles
            .Where(p => p.UserId == userId)
            .Include(p => p.Role)
            .Select(s => s.Role!.Name)
            .ToListAsync();
    }
}
