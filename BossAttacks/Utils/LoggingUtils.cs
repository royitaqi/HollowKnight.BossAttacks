using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BossAttacks.Modules;
using Modding;
using UnityEngine;

namespace BossAttacks.Utils
{
    internal static class LoggingUtils
    {
        public static Modding.LogLevel LogLevel = Modding.LogLevel.Info;

        public static void LogModTEMP<T>(this T self, string message)
        {
#if DEBUG
            LogModImpl(self, "TEMP", message);
#endif
        }

        // These logs are accepted:
        // - Unexpected issue and NOT okay to continue.
        public static void LogModError<T>(this T self, string message)
        {
            if (LogLevel <= Modding.LogLevel.Error)
            {
                LogModImpl(self, "E", message);
                throw new ModException(message);
            }
        }

        // These logs are accepted:
        // - Unexpected issue but okay to continue.
        public static void LogModWarn<T>(this T self, string message)
        {
            if (LogLevel <= Modding.LogLevel.Warn)
            {
                LogModImpl(self, "W", message);
            }
        }

        // These logs are accepted:
        // - The only log for a *manually triggered*, *infrequent* event (a few in a minute; e.g. change scene; start boss fight).
        public static void LogMod<T>(this T self, string message)
        {
#if DEBUG
            if (LogLevel <= Modding.LogLevel.Info)
            {
                LogModImpl(self, "I", message);
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
                LogModImpl(self, "D", message);
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
                LogModImpl(self, "F", message);
            }
#endif
        }

        internal static void LogModImpl<T>(T self, string flag, string message)
        {
            var time = TimeStringFunction(TimeFunction());
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

            var content = $"[{flag}] [{id}] {message}";

            if (FilterFunction(content))
            {
                LoggingFunction($"{time} {content}");
                lastIsLogged = true;
            }
            else if (lastIsLogged) // avoids repeated "hidden logs" log
            {
                LoggingFunction($"{time} (hidden logs)");
                lastIsLogged = false;
            }
        }
        private static bool lastIsLogged = true;

        internal static bool NoFilters(string content)
        {
            return true;
        }

        internal static bool DontRepeatLast(string content)
        {
            var ret = content != lastContent;
            lastContent = content;
            return ret;
        }
        private static string lastContent;

        internal static bool DontRepeatWithin1s(string content)
        {
            var now = TimeFunction();
            var updateTime = () =>
            {
                cannotLogBefore[content] = now + TimeSpan.FromSeconds(1);
            };

            if (!cannotLogBefore.ContainsKey(content))
            {
                updateTime();
                return true;
            }
            if (now < cannotLogBefore[content])
            {
                updateTime();
                return false;
            }
            updateTime();
            return true;
        }
        private static Dictionary<string, DateTime> cannotLogBefore = new();

        internal static Func<DateTime> TimeFunction = () => DateTime.Now;
        internal static Func<DateTime, string> TimeStringFunction = datetime => datetime.ToString("HH':'mm':'ss'.'fff");
        internal static Func<string, bool> FilterFunction = NoFilters;
        internal static Action<string> LoggingFunction;


        public static void EnableDebugger()
        {
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
        }

        private static void ModHooks_HeroUpdateHook()
        {
            if (Input.GetKeyDown(KeyCode.LeftBracket)) // less logs
            {
                LogLevel = LogLevel switch
                {
                    LogLevel.Info => LogLevel.Info,
                    LogLevel.Debug => LogLevel.Info,
                    LogLevel.Fine => LogLevel.Debug,
                    _ => throw new ModException($"Should not have log level {LogLevel}"),
                };
                typeof(LoggingUtils).LogMod($"LogLevel = {LoggingUtils.LogLevel}");
            }
            else if (Input.GetKeyDown(KeyCode.RightBracket)) // more logs
            {
                LogLevel = LogLevel switch
                {
                    LogLevel.Info => LogLevel.Debug,
                    LogLevel.Debug => LogLevel.Fine,
                    LogLevel.Fine => LogLevel.Fine,
                    _ => throw new ModException($"Should not have log level {LogLevel}"),
                };
                typeof(LoggingUtils).LogMod($"LogLevel = {LoggingUtils.LogLevel}");
            }
            else if (Input.GetKeyDown(KeyCode.Minus)) // less filters
            {
                if (FilterFunction == NoFilters)
                {
                    FilterFunction = NoFilters;
                    typeof(LoggingUtils).LogMod("LogFilter = NoFilters");
                }
                else if (FilterFunction == DontRepeatLast)
                {
                    FilterFunction = NoFilters;
                    typeof(LoggingUtils).LogMod("LogFilter = NoFilters");
                }
                else if (FilterFunction == DontRepeatWithin1s)
                {
                    FilterFunction = DontRepeatLast;
                    typeof(LoggingUtils).LogMod("LogFilter = DontRepeatLast");
                }
                else
                {
                    typeof(LoggingUtils).LogMod("LogFilter = unknown");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Equals)) // more filters
            {
                if (FilterFunction == NoFilters)
                {
                    FilterFunction = DontRepeatLast;
                    typeof(LoggingUtils).LogMod("LogFilter = DontRepeatLast");
                }
                else if (FilterFunction == DontRepeatLast)
                {
                    FilterFunction = DontRepeatWithin1s;
                    typeof(LoggingUtils).LogMod("LogFilter = DontRepeatWithin1s");
                }
                else if (FilterFunction == DontRepeatWithin1s)
                {
                    FilterFunction = DontRepeatWithin1s;
                    typeof(LoggingUtils).LogMod("LogFilter = DontRepeatWithin1s");
                }
                else
                {
                    typeof(LoggingUtils).LogMod("LogFilter = unknown");
                }
            }
        }
    }
}
