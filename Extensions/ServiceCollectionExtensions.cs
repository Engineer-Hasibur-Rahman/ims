using ims.Application.Interfaces;
using ims.Application.Services;
using ims.Filters;
using ims.Infrastructure.Data;
using ims.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace ims.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IEmailService, ims.Application.Services.EmailService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<ims.Application.Validators.LoginRequestDtoValidator>();

            services.AddScoped<ValidationFilter>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
