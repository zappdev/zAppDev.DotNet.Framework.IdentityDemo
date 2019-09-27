using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
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
}
