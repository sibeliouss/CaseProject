namespace Entities.Concrete;

public class UserRole
{
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
    public Guid UserId {  get; set; } 
}