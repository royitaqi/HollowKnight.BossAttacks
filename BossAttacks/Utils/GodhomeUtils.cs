using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BossAttacks.Modules;

namespace BossAttacks.Utils
{
    static class GodhomeUtils
    {
        public class Boss
        {
            public string GoName;
            public string FsmName;
        }
        public static Dictionary<string, Boss> SceneToBoss = new()
        {
            { "GG_White_Defender", new() { GoName = "White Defender", FsmName = "Dung Defender" } },
        };
    }
}
