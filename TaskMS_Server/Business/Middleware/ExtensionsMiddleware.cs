using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Middleware;

public static class ExtensionsMiddleware
{
    public static void AutoMigration(WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var context = scoped.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }

    public static void CreateFirstUser(WebApplication app)
    {
        using var scoped = app.Services.CreateScope(); 
        var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<User>>();
        if (!userManager.Users.Any())
        {
            userManager.CreateAsync(new()
            {
                Email = "sibel@admin.com",
                UserName = "sibel",
                FirstName = "Sibel",
                LastName = "Öztürk",
                EmailConfirmed = true,
            }, "Passw0rd*").Wait();
        }
    }

    public static void CreateUsers(WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<User>>();

        var userInfos =
            new List<(string email, string userName, string firstName, string lastName, string password)>
            {
                ("zeynepb@gmail.com", "zeynepb", "Zeynep", "Başaran", "Passw0rd*"),
                ("jaleoz@gmail.com", "jaleoz", "Jale", "Öztürk", "Passw0rd*"),
                ("sozturk@gmail.com", "sozturk", "Sezer", "Öztürk", "Passw0rd*")
                
            };

        foreach (var userInfo in userInfos)
        {
            var user = new User
            {
                Email = userInfo.email,
                UserName = userInfo.userName,
                FirstName = userInfo.firstName,
                LastName = userInfo.lastName,
                EmailConfirmed = true,
            };

            userManager.CreateAsync(user, userInfo.password).Wait();
        }
    }


    public static void CreateRoles(WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        if (!roleManager.Roles.Any())
        {
            roleManager.CreateAsync(new Role()
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
            }).Wait();
        }
    }

    public static void CreateUserRole(WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var context = scoped.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<Role>>();


        User? user = userManager.Users.FirstOrDefault(p => p.Email == "sibel@admin.com");
        if (user is not null)
        {
            Role? role = roleManager.Roles.FirstOrDefault(p => p.Name == "Admin");
            if (role is not null)
            {
                bool userRoleExist = context.AppUserRoles.Any(p => p.RoleId == role.Id && p.UserId == user.Id);
                if (!userRoleExist)
                {
                    UserRole appUserRole = new()
                    {
                        RoleId = role.Id,
                        UserId = user.Id,
                    };

                    context.AppUserRoles.Add(appUserRole);
                    context.SaveChanges();
                }
            }
        }
    } 
}