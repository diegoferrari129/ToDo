using Microsoft.Extensions.DependencyInjection;
using ToDo.Application.Services;
using ToDo.Domain.Interfaces;

namespace ToDo.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Services
            services.AddScoped<ITaskItemService, TaskItemService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
