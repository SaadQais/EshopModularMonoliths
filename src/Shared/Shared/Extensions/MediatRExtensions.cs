using Microsoft.Extensions.Configuration;

namespace Shared.Extensions
{
    public static class MediatRExtensions
    {
        public static IServiceCollection AddMediatRWithAssemblies
            (this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(assemblies);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
                config.LicenseKey = configuration["MediatR:LicenseKey"];
            });

            services.AddValidatorsFromAssemblies(assemblies);

            return services;
        }
    }
}
