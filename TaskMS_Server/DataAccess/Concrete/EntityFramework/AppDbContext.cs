using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework;

public class AppDbContext : IdentityDbContext<User, Role, Guid>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<TaskEnt?> TaskEnts { get; set; }
    public DbSet<TaskFile> TaskFiles { get; set; }
    
    public DbSet<UserRole> AppUserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<TaskEnt>()
            .Property(t => t.Status)
            .HasConversion<int>();
        
        //Composite Key
        builder.Entity<UserRole>().HasKey(x => new { x.RoleId, x.UserId });

        builder.Ignore<IdentityUserRole<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();
    }
}