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
        public void TestMethod1()
        {

            var manualResetEvent = new ManualResetEvent(false);

            ServiceMock.Setup(x => x.SubmitFeed()).Callback(() => { manualResetEvent.Set(); }).Throws<Exception>();

            Task.Run(() => { Shop.Perfom(); });

            manualResetEvent.WaitOne(TimeSpan.FromSeconds(1));

            manualResetEvent.Reset();

            Task.Run(() => { Shop.Perfom(); });

            manualResetEvent.WaitOne(TimeSpan.FromSeconds(1));

            ServiceMock.Verify(x => x.SubmitFeed(), Times.Once);
        }
    }
}
