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

        public static void LogModTEMP<T>(this T logger, string message)
        {
#if DEBUG
            DoLog(logger, "TEMP", message);
#endif
        }

        // These logs are accepted:
        // - Unexpected issue and NOT okay to continue.
        public static void LogModError<T>(this T logger, string message)
        {
            if (LogLevel <= Modding.LogLevel.Error)
            {
                DoLog(logger, "E", message);
                throw new ModException(message);
            }
        }

        // These logs are accepted:
        // - Unexpected issue but okay to continue.
        public static void LogModWarn<T>(this T logger, string message)
        {
            if (LogLevel <= Modding.LogLevel.Warn)
            {
                DoLog(logger, "W", message);
            }
        }

        // These logs are accepted:
        // - The only log for a *manually triggered*, *infrequent* event (a few in a minute; e.g. change scene; start boss fight).
        public static void LogMod<T>(this T logger, string message)
        {
#if DEBUG
            if (LogLevel <= Modding.LogLevel.Info)
            {
                DoLog(logger, "I", message);
            }
#endif
        }

        // These logs are accepted:
        // - The more detailed logs for a *manually triggered*, *infrequent* event (a few in a minute; e.g. change scene; start boss fight).
        // - The only log for a *manually triggered*, *frequent* event (once every second; e.g. deal damage; take hits; TK status change).
        public static void LogModDebug<T>(this T logger, string message)
        {
#if DEBUG
            if (LogLevel <= Modding.LogLevel.Debug)
            {
                DoLog(logger, "D", message);
            }
#endif
        }

        // These logs are accepted:
        // - The more detailed logs for a *manually triggered*, *frequent* event (once every second; e.g. deal damage; take hits; TK status change).
        // - Automatic events.
        public static void LogModFine<T>(this T logger, string message)
        {
#if DEBUG
            if (LogLevel <= Modding.LogLevel.Fine)
            {
                DoLog(logger, "F", message);
            }
#endif
        }

        private static void DoLog<T>(T logger, string flag, string message)
        {
            var time = DateTime.Now.ToString("HH':'mm':'ss'.'fff");
            var type = logger.GetType();
            var id = type.GetProperty("ID")?.GetValue(logger) as string ?? type.Name;
            
            BossAttacks.Instance.Log($"{time} [{flag}] [{id}] {message}");
        }
    }
}
