using System;
using System.Threading;
using System.Threading.Tasks;
using AO.Clients.Amazon.Domain.Feed;
using MassTransit;
using NUnit.Framework;

namespace AO.Clients.Amazon.Tests.BusIntegrationTests
{
    public class ExceptionFilterIntegrationTests : FilterIntergarationTests
    {
        [Test]
        public async Task WhenConsumerThrownStaleObjectStateExceptionExceptionThrown_ThenExecutionShouldBeContinued()
        {
            var manualEvent = new ManualResetEvent(false);
            var firstHandlerManualEvent = new ManualResetEvent(false);
            var secondHandlerManualEvent = new ManualResetEvent(false);

            var feedMessageId = 1;
            var execAfterException = false;
            var defaultTimeout = TimeSpan.FromSeconds(10);

            _bus.ConnectHandler<CustomMessage>(x =>
            {
                if (!x.Message.RunFirstHandler) return Task.FromResult(true);

                Assert.True(secondHandlerManualEvent.WaitOne(defaultTimeout));

                var localSession = GetSession();
                var message = localSession.Get<FeedState.FeedMessage>(feedMessageId);

                using (var transaction = localSession.BeginTransaction())
                {
                    message.ResultDescription = Guid.NewGuid().ToString();
                    localSession.Update(message);
                    transaction.Commit();

                    firstHandlerManualEvent.Set();
                };

                x.Message.RunFirstHandler = false;
                return Task.FromResult(true);
            });

            _bus.ConnectHandler<CustomMessage>(x =>
            {
                var localSession = GetSession();
                var message = localSession.Get<FeedState.FeedMessage>(feedMessageId);

                secondHandlerManualEvent.Set();
                Assert.True(firstHandlerManualEvent.WaitOne(defaultTimeout));

                using (var transaction = localSession.BeginTransaction())
                {
                    message.ResultDescription = Guid.NewGuid().ToString();
                    localSession.Update(message);
                    transaction.Commit();
                };

                execAfterException = true;

                manualEvent.Set();

                return Task.FromResult(true);
            });

            await _bus.Publish(new CustomMessage());

            Assert.True(manualEvent.WaitOne(defaultTimeout));
            Assert.True(execAfterException);
        }
    }

    public class CustomMessage
    {
        public bool RunFirstHandler { get; set; }
        public CustomMessage()
        {
            RunFirstHandler = true;
        }
    }
}
