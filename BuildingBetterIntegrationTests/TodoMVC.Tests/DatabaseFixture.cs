using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NDbUnit.Core;
using NDbUnit.Core.SqlClient;
using NHibernate;
using NHibernate.Event;
using TodoMVC.Persistence.Interceptors;

namespace TodoMVC.Tests
{
    public class DatabaseFixture
    {
        private const string CoreAssemblyName = "TodoMVC";
        private static readonly Lazy<ISessionFactory> sessionFactory = new Lazy<ISessionFactory>(CreateSessionFactory);
        private static readonly Lazy<INDbUnitTest> dataController = new Lazy<INDbUnitTest>(CreateDataController);

        public ISessionFactory SessionFactory
        {
            get { return sessionFactory.Value; }
        }

        public void UseLatestDatabaseSchema()
        {
            var announcer = new BaseAnnouncer(message => Debug.WriteLineIf(Debugger.IsAttached, message));
            var runner = new RunnerContext(announcer)
            {
                Database = "SqlServer",
                Connection = ConfigurationManager.ConnectionStrings["Database"].ConnectionString,
                Target = Path.Combine(Environment.CurrentDirectory, CoreAssemblyName + ".dll")
            };
            new TaskExecutor(runner).Execute();
        }

        public void LoadData(string embeddedResourceName)
        {
            if (Assembly.GetExecutingAssembly().GetManifestResourceInfo(embeddedResourceName) == null) throw new ArgumentOutOfRangeException("embeddedResourceName", "Unable to find embedded resource with specified name.");
            dataController.Value.ReadXml(Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName));
            dataController.Value.PerformDbOperation(DbOperationFlag.CleanInsertIdentity);
        }

        public void DeleteData()
        {
            dataController.Value.PerformDbOperation(DbOperationFlag.DeleteAll);
        }

        private static ISessionFactory CreateSessionFactory()
        {
            // Ensure |DataDirectory| in the AttachDbFilename connection string parameter resolves to the current test execution location (i.e. bin\Debug)
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.CurrentDirectory);

            var coreAssembly = Assembly.Load(CoreAssemblyName);

            var configuration = Fluently.Configure()
                                        .Database(MsSqlConfiguration.MsSql2008
                                        .ConnectionString(c => c.FromConnectionStringWithKey("Database")))
                                        .ExposeConfiguration(c =>
                                        {
                                            var entityValidationInterceptor = new EntityValidationInterceptor();
                                            c.EventListeners.PreCollectionUpdateEventListeners = new IPreCollectionUpdateEventListener[] { entityValidationInterceptor };
                                            c.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { entityValidationInterceptor };
                                            c.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { entityValidationInterceptor };
                                        })
                                        .Mappings(m => m.FluentMappings.AddFromAssembly(coreAssembly))
                                        .BuildConfiguration();

            var result = configuration.BuildSessionFactory();
            return result;
        }

        private static INDbUnitTest CreateDataController()
        {
            var result = new SqlDbUnitTest(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TodoMVC.Tests.Database.xsd"))
            {
                result.ReadXmlSchema(stream);
            }
            return result;
        }
    }
}
