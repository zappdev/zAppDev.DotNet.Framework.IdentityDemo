using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using zAppDev.DotNet.Framework.Data;

namespace IdentityDemo.DAL
{
    public class DBSessionManager
    {
        public static ISessionFactory CreateSessionFactory(string connectionString)
        {
            return Fluently.Configure()
                .Database(
                        MsSqlConfiguration.MsSql7.ConnectionString(connectionString)
                )
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<DBSessionManager>();
                    m.HbmMappings.AddFromAssemblyOf<DBSessionManager>();
                    m.HbmMappings.AddFromAssemblyOf<zAppDev.DotNet.Framework.Auditing.AuditableEntity>();
                })
                .ExposeConfiguration(cfg =>
                {
                    var up = new SchemaUpdate(cfg);
                    UpdateDatabaseSchema(cfg, connectionString);
                })
                .BuildSessionFactory();
        }

        private static void UpdateDatabaseSchema(NHibernate.Cfg.Configuration cfg, string connectionString)
        {
            MiniSessionManager.ExecuteScript(connectionString, @"
                IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'wf') EXEC('CREATE SCHEMA wf AUTHORIZATION [dbo]');
                IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'security') EXEC('CREATE SCHEMA security AUTHORIZATION [dbo]');
				IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'audit') EXEC('CREATE SCHEMA audit AUTHORIZATION [dbo]');"
                ); 

            var updateCode = new System.Text.StringBuilder();
            var schemaUpdate = new SchemaUpdate(cfg);
            schemaUpdate.Execute(row =>
            {
                updateCode.AppendLine(row);
                updateCode.AppendLine();
            }, true);
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddHibernate(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionSource = configuration.GetConnectionString("DefaultConnection");
            // Singleton objects are the same for every object and every request.
            var factory = DBSessionManager.CreateSessionFactory(connectionSource);
            services.AddSingleton(provider => factory);
            // Scoped objects are the same within a request, but different across different requests.
            services.AddScoped((provider) =>
            {
                var factoryLocal = provider.GetService<ISessionFactory>();
                var session = factoryLocal.OpenSession();
                session.FlushMode = FlushMode.Manual;
                return session;
            });
            services.AddScoped<IMiniSessionService, MiniSessionService>();
            services.AddScoped<IRepository, Repository>();
        }
    }
}
