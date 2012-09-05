using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Bits.Persistence.Interceptors;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NDbUnit.Core;
using NDbUnit.Core.SqlClient;
using NHibernate;
using NHibernate.Event;

namespace Bits.IntegrationTests
{
    public class DatabaseFixture
    {
        public const string ConnectionStringKey = "Database";
        private const string CoreAssemblyName = "Bits.Core";
        private static readonly Lazy<ISessionFactory> sessionFactory = new Lazy<ISessionFactory>(() => CreateSessionFactory(DatabaseFixture.ConnectionStringKey, DatabaseFixture.CoreAssemblyName));
        private static readonly Lazy<INDbUnitTest> dataController = new Lazy<INDbUnitTest>(() => CreateDataController(DatabaseFixture.ConnectionStringKey));

        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[DatabaseFixture.ConnectionStringKey].ConnectionString;
            }
        }

        public virtual INDbUnitTest DataController
        {
            get
            {
                return dataController.Value;
            }
        }

        public virtual ISessionFactory SessionFactory
        {
            get
            {
                return sessionFactory.Value;
            }
        }

        static DatabaseFixture()
        {
            // Ensure |DataDirectory| in the AttachDbFilename parameter resolves to the current test running location (i.e. bin\Debug)
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.CurrentDirectory);
        }

        public virtual void LoadData(Type type)
        {
            LoadData(String.Format(CultureInfo.InvariantCulture, "{0}.{1}.xml", type.Namespace, type.Name));
        }

        public virtual void LoadData(string embeddedResourceName)
        {
            if (Assembly.GetExecutingAssembly().GetManifestResourceInfo(embeddedResourceName) == null)
                throw new ArgumentOutOfRangeException("embeddedResourceName", embeddedResourceName, "Unable to find embedded resource with specified name.");
            DataController.ReadXml(Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName));
            DataController.PerformDbOperation(DbOperationFlag.CleanInsertIdentity);
        }

        public virtual void RollbackDatabaseSchema(long version = 0)
        {
            var announcer = new MigrationAnnoucer(message => Debug.WriteLineIf(Debugger.IsAttached, message), message => Debug.WriteLineIf(Debugger.IsAttached, message)) { ShowSql = true };
            var runner = new RunnerContext(announcer)
            {
                Database = "SqlServer",
                Connection = ConnectionString,
                Target = Path.Combine(Environment.CurrentDirectory, CoreAssemblyName + ".dll"),
                Task = "migrate:down",
                Version = version,
                ApplicationContext = "LocalDb"
            };
            new TaskExecutor(runner).Execute();
        }

        public virtual void UseLatestDatabaseSchema()
        {
            var announcer = new MigrationAnnoucer(message => Debug.WriteLineIf(Debugger.IsAttached, message), message => Debug.WriteLineIf(Debugger.IsAttached, message)) { ShowSql = true };
            var runner = new RunnerContext(announcer)
            {
                Database = "SqlServer",
                Connection = ConnectionString,
                Target = Path.Combine(Environment.CurrentDirectory, CoreAssemblyName + ".dll"),
                ApplicationContext = "LocalDb"
            };
            new TaskExecutor(runner).Execute();
        }

        protected static ISessionFactory CreateSessionFactory(string connectionStringKey, string mappingAssembly)
        {
            var coreAssembly = Assembly.Load(mappingAssembly);

            var configuration = Fluently.Configure()
                                        .Database(MsSqlConfiguration.MsSql2008
                                                                    .ConnectionString(c => c.FromConnectionStringWithKey(connectionStringKey)))
                                        .ExposeConfiguration(c =>
                                        {
                                            c.SetProperty("generate_statistics", "true");
                                            var entityValidationInterceptor = new EntityValidationInterceptor();
                                            c.EventListeners.PreCollectionUpdateEventListeners = new IPreCollectionUpdateEventListener[] { entityValidationInterceptor };
                                            c.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { entityValidationInterceptor };
                                            c.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { entityValidationInterceptor };
                                        })
                                        .Mappings(m => m.FluentMappings
                                                        .AddFromAssembly(coreAssembly)
                                                        .ExportTo(Environment.CurrentDirectory))
                                        .BuildConfiguration();

            var result = configuration.BuildSessionFactory();

            NHibernateProfiler.Initialize();

            return result;
        }

        protected static INDbUnitTest CreateDataController(string connectionStringKey)
        {
            var result = new SqlDbUnitTest(ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bits.IntegrationTests.Database.xsd"))
            {
                result.ReadXmlSchema(stream);
            }
            return result;
        }

        private class MigrationAnnoucer : Announcer
        {
            private readonly Action<string> info;
            private readonly Action<string> error;

            public MigrationAnnoucer(Action<string> info, Action<string> error)
            {
                this.info = info;
                this.error = error;
            }

            public override void Error(string message)
            {
                error(message);
            }

            public override void Write(string message, bool escaped)
            {
                info(message);
            }
        }
    }
}