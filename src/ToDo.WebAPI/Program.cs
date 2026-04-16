
using Serilog;
using ToDo.Application;
using ToDo.Infrastructure;
using ToDo.WebAPI.Middleware;

namespace ToDo.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);


            // Serilog
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            // Controllers
            builder.Services.AddControllers()
                .AddNewtonsoftJson();

            // OpenAPI
            builder.Services.AddOpenApi();

            // Application services
            builder.Services.AddApplication();

            // Infrastructure services
            builder.Services.AddInfrastructure(builder.Configuration);



            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();


            app.Run();
        }
    }
}
