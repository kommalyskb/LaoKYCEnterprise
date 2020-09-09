using CouchDBService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portal.Repositories;
using Shared.Configs;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal
{
    public static class StartupHelper
    {
        public static void AddCouchDBConfigService(this IServiceCollection services, IConfiguration configuration)
        {
            // Inject configuration 
            var res = GetCouchDBConfiguration(configuration);
            services.AddSingleton(res);
        }
        public static DBConfig GetCouchDBConfiguration(IConfiguration configuration)
        {
            var envConfig = configuration.GetSection(nameof(DBConfig)).Get<DBConfig>();

            // Cannot load configuration - use default configuration values
            if (envConfig == null)
            {
                return new DBConfig();
            }

            return envConfig;
        }

        public static void AddCouchDBContextService(this IServiceCollection services)
        {
            // Inject Dependency
            services.AddSingleton<ICouchContext, CouchContext>();
        }

        public static void AddAPILaoKYCService(this IServiceCollection services)
        {
            // Inject Dependency
            services.AddSingleton<IAPILaoKYC, APILaoKYC>();
        }

        public static void AddMyAppService(this IServiceCollection services)
        {
            // Inject Dependency
            services.AddSingleton<IMyAppClient, MyAppClient>();
        }
        public static void AddApiResourceService(this IServiceCollection services)
        {
            // Inject Dependency
            services.AddSingleton<IMyAPIResource, MyAPIResource>();
        }
    }
}
