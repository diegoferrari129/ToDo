using Microsoft.Extensions.DependencyInjection;
using ToDo.Application.Services.Auth;
using ToDo.Application.Services.TaskItems;
using ToDo.Application.Services.Users;

namespace ToDo.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITaskItemService, TaskItemService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
