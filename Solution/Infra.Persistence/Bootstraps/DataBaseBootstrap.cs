using System.Diagnostics.CodeAnalysis;
using Infra.Persistence.Configs;
using Infra.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infra.Persistence.Bootstraps
{
    [ExcludeFromCodeCoverage]
    public class DatabaseBootstrap : ConnectionConfig, IDatabaseBootstrap
    {
        public DatabaseBootstrap(IConfiguration configuration)
            : base(configuration) { }

        public void Setup()
        {
            TodoBootstrap.Setup(_connection);
        }
    }
}