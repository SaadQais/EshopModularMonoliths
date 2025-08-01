﻿namespace Shared.Messaging.Extensions
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitWithAssemblies(
            this IServiceCollection services,
            IConfiguration configuration,
            params Assembly[] assemblies)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                config.SetInMemorySagaRepositoryProvider();

                config.AddConsumers(assemblies);
                config.AddSagaStateMachines(assemblies);
                config.AddSagas(assemblies);
                config.AddActivities(assemblies);

                //config.UsingInMemory((context, configurator) =>
                //{
                //    configurator.ConfigureEndpoints(context);
                //});

                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(configuration["MessageBroker:UserName"]!);
                        host.Password(configuration["MessageBroker:Password"]!);
                    });
                    configurator.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
