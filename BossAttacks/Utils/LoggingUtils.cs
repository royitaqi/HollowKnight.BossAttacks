using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BossAttacks.Modules;

namespace BossAttacks.Utils
{
    internal static class LoggingUtils
    {
        //public static Modding.LogLevel LogLevel = Modding.LogLevel.Info;
        public static Modding.LogLevel LogLevel = Modding.LogLevel.Fine;

        public static void LogModTEMP<T>(this T self, string message)
        {
#if DEBUG
            DoLog(self, "TEMP", message);
#endif
        }

        // These logs are accepted:
        // - Unexpected issue and NOT okay to continue.
        public static void LogModError<T>(this T self, string message)
        {
            if (LogLevel <= Modding.LogLevel.Error)
            {
                DoLog(self, "E", message);
                throw new ModException(message);
            }
        }

        // These logs are accepted:
        // - Unexpected issue but okay to continue.
        public static void LogModWarn<T>(this T self, string message)
        {
            if (LogLevel <= Modding.LogLevel.Warn)
            {
                DoLog(self, "W", message);
            }
        }

        // These logs are accepted:
        // - The only log for a *manually triggered*, *infrequent* event (a few in a minute; e.g. change scene; start boss fight).
        public static void LogMod<T>(this T self, string message)
        {
#if DEBUG
            if (LogLevel <= Modding.LogLevel.Info)
            {
                DoLog(self, "I", message);
            }
#endif
        }

        // These logs are accepted:
        // - The more detailed logs for a *manually triggered*, *infrequent* event (a few in a minute; e.g. change scene; start boss fight).
        // - The only log for a *manually triggered*, *frequent* event (once every second; e.g. deal damage; take hits; TK status change).
        public static void LogModDebug<T>(this T self, string message)
        {
#if DEBUG
            if (LogLevel <= Modding.LogLevel.Debug)
            {
                DoLog(self, "D", message);
            }
#endif
        }

        // These logs are accepted:
        // - The more detailed logs for a *manually triggered*, *frequent* event (once every second; e.g. deal damage; take hits; TK status change).
        // - Automatic events.
        public static void LogModFine<T>(this T self, string message)
        {
#if DEBUG
            if (LogLevel <= Modding.LogLevel.Fine)
            {
                DoLog(self, "F", message);
            }
#endif
        }

        internal static void DoLog<T>(T self, string flag, string message)
        {
            var time = TimeFunction();
            string id;

            if (self == null)
            {
                id = typeof(T).Name;
            }
            else if (typeof(T) == typeof(Type))
            {
                id = (self as Type).Name;
            }
            else
            {
                var type = self.GetType();
                id = type.GetProperty("ID")?.GetValue(self) as string ?? type.Name;
            }

            LoggingFunction($"{time} [{flag}] [{id}] {message}");
        }

        internal static Action<string> LoggingFunction;
        internal static Func<string> TimeFunction = () => DateTime.Now.ToString("HH':'mm':'ss'.'fff");
    }
}
