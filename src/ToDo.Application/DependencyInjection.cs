using Microsoft.Extensions.DependencyInjection;
using ToDo.Application.Services;

namespace ToDo.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Services
            services.AddScoped<ITaskItemService, TaskItemService>();

            return services;
        }
    }
}
