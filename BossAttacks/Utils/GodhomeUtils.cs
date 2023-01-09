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
        public static readonly Dictionary<string, Boss> SceneToBoss = new()
        {
            { "GG_Broken_Vessel"     , null },
            { "GG_Brooding_Mawlek"   , null },
            { "GG_Brooding_Mawlek_V" , null },
            { "GG_Collector"         , null },
            { "GG_Collector_V"       , null },
            { "GG_Crystal_Guardian"  , null },
            { "GG_Crystal_Guardian_2", null },
            { "GG_Dung_Defender"     , null },
            { "GG_Failed_Champion"   , null },
            { "GG_False_Knight"      , null },
            { "GG_Flukemarm"         , null },
            { "GG_Ghost_Galien"      , null },
            { "GG_Ghost_Gorb"        , null },
            { "GG_Ghost_Gorb_V"      , null },
            { "GG_Ghost_Hu"          , null },
            { "GG_Ghost_Markoth"     , null },
            { "GG_Ghost_Markoth_V"   , null },
            { "GG_Ghost_Marmu"       , null },
            { "GG_Ghost_Marmu_V"     , null },
            { "GG_Ghost_No_Eyes"     , null },
            { "GG_Ghost_No_Eyes_V"   , null },
            { "GG_Ghost_Xero"        , null },
            { "GG_Ghost_Xero_V"      , null },
            { "GG_God_Tamer"         , null },
            { "GG_Grey_Prince_Zote"  , null },
            { "GG_Grimm"             , null },
            { "GG_Grimm_Nightmare"   , null },
            { "GG_Gruz_Mother"       , null },
            { "GG_Gruz_Mother_V"     , null },
            { "GG_Hive_Knight"       , null },
            { "GG_Hollow_Knight"     , new() { GoName = "HK Prime", FsmName = "Control", StateNames = new[] { "Choice P1", "Choice P2", "Choice P3" } } }, // all = good
            { "GG_Hornet_1"          , null },
            { "GG_Hornet_2"          , null },
            { "GG_Lost_Kin"          , null },
            { "GG_Mage_Knight"       , null },
            { "GG_Mage_Knight_V"     , null },
            { "GG_Mantis_Lords"      , null },
            { "GG_Mantis_Lords_V"    , null },
            { "GG_Mega_Moss_Charger" , null },
            { "GG_Nailmasters"       , null },
            { "GG_Nosk"              , null },
            { "GG_Nosk_Hornet"       , null },
            { "GG_Oblobbles"         , null },
            { "GG_Painter"           , null },
            { "GG_Radiance"          , null },
            { "GG_Sly"               , null },
            { "GG_Soul_Master"       , null },
            { "GG_Soul_Tyrant"       , new() { GoName = "Dream Mage Lord", FsmName = "Mage Lord" } }, // all = good
            { "GG_Traitor_Lord"      , null },
            { "GG_Uumuu"             , null },
            { "GG_Uumuu_V"           , null },
            { "GG_Vengefly"          , null },
            { "GG_Vengefly_V"        , null },
            { "GG_Watcher_Knights"   , null },
            { "GG_White_Defender"    , new() { GoName = "White Defender", FsmName = "Dung Defender" } }, // ground slam = stall
        };
    }
}
