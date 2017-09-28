using System;
using AO.Clients.Amazon.Tests.DataBase;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace AO.Clients.Amazon.Tests.BusIntegrationTests
{
    public class BaseFilterIntergarationTests
    {
        private static Configuration Configuration;
        protected static ISessionFactory SessionFactory;
        protected ISession session;

        protected BaseFilterIntergarationTests()
        {
            if (Configuration == null)
            {
                Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString("Server=.; initial catalog=AOAmazon; Integrated Security=SSPI; Application Name=AmazonWebHost; max pool size=5000")
                        .ShowSql())
                    .ExposeConfiguration(cfg => Configuration = cfg)
                    .BuildConfiguration();

                SessionFactory = SessionFactoryProvider.CreateSessionFactory(Configuration);
            }

            session = SessionFactory.OpenSession(new SqlLiteSessionInterceptor());

        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
