using System.Diagnostics.CodeAnalysis;
using Core.Application.Interfaces.Repositories;
using Infra.Persistence.Bootstraps;
using Infra.Persistence.Interfaces;
using Infra.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Persistence.Ioc
{
    [ExcludeFromCodeCoverage]
    public static class ServiceRegistration
    {
        public static void ConfigureDatabaseBootstrap(this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            serviceProvider.GetService<IDatabaseBootstrap>()?.Setup();
        }

        public static void AddPersistenceLayer(this IServiceCollection services)
        {
            services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

            services.AddScoped(typeof(IGenericRepositoryAsync<,>), typeof(GenericRepositoryAsync<,>));
            services.AddScoped<ITodoRepositoryAsync, TodoRepositoryAsync>();
        }
    }
}