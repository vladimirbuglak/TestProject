using ClassLibrary1;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace UnitTestProject1
{
    public class InMemorySessionFactoryProvider
    {
        private static InMemorySessionFactoryProvider instance;
        public static InMemorySessionFactoryProvider Instance
        {
            get { return instance ?? (instance = new InMemorySessionFactoryProvider()); }
        }

        private ISessionFactory sessionFactory;
        private Configuration configuration = new Configuration();

        private InMemorySessionFactoryProvider() { }

        public void Initialize()
        {
            sessionFactory = CreateSessionFactory();
        }

        private ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString("Server=.; initial catalog=AOAmazon; Integrated Security=SSPI; Application Name=AmazonWebHost; max pool size=5000"))
               .Mappings(x => x.FluentMappings.Add<FeedMessagesMapping>())
                .ExposeConfiguration(cfg => configuration = cfg)
                .BuildSessionFactory();
        }

        public ISession OpenSession()
        {
            ISession session = sessionFactory.OpenSession();

            return session;
        }

        public void Dispose()
        {
            if (sessionFactory != null)
                sessionFactory.Dispose();

            sessionFactory = null;
            configuration = null;
        }
    }
}