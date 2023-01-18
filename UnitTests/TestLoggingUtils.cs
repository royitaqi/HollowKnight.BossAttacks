using BossAttacks.Modules.Generic;

namespace UnitTests
{
    [TestClass]
    public class TestLoggingUtils
    {
        private class A { }

        [TestMethod]
        public void TestLogAsNormal()
        {
            var mock = new Mock<Action<string>>();
            LoggingUtils.LoggingFunction = mock.Object;
            LoggingUtils.TimeFunction = () => "test time";

            var a = new A();
            a.LogModDebug("test message");
            mock.Verify(m => m("test time [D] [A] test message"), Times.Once());
        }

        [TestMethod]
        public void TestLogWithNull()
        {
            var mock = new Mock<Action<string>>();
            LoggingUtils.LoggingFunction = mock.Object;
            LoggingUtils.TimeFunction = () => "test time";

            A? a = null;
            a.LogModDebug("test message");
            mock.Verify(m => m("test time [D] [A] test message"), Times.Once());
        }

        private static class StaticA { }

        [TestMethod]
        public void TestLogWithStaticClass()
        {
            var mock = new Mock<Action<string>>();
            LoggingUtils.LoggingFunction = mock.Object;
            LoggingUtils.TimeFunction = () => "test time";

            typeof(StaticA).LogModDebug("test message");
            mock.Verify(m => m("test time [D] [StaticA] test message"), Times.Once());
        }
    }
}
