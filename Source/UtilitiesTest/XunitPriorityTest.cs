using Utilities.General.XunitPriorityAttributes;
using Xunit;

namespace UtilitiesTest
{
    [TestCaseOrderer("Utilities.General.XunitPriorityAttributes.PriorityOrderer", "Utilities.General")]
    public class XunitPriorityTest
    {
        private static bool _test1Called;
        private static bool _test2ACalled;
        private static bool _test2BCalled;
        private static bool _test3Called;

        [Fact, TestPriority(5)]
        public void Test3()
        {
            _test3Called = true;

            Assert.True(_test1Called);
            Assert.True(_test2ACalled);
            Assert.True(_test2BCalled);
        }

        [Fact, TestPriority(0)]
        public void Test2B()
        {
            _test2BCalled = true;

            Assert.True(_test1Called);
            Assert.True(_test2ACalled);
            Assert.False(_test3Called);
        }

        [Fact, TestPriority(-3)]
        public void Test2A()
        {
            _test2ACalled = true;

            Assert.True(_test1Called);
            Assert.False(_test2BCalled);
            Assert.False(_test3Called);
        }

        [Fact, TestPriority(-5)]
        public void Test1()
        {
            _test1Called = true;

            Assert.False(_test2ACalled);
            Assert.False(_test2BCalled);
            Assert.False(_test3Called);
        }
    }
}