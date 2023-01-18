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

    public static class ModAssert
    {
        public static void DebugBuild(bool condition, string message)
        {
#if (DEBUG)
            if (!condition)
            {
                typeof(ModAssert).LogModError(message);
            }
#endif
        }

        public static void AllBuilds(bool condition, string message)
        {
            if (!condition)
            {
                typeof(ModAssert).LogModError(message);
            }
        }
    }
}
