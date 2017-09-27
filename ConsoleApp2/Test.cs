using NUnit.Framework;

namespace ConsoleApp2
{
    [TestFixture]
    class Test
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(0, 0);
        }
    }
}
