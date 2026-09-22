using Microsoft.Extensions.DependencyInjection;

namespace AssureCloud.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        // Controllers are registered in Program.cs
        // This method can be used for API-specific services
        return services;
    }
}