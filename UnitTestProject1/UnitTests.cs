using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary1;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace UnitTestProject1
{
    [TestFixture]
    public class UnitTests
    {
        [SetUp]
        public void Setup()
        {
            InMemorySessionFactoryProvider.Instance.Initialize();
        }

        [Test]
        public async Task TestTeardown()
        {

            var session = InMemorySessionFactoryProvider.Instance.OpenSession();

            var first = session.Get<FeedMessage>(1);
            
            Task.Run(() =>
            {
                var first = session.Get<FeedMessage>(1);

            });

            Task.Run(() =>
            {
                var second = session.Get<FeedMessage>(1);

            });

            first.FeedType = "New";

        }
    }
}
