using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using server.Application.Common.Settings;
using server.Application.Interfaces;
using server.Application.Services;

namespace server.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProfileService, ProfileService>();
        return services;
    }
}
