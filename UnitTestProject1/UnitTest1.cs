using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;


namespace UnitTestProject1
{
    [TestFixture]
    public class UnitTest1
    {
        private Mock<IService> ServiceMock { get; set; }
        
        private Shop Shop { get; set; }

        [SetUp]
        public void Setup()
        {
            ServiceMock = new Mock<IService>();
            Shop = new Shop(ServiceMock.Object);
        }

        [Test]
        public async Task TestMethod1()
        {

            var manualResetEvent = new ManualResetEvent(false);
            var manualResetEvent1 = new ManualResetEvent(false);

            ServiceMock.Setup(x => x.SubmitFeed()).Callback(() => { manualResetEvent.Set(); }).Throws<Exception>();


            Task.Run(() => { Shop.Perfom(1); });

            manualResetEvent.WaitOne(TimeSpan.FromSeconds(1));

            ServiceMock.Setup(x => x.SubmitFeed()).Callback(() => { manualResetEvent1.Set(); }).Throws<Exception>();

            Task.Run(() => { Shop.Perfom(2); });

            manualResetEvent.WaitOne(TimeSpan.FromSeconds(5));

            ServiceMock.Verify(x => x.SubmitFeed(), Times.Once);
        }
    }
}
