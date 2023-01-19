using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BossAttacks.Modules;

namespace BossAttacks.Utils
{
    public class ModException : Exception
    {
        public ModException(string message) : base(message)
        {
        }
    }

    /**
     * Debug build: Log [E] and throw.
     * Release build: Log [W] and return **true**.
     *   - Caller can use the return value to recover.
     *   - E.g. `if (ModAssert.DebugBuild()) { recover(); }`
     */
    public static class ModAssert
    {
        public static bool DebugBuild(bool condition, string message)
        {
            if (!condition)
            {
#if (DEBUG)
                typeof(ModAssert).LogModError(message);
#else
                typeof(ModAssert).LogModWarn(message);
#endif
            }
            return !condition;
        }

        /**
         * Debug build: Log [E] and throw.
         * Release build: Log [E] and throw.
         */
        public static void AllBuilds(bool condition, string message)
        {
            if (!condition)
            {
                typeof(ModAssert).LogModError(message);
            }
        }
    }
}
