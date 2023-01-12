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
            var time = DateTime.Now.ToString("HH':'mm':'ss'.'fff");
            BossAttacks.Instance.Log($"{time} [TEMP] [{logger?.GetType()?.Name}] {message}");
#endif
        }

        // These logs are accepted:
        // - Unexpected issue and NOT okay to continue.
        public static void LogModError<T>(this T logger, string message)
        {
            if (LogLevel <= Modding.LogLevel.Error)
            {
                var time = DateTime.Now.ToString("HH':'mm':'ss'.'fff");
                BossAttacks.Instance.LogError($"{time} [E] [{logger?.GetType()?.Name}] {message}");
                throw new ModException(message);
            }
        }

        // These logs are accepted:
        // - Unexpected issue but okay to continue.
        public static void LogModWarn<T>(this T logger, string message)
        {
            if (LogLevel <= Modding.LogLevel.Warn)
            {
                var time = DateTime.Now.ToString("HH':'mm':'ss'.'fff");
                BossAttacks.Instance.LogWarn($"{time} [W] [{logger?.GetType()?.Name}] {message}");
            }
        }

        // These logs are accepted:
        // - The only log for a *manually triggered*, *infrequent* event (a few in a minute; e.g. change scene; start boss fight).
        public static void LogMod<T>(this T logger, string message)
        {
#if DEBUG
            if (LogLevel <= Modding.LogLevel.Info)
            {
                var time = DateTime.Now.ToString("HH':'mm':'ss'.'fff");
                BossAttacks.Instance.Log($"{time} [I] [{logger?.GetType()?.Name}] {message}");
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
                var time = DateTime.Now.ToString("HH':'mm':'ss'.'fff");
                BossAttacks.Instance.Log($"{time} [D] [{logger?.GetType()?.Name}] {message}");
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
                var time = DateTime.Now.ToString("HH':'mm':'ss'.'fff");
                BossAttacks.Instance.Log($"{time} [F] [{logger?.GetType()?.Name}] {message}");
            }
#endif
        }
    }
}
