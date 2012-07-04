using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NLog;

namespace TodoMVC.Persistence
{
    public class SessionProvider
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        public static readonly ConnectionStringSettings ConnectionSettings = ConfigurationManager.ConnectionStrings["DefaultConnection"];
        private readonly ISessionFactory sessionFactory;

        /// <summary>
        /// Creates a new session factory used to create session instances
        /// </summary>
        /// <remarks>
        /// Provider should be instantiated once per application.
        /// </remarks>
        public SessionProvider()
        {
            var configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                              .ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection")))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildConfiguration();

            sessionFactory = configuration.BuildSessionFactory();
        }

        public ISession CreateInstance()
        {
            var session = sessionFactory.OpenSession();
            var connectionStringBuilder = new SqlConnectionStringBuilder(session.Connection.ConnectionString);
            log.Debug("Session opened to {0}", connectionStringBuilder.DataSource);
            return session;
        }

        public static void DisposeInstance(ISession session)
        {
            if (session == null || !session.IsOpen) return;

            try
            {
                if (session.Transaction != null && session.Transaction.IsActive)
                {
                    session.Transaction.Rollback();
                    var exception = new TransactionException("Rolling back uncommited NHibernate transaction.");
                    log.WarnException("Rolling back uncommited database transaction.", exception);
                    throw exception;
                }
                session.Flush();
            }
            finally
            {
                session.Close();
                session.Dispose();
                log.Debug("Session closed");
            }
        }
    }
}