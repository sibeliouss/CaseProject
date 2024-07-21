using System.Text;
using Business.Abstract;
using Business.Concrete;
using Business.Services;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Business.DependencyResolvers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<JwtService>(); 
     
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthDal, AuthDal>();
       
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITaskDal, EfTaskDal>();
        
        // giriş kontrol sisteminin aktif edilmesi için
        services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true, //tokeni gönderen kişi bilgisi
                ValidateAudience = true, // tokeni kullanacak site ya da kişi bilgisi
                ValidateIssuerSigningKey = true, //güvenlik anahtarı üretmesini sağlayan güvenlik sözcüğü
                ValidateLifetime = true, // tokenin yaşam süresini kontrol edilmesi
                ValidIssuer = "Sibel Öztürk",
                ValidAudience = "Task Project",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("StrongAndSecretKeyStrongAndSecretKeyStrongAndSecretKeyStrongAndSecretKey"))
            };
        });
        
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("TaskProject")));
        
        services.AddIdentity<User, Role>(opt =>
        {
            opt.Password.RequiredLength = 6;
            opt.SignIn.RequireConfirmedEmail = true;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(8);
            opt.Lockout.MaxFailedAccessAttempts = 2;
            opt.Lockout.AllowedForNewUsers = true;

        }).AddEntityFrameworkStores<AppDbContext>();
        
        return services;
        
    }
    
}