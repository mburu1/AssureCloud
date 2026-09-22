using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using AssureCloud.Messaging.Abstractions;
using AssureCloud.Messaging.Services;

namespace AssureCloud.Messaging;

public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MessagingOptions>(configuration.GetSection(MessagingOptions.SectionName));

        var options = configuration.GetSection(MessagingOptions.SectionName).Get<MessagingOptions>()
            ?? throw new InvalidOperationException("Messaging configuration not found.");

        services.AddSingleton(sp =>
        {
            var messagingOptions = sp.GetRequiredService<IOptions<MessagingOptions>>().Value;
            return new ServiceBusClient(messagingOptions.ConnectionString);
        });

        services.AddScoped<IMessagePublisher, ServiceBusMessagePublisher>();
        services.AddHostedService<ServiceBusMessageProcessor>();

        return services;
    }
}