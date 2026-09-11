using Microsoft.Extensions.DependencyInjection;
using ToDo.Application.Services;
using ToDo.Application.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ToDo.Application.Common.Handlers;
using ToDo.Application.Common.Options;
using Microsoft.AspNetCore.Authentication;

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

    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services)
    {
        services.AddOptions<ApiOptions>()
            .BindConfiguration("Api");

        return services;
    }

    public static IServiceCollection AddApiKeyAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiKeyPolicy", policy =>
            {
                policy.AddAuthenticationSchemes(ApiKeyAuthenticationHandler.SchemeName);
                policy.RequireAuthenticatedUser();
            });
        });

        services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationHandler.SchemeName, null);

        return services;
    }
}
