using BossAttacks.Modules.Generic;

namespace UnitTests
{
    [TestClass]
    public class TestLoggingUtils
    {
        [TestInitialize]
        public void Setup()
        {
            LoggingUtils.LoggingFunction = null;
            LoggingUtils.LogLevel = Modding.LogLevel.Info;
            LoggingUtils.TimeStringFunction = (_) => "test time";
            LoggingUtils.FilterFunction = (_) => true;
        }

        private class A { }

        [TestMethod]
        public void TestLogAsNormal()
        {
            var mock = new Mock<Action<string>>();
            LoggingUtils.LoggingFunction = mock.Object;

            var a = new A();
            a.LogMod("test message");
            mock.Verify(m => m("test time [I] [A] test message"), Times.Once());
        }

        [TestMethod]
        public void TestLogWithNull()
        {
            var mock = new Mock<Action<string>>();
            LoggingUtils.LoggingFunction = mock.Object;

            A? a = null;
            a.LogMod("test message");
            mock.Verify(m => m("test time [I] [A] test message"), Times.Once());
        }

        private static class StaticA { }

        [TestMethod]
        public void TestLogWithStaticClass()
        {
            var mock = new Mock<Action<string>>();
            LoggingUtils.LoggingFunction = mock.Object;

            typeof(StaticA).LogMod("test message");
            mock.Verify(m => m("test time [I] [StaticA] test message"), Times.Once());
        }

        [TestMethod]
        public void TestLogWithLogLevels()
        {
            var mock = new Mock<Action<string>>();
            LoggingUtils.LoggingFunction = mock.Object;
            LoggingUtils.LogLevel = Modding.LogLevel.Debug;

            var a = new A();
            a.LogMod("test message");
            a.LogModDebug("test message");
            a.LogModFine("test message");
            mock.Verify(m => m("test time [I] [A] test message"), Times.Once());
            mock.Verify(m => m("test time [D] [A] test message"), Times.Once());
            mock.Verify(m => m("test time [F] [A] test message"), Times.Never());
        }

        [TestMethod]
        public void TestLogDontRepeatWithin1s()
        {
            var mock = new Mock<Action<string>>();
            LoggingUtils.LoggingFunction = mock.Object;
            LoggingUtils.FilterFunction = LoggingUtils.DontRepeatWithin1s;
            LoggingUtils.TimeStringFunction = datetime => datetime.ToString("HH':'mm':'ss'.'fff");

            LoggingUtils.TimeFunction = () => DateTime.Parse("2023-01-21 00:00:00");
            typeof(TestLoggingUtils).LogMod("test message");
            typeof(TestLoggingUtils).LogMod("test message");
            typeof(TestLoggingUtils).LogMod("test message");
            LoggingUtils.TimeFunction = () => DateTime.Parse("2023-01-21 00:00:05");
            typeof(TestLoggingUtils).LogMod("test message");
            typeof(TestLoggingUtils).LogMod("test message");
            typeof(TestLoggingUtils).LogMod("test message");

            mock.Verify(m => m("00:00:00.000 [I] [TestLoggingUtils] test message"), Times.Once()); // 1st log
            mock.Verify(m => m("00:00:00.000 (hidden logs)"), Times.Once()); // 2nd log
            // 3rd log should not show
            // 5 second delay
            mock.Verify(m => m("00:00:05.000 [I] [TestLoggingUtils] test message"), Times.Once()); // 4th log
            mock.Verify(m => m("00:00:05.000 (hidden logs)"), Times.Once()); // 5th log
            // 6th log should not show
        }
    }
}
