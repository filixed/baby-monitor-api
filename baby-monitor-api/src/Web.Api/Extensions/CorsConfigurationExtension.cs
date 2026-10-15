using Web.Api.Configurations;

namespace Web.Api.Extensions;

public static class CorsConfigurationExtension
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        string[]? allowedOrigins = configuration
            .GetSection(CorsConstants.ConfigSectionKey)
            .Get<string[]>();

        services.AddCors(options => options.AddPolicy(CorsConstants.PolicyName, policy =>
            {
                if (allowedOrigins is { Length: > 0 })
                {
                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            }));

        return services;
    }
}
