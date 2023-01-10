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
            { "GG_Broken_Vessel"     , new() { GoName = "Infected Knight", FsmName = "IK Control" } }, // Attack Choice V3 // all = good
            { "GG_Brooding_Mawlek"   , new() { GoName = "Battle Scene/Mawlek Body", FsmName = "Mawlek Control" } }, // X no random
            { "GG_Brooding_Mawlek_V" , new() { GoName = "Battle Scene/Mawlek Body", FsmName = "Mawlek Control" } }, // X no random
            { "GG_Collector"         , new() { GoName = "Battle Scene/Jar Collector", FsmName = "Control" } }, // Move Choice V2 // !!! cannot control jars
            { "GG_Collector_V"       , new() { GoName = "Battle Scene/Jar Collector", FsmName = "Control" } }, // Move Choice V2 // !!! cannot control jars
            { "GG_Crystal_Guardian"  , new() { GoName = "Mega Zombie Beam Miner (1)", FsmName = "Beam Miner" } }, // Choice V3 // all = good
            { "GG_Crystal_Guardian_2", new() { GoName = "Battle Scene/Zombie Beam Miner Rematch", FsmName = "Beam Miner" } }, // Choice V3 // all = good
            { "GG_Dung_Defender"     , new() { GoName = "Dung Defender", FsmName = "Dung Defender" } }, // Move Choice V3 // all = good
            { "GG_Failed_Champion"   , new() { GoName = "False Knight Dream", FsmName = "FalseyControl" } }, // Move Choice no-V // !!! row check
            { "GG_False_Knight"      , new() { GoName = "Battle Scene/False Knight New", FsmName = "FalseyControl" } }, // Move Choice no-V // !!! row check
            { "GG_Flukemarm"         , null }, // X not interesting
            { "GG_Ghost_Galien"      , null }, // X not interesting
            { "GG_Ghost_Gorb"        , null }, // X not interesting
            { "GG_Ghost_Gorb_V"      , null }, // X not interesting
            { "GG_Ghost_Hu"          , null }, // X not interesting
            { "GG_Ghost_Markoth"     , null }, // X not interesting
            { "GG_Ghost_Markoth_V"   , null }, // X not interesting
            { "GG_Ghost_Marmu"       , null }, // X not interesting
            { "GG_Ghost_Marmu_V"     , null }, // X not interesting
            { "GG_Ghost_No_Eyes"     , null }, // X not interesting
            { "GG_Ghost_No_Eyes_V"   , null }, // X not interesting
            { "GG_Ghost_Xero"        , null }, // X not interesting
            { "GG_Ghost_Xero_V"      , null }, // X not interesting
            { "GG_God_Tamer"         , new() { GoName = "Entry Object/Lobster", FsmName = "Control", StateNames = new[] { "Attack Choice" } } }, // Attack Choice V3. Q: IDLE == FINISHED? // !!! to be verified
            { "GG_Grey_Prince_Zote"  , new() { GoName = "Grey Prince", FsmName = "Control", StateNames = new[] { "Move Choice 1", "Move Choice 2", "Move Choice 3" } } }, // V3
            { "GG_Grimm"             , new() { GoName = "Grimm Scene/Grimm Boss", FsmName = "Control" } }, // Move Choice V3 // !!! cannot change transition. verified it's the same go and fsm objects. boss states are not printing.
            { "GG_Grimm_Nightmare"   , new() { GoName = "Grimm Control/Nightmare Grimm Boss", FsmName = "Control" } }, // Move Choice V3 // !!! can control but first need to allow one slash to get boss start moving. see Move Choice action #0.
            { "GG_Gruz_Mother"       , new() { GoName = "_Enemies/Giant Fly", FsmName = "Big Fly Control", StateNames = new[] { "Super Choose" } } }, // V2
            { "GG_Gruz_Mother_V"     , new() { GoName = "_Enemies/Giant Fly", FsmName = "Big Fly Control", StateNames = new[] { "Super Choose" } } }, // V2 // !!! row check
            { "GG_Hive_Knight"       , new() { GoName = "Battle Scene/Hive Knight", FsmName = "Control", StateNames = new[] { "Phase 1", "Phase 2", "Phase 3" } } }, // V3 // !!! bee roar cannot trigger
            { "GG_Hollow_Knight"     , new() { GoName = "HK Prime", FsmName = "Control", StateNames = new[] { "Choice P1", "Choice P2", "Choice P3" } } }, // all = good
            { "GG_Hornet_1"          , new() { GoName = "Boss Holder/Hornet Boss 1", FsmName = "Control", StateNames = new[] { "Move Choice A", "Move Choice B" } } }, // V3 // all = good
            { "GG_Hornet_2"          , new() { GoName = "Boss Holder/Hornet Boss 2", FsmName = "Control", StateNames = new[] { "Move Choice A", "Move Choice B" } } }, // V3 // !!! add readme
            { "GG_Lost_Kin"          , new() { GoName = "Lost Kin", FsmName = "IK Control" } }, // Attack Choice V3 // all = good
            { "GG_Mage_Knight"       , new() { GoName = "Mage Knight", FsmName = "Mage Knight" } }, // Move Decision no-V
            { "GG_Mage_Knight_V"     , new() { GoName = "Balloon Spawner/Mage Knight", FsmName = "Mage Knight", StateNames = new[] { "Move Decision" } } }, // Move Decision no-V // !!! to be verified
            { "GG_Mantis_Lords"      , new() { GoName = "Mantis Battle/Battle Main/Mantis Lord", FsmName = "Mantis Lord" } }, // Attack Choice V3
            { "GG_Mantis_Lords_V"    , new() { GoName = "Mantis Battle/Battle Main/Mantis Lord", FsmName = "Mantis Lord" } }, // Attack Choice V3 // !!! only phase 1
            { "GG_Mega_Moss_Charger" , new() { GoName = "Mega Moss Charger", FsmName = "Mossy Control" } }, // Attack Choice V2 // all = good
            { "GG_Nailmasters"       , null }, // ??? very complex
            { "GG_Nosk"              , new() { GoName = "Mimic Spider", FsmName = "Mimic Spider",  StateNames = new[] { "Idle", "Idle No Spit" } } }, // V3
            { "GG_Nosk_Hornet"       , new() { GoName = "Battle Scene/Hornet Nosk", FsmName = "Hornet Nosk", StateNames = new[] { "Choose Attack", "Choose Attack 2" } } }, // V3 // !!! add readme about half
            { "GG_Oblobbles"         , null }, // X not interesting
            { "GG_Painter"           , new() { GoName = "Battle Scene/Sheo Boss", FsmName = "nailmaster_sheo", StateNames = new[] { "Choice" } } }, // Choice V3 // !!! to be verified
            { "GG_Radiance"          , new() { GoName = "Boss Control/Absolute Radiance", FsmName = "Attack Choices", StateNames = new[] { "A1 Choice", "A2 Choice" } } }, // V3 // !! add readme
            { "GG_Sly"               , new() { GoName = "Battle Scene/Sly Boss", FsmName = "Control", StateNames = new[] { "Near", "Mid", "Far", "NA Choice" } } }, // V3 // !!! add readme
            { "GG_Soul_Master"       , new() { GoName = "Mage Lord", FsmName = "Mage Lord" } }, // Attack Choice V3 // all = good
            { "GG_Soul_Tyrant"       , new() { GoName = "Dream Mage Lord", FsmName = "Mage Lord" } }, // all = good
            { "GG_Traitor_Lord"      , new() { GoName = "Battle Scene/Wave 3/Mantis Traitor Lord", FsmName = "Mantis" } }, // Attack Choice V2 // !!! SLAM has transition but never triggers
            { "GG_Uumuu"             , null }, // X not interesting
            { "GG_Uumuu_V"           , new() { GoName = "Mega Jellyfish GG", FsmName = "Mega Jellyfish", StateNames = new[] { "Choice" } } }, // V3 // !!! to be verified
            { "GG_Vengefly"          , new() { GoName = "Giant Buzzer Col", FsmName = "Big Buzzer", StateNames = new[] { "Choose Attack" } } }, // ??? Choose Attack manual random
            { "GG_Vengefly_V"        , new() { GoName = "Giant Buzzer Col", FsmName = "Big Buzzer" } }, // ??? Choose Attack manual random // ??? multiple GOs
            { "GG_Watcher_Knights"   , null }, // ??? multiple GOs
            { "GG_White_Defender"    , new() { GoName = "White Defender", FsmName = "Dung Defender" } }, // all = good
        };
    }
}
