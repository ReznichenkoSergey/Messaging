using MassTransit;
using MassTransit.MultiBus;
using Messaging.Infrastructure.Configurations;
using Messaging.Infrastructure.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Messaging
{
    public static class ServiceExtentions
    {

        public static IServiceCollection AddRabbitServiseBus(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<RabbitMQConfig>(config.GetSection("RabbitMQConfig"));

            services.AddMassTransit<IFileCollector>(x =>
            {
                x.AddConsumer<FileCollectorConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = context.GetRequiredService<IOptions<RabbitMQConfig>>().Value;
                    cfg.Host(options.Url, rmhc =>
                    {
                        rmhc.Username(options.User);
                        rmhc.Password(options.Password);
                    });
                    cfg.ReceiveEndpoint("fileTransmit", e =>
                    {
                        e.ConfigureConsumer<FileCollectorConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();
            return services;
        }

    }
}
