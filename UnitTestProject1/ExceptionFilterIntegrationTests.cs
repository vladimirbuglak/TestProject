using System;
using System.Threading;
using System.Threading.Tasks;
using AO.Clients.Amazon.Domain.Feed;
using NUnit.Framework;

namespace AO.Clients.Amazon.Tests.BusIntegrationTests
{
    public class ExceptionFilterIntegrationTests : FilterIntergarationTests
    {
        [Test]
        public void ExceptionTest()
        {
            ManualResetEvent autoResetEvent1 = new ManualResetEvent(false);
            ManualResetEvent autoResetEvent2 = new ManualResetEvent(false);
            ManualResetEvent autoResetEvent3 = new ManualResetEvent(false);
            ManualResetEvent autoResetEvent4 = new ManualResetEvent(false);

            Task.Run(() =>
            {
                var row = session.Get<FeedState.FeedMessage>(1);
                autoResetEvent1.Set();
                Console.WriteLine("-- After event1 --- ");
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("-- After event1 (2)  --- ");
                        row.ResultDescription = "New Description";
                        Console.WriteLine("-- After event1 (3) --- ");
                        session.Update(row);
                        Console.WriteLine("-- After event1 (4) --- ");
                        transaction.Commit();
                        autoResetEvent1.Set();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            });

            Console.WriteLine("autoResetEvent1.WaitOne");
            Assert.True(autoResetEvent1.WaitOne(TimeSpan.FromSeconds(1)));

            Task.Run(() =>
            {
                var row = session.Get<FeedState.FeedMessage>(1);
                autoResetEvent2.Set();
                Console.WriteLine("-- After event2 --- ");
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("-- After event2 (2) --- ");
                        row.ResultDescription = "New Description";
                        Console.WriteLine("-- After event2 (3) --- ");
                        session.Update(row);
                        Console.WriteLine("-- After event2 (4) --- ");
                        transaction.Commit();
                        Console.WriteLine("-- After event2 (5) --- ");
                        autoResetEvent2.Set();
                        Console.WriteLine("-- After event2 (6) --- ");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("-- After event2 (7) --- ");
                        transaction.Rollback();
                    }
                }
            });

            Console.WriteLine("autoResetEvent2.WaitOne");
            Assert.True(autoResetEvent2.WaitOne(TimeSpan.FromSeconds(1)));

            autoResetEvent1.Reset();

            Assert.True(autoResetEvent1.WaitOne(TimeSpan.FromSeconds(1)));

            autoResetEvent2.Reset();

            Assert.True(autoResetEvent2.WaitOne(TimeSpan.FromSeconds(1)));

        }
    }
}
