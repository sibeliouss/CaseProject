using Entities.Concrete;

namespace DataAccess.Abstract;

public interface IAuthDal
{
    Task<User?> FindUserByNameOrEmailAsync(string userNameOrEmail);
    Task<List<string?>> GetRolesByUserIdAsync(Guid userId);
}