using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Domain.Interfaces;
using ToDo.Infrastructure.Data;
using ToDo.Infrastructure.Repositories;
using ToDo.Infrastructure.Services;

namespace ToDo.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // database
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (environment == "Development")
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));
            }
            else
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite(connectionString));
            }

            // repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITaskItemRepository, TaskItemRepository>();

            // services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPasswordService, PasswordService>();

            return services;
        }
    }
}
