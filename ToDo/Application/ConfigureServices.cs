using Microsoft.Extensions.DependencyInjection;
using ToDo.Application.Services;
using ToDo.Application.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ToDo.Application.Common.Handlers;

namespace ToDo.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IToDoTaskService, ToDoTaskService>();

        return services;
    }

    public static ConfigureHostBuilder AddSerilogLogging(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });

        return host;
    }

    public static IServiceCollection AddExceptionHandler(this IServiceCollection services, IWebHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }
        return services;
    }
}
