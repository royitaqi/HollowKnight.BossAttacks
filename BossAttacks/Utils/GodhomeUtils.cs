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
            public string[] StateNames;
        }
        public static Dictionary<string, Boss> SceneToBoss = new()
        {
            { "GG_Vengefly_V"        , null },
            { "GG_Gruz_Mother_V"     , null },
            { "GG_False_Knight"      , null },
            { "GG_Mega_Moss_Charger" , null },
            { "GG_Hornet_1"          , null },
            { "GG_Ghost_Gorb_V"      , null },
            { "GG_Dung_Defender"     , null },
            { "GG_Mage_Knight_V"     , null },
            { "GG_Brooding_Mawlek_V" , null },
            { "GG_Nailmasters"       , null },
            { "GG_Ghost_Xero_V"      , null },
            { "GG_Crystal_Guardian"  , null },
            { "GG_Soul_Master"       , null },
            { "GG_Oblobbles"         , null },
            { "GG_Mantis_Lords_V"    , null },
            { "GG_Ghost_Marmu_V"     , null },
            { "GG_Flukemarm"         , null },
            { "GG_Broken_Vessel"     , null },
            { "GG_Ghost_Galien"      , null },
            { "GG_Painter"           , null },
            { "GG_Hive_Knight"       , null },
            { "GG_Ghost_Hu"          , null },
            { "GG_Collector_V"       , null },
            { "GG_God_Tamer"         , null },
            { "GG_Grimm"             , null },
            { "GG_Watcher_Knights"   , null },
            { "GG_Uumuu_V"           , null },
            { "GG_Nosk_Hornet"       , null },
            { "GG_Sly"               , null },
            { "GG_Hornet_2"          , null },
            { "GG_Crystal_Guardian_2", null },
            { "GG_Lost_Kin"          , null },
            { "GG_Ghost_No_Eyes_V"   , null },
            { "GG_Traitor_Lord"      , null },
            { "GG_White_Defender"    , new() { GoName = "White Defender", FsmName = "Dung Defender" } }, // ground slam = stall
            { "GG_Soul_Tyrant"       , new() { GoName = "Dream Mage Lord", FsmName = "Mage Lord" } }, // all = good
            { "GG_Ghost_Markoth_V"   , null },
            { "GG_Grey_Prince_Zote"  , null },
            { "GG_Failed_Champion"   , null },
            { "GG_Grimm_Nightmare"   , null },
            { "GG_Hollow_Knight"     , new() { GoName = "HK Prime", FsmName = "Control", StateNames = new[] { "Choice P1", "Choice P2", "Choice P3" } } }, // all = good
            { "GG_Radiance"          , null },
        };
    }
}
