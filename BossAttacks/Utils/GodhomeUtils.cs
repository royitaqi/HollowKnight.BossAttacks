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
            { "GG_Broken_Vessel"     , new() { GoName = "Infected Knight", FsmName = "IK Control" } }, // Attack Choice V3 // all = good // plan A generic
            { "GG_Brooding_Mawlek"   , new() { GoName = "Battle Scene/Mawlek Body", FsmName = "Mawlek Control" } }, // X no random // !!! Can still be rewired
            { "GG_Brooding_Mawlek_V" , new() { GoName = "Battle Scene/Mawlek Body", FsmName = "Mawlek Control" } }, // X no random // !!! Can still be rewired
            { "GG_Collector"         , new() { GoName = "Battle Scene/Jar Collector", FsmName = "Control" } }, // Move Choice V2 // !!! cannot control jars
            { "GG_Collector_V"       , new() { GoName = "Battle Scene/Jar Collector", FsmName = "Control" } }, // Move Choice V2 // !!! cannot control jars
            { "GG_Crystal_Guardian"  , new() { GoName = "Mega Zombie Beam Miner (1)", FsmName = "Beam Miner" } }, // Choice V3 // all = good // plan A generic
            { "GG_Crystal_Guardian_2", new() { GoName = "Battle Scene/Zombie Beam Miner Rematch", FsmName = "Beam Miner" } }, // Choice V3 // all = good // plan A generic
            { "GG_Dung_Defender"     , new() { GoName = "Dung Defender", FsmName = "Dung Defender" } }, // Move Choice V3 // all = good // plan A generic + behavior rewirer
            { "GG_Failed_Champion"   , new() { GoName = "False Knight Dream", FsmName = "FalseyControl" } }, // Move Choice no-V // !!! row check // plan A generic + variable setter
            { "GG_False_Knight"      , new() { GoName = "Battle Scene/False Knight New", FsmName = "FalseyControl" } }, // Move Choice no-V // !!! row check // plan A generic + variable setter
            { "GG_Flukemarm"         , null }, // X not interesting
            { "GG_Ghost_Galien"      , null }, // X not interesting
            { "GG_Ghost_Gorb"        , null }, // X not interesting
            { "GG_Ghost_Gorb_V"      , null }, // X not interesting
            { "GG_Ghost_Hu"          , null }, // X not interesting // !!! just need to figure out which FSM controls the attack
            { "GG_Ghost_Markoth"     , null }, // X not interesting
            { "GG_Ghost_Markoth_V"   , null }, // X not interesting
            { "GG_Ghost_Marmu"       , null }, // X not interesting
            { "GG_Ghost_Marmu_V"     , null }, // X not interesting
            { "GG_Ghost_No_Eyes"     , null }, // X not interesting
            { "GG_Ghost_No_Eyes_V"   , null }, // X not interesting
            { "GG_Ghost_Xero"        , null }, // X not interesting
            { "GG_Ghost_Xero_V"      , null }, // X not interesting
            { "GG_God_Tamer"         , new() { GoName = "Entry Object/Lobster", FsmName = "Control", StateNames = new[] { "Attack Choice" } } }, // Attack Choice V3. // remove FINISHED from attack list // plan A generic
            { "GG_Grey_Prince_Zote"  , new() { GoName = "Grey Prince", FsmName = "Control", StateNames = new[] { "Move Choice 1", "Move Choice 2", "Move Choice 3" } } }, // V3 // plan A phased
            { "GG_Grimm"             , new() { GoName = "Grimm Scene/Grimm Boss", FsmName = "Control" } }, // Move Choice V3 // !!! cannot change transition. verified it's the same go and fsm objects. boss states are not printing.
            { "GG_Grimm_Nightmare"   , new() { GoName = "Grimm Control/Nightmare Grimm Boss", FsmName = "Control" } }, // Move Choice V3 // !!! can control but first need to allow one slash to get boss start moving. see Move Choice action #0. // plan A generic + variable setter
            { "GG_Gruz_Mother"       , new() { GoName = "_Enemies/Giant Fly", FsmName = "Big Fly Control", StateNames = new[] { "Super Choose" } } }, // V2 // plan A generic + variable setter
            { "GG_Gruz_Mother_V"     , new() { GoName = "_Enemies/Giant Fly", FsmName = "Big Fly Control", StateNames = new[] { "Super Choose" } } }, // V2 // !!! row check // plan A generic + variable setter
            { "GG_Hive_Knight"       , new() { GoName = "Battle Scene/Hive Knight", FsmName = "Control", StateNames = new[] { "Phase 1", "Phase 2", "Phase 3" } } }, // V3 // !!! bee roar cannot trigger
            { "GG_Hollow_Knight"     , new() { GoName = "HK Prime", FsmName = "Control", StateNames = new[] { "Choice P1", "Choice P2", "Choice P3" } } }, // all = good
            { "GG_Hornet_1"          , new() { GoName = "Boss Holder/Hornet Boss 1", FsmName = "Control", StateNames = new[] { "Move Choice A", "Move Choice B" } } }, // V3 // all = good // !!! parallel Choice states
            { "GG_Hornet_2"          , new() { GoName = "Boss Holder/Hornet Boss 2", FsmName = "Control", StateNames = new[] { "Move Choice A", "Move Choice B" } } }, // V3 // !!! add readme // !!! parallel Choice states
            { "GG_Lost_Kin"          , new() { GoName = "Lost Kin", FsmName = "IK Control" } }, // Attack Choice V3 // all = good
            { "GG_Mage_Knight"       , new() { GoName = "Mage Knight", FsmName = "Mage Knight" } }, // Move Decision no-V
            { "GG_Mage_Knight_V"     , new() { GoName = "Balloon Spawner/Mage Knight", FsmName = "Mage Knight", StateNames = new[] { "Move Decision" } } }, // Move Decision no-V // !!! to be verified
            { "GG_Mantis_Lords"      , new() { GoName = "Mantis Battle/Battle Main/Mantis Lord", FsmName = "Mantis Lord" } }, // Attack Choice V3
            { "GG_Mantis_Lords_V"    , new() { GoName = "Mantis Battle/Battle Main/Mantis Lord", FsmName = "Mantis Lord" } }, // Attack Choice V3 // !!! only phase 1
            { "GG_Mega_Moss_Charger" , new() { GoName = "Mega Moss Charger", FsmName = "Mossy Control" } }, // Attack Choice V2 // all = good
            { "GG_Nailmasters"       , null }, // ??? very complex
            { "GG_Nosk"              , new() { GoName = "Mimic Spider", FsmName = "Mimic Spider",  StateNames = new[] { "Idle", "Idle No Spit" } } }, // V3 // !!! parallel Choice states // !!! need custom module to remove constraints
            { "GG_Nosk_Hornet"       , new() { GoName = "Battle Scene/Hornet Nosk", FsmName = "Hornet Nosk", StateNames = new[] { "Choose Attack", "Choose Attack 2" } } }, // V3 // !!! add readme about HALF or remove from attack list
            { "GG_Oblobbles"         , null }, // X not interesting
            { "GG_Painter"           , new() { GoName = "Battle Scene/Sheo Boss", FsmName = "nailmaster_sheo", StateNames = new[] { "Choice" } } }, // Choice V3 // !!! to be verified
            { "GG_Radiance"          , new() { GoName = "Boss Control/Absolute Radiance", FsmName = "Attack Choices", StateNames = new[] { "A1 Choice", "A2 Choice" } } }, // V3 // !! add readme
            { "GG_Sly"               , new() { GoName = "Battle Scene/Sly Boss", FsmName = "Control", StateNames = new[] { "Near", "Mid", "Far", "NA Choice" } } }, // V3 // !!! add readme // !!! parallel Choice states
            { "GG_Soul_Master"       , new() { GoName = "Mage Lord", FsmName = "Mage Lord" } }, // Attack Choice V3 // all = good
            { "GG_Soul_Tyrant"       , new() { GoName = "Dream Mage Lord", FsmName = "Mage Lord" } }, // all = good
            { "GG_Traitor_Lord"      , new() { GoName = "Battle Scene/Wave 3/Mantis Traitor Lord", FsmName = "Mantis" } }, // Attack Choice V2 // !!! SLAM has transition but never triggers - add readme about SLAM or remove from attack list
            { "GG_Uumuu"             , null }, // X not interesting
            { "GG_Uumuu_V"           , new() { GoName = "Mega Jellyfish GG", FsmName = "Mega Jellyfish", StateNames = new[] { "Choice" } } }, // V3 // !!! to be verified
            { "GG_Vengefly"          , new() { GoName = "Giant Buzzer Col", FsmName = "Big Buzzer", StateNames = new[] { "Choose Attack" } } }, // ??? Choose Attack manual random
            { "GG_Vengefly_V"        , new() { GoName = "Giant Buzzer Col", FsmName = "Big Buzzer" } }, // ??? Choose Attack manual random // ??? multiple GOs
            { "GG_Watcher_Knights"   , null }, // ??? multiple GOs // ??? multiple Choice states interconnected (support for multiple GO isn't high priority)
            { "GG_White_Defender"    , new() { GoName = "White Defender", FsmName = "Dung Defender" } }, // all = good
        };


        public static readonly Dictionary<string, ModuleConfig[]> SceneToModuleConfigs = new()
        {
            { "GG_Broken_Vessel"     , null },
            { "GG_Brooding_Mawlek"   , new ModuleConfig[] {
                new DefaultConfig { GoName = "Battle Scene/Mawlek Body", FsmName = "Mawlek Control" },
                new GenericAttackSelectorConfig { StateName = "Super Select", MapEvents = new() { { "TRUE", "SPIT" }, { "FALSE", "JUMP" } } },
                new PrintStatesModuleConfig(),
            } },
            { "GG_Brooding_Mawlek_V" , null },
            { "GG_Collector"         , null },
            { "GG_Collector_V"       , null },
            { "GG_Crystal_Guardian"  , new ModuleConfig[] {
                new DefaultConfig { GoName = "Mega Zombie Beam Miner (1)", FsmName = "Beam Miner" },
                new GenericAttackSelectorConfig(),
                new PrintStatesModuleConfig(),
            } },
            { "GG_Crystal_Guardian_2", new ModuleConfig[] {
                new DefaultConfig { GoName = "Battle Scene/Zombie Beam Miner Rematch", FsmName = "Beam Miner" },
                new GenericAttackSelectorConfig(),
                new PrintStatesModuleConfig(),
            } },
            { "GG_Dung_Defender"     , new ModuleConfig[] {
                new DefaultConfig { GoName = "Dung Defender", FsmName = "Dung Defender" },
                new GenericAttackSelectorConfig(),
                new PrintStatesModuleConfig(),
            } },
            { "GG_Failed_Champion"   , new ModuleConfig[] {
                new DefaultConfig { GoName = "False Knight Dream", FsmName = "FalseyControl" },
                new GenericAttackSelectorConfig { IgnoreEvents = new() { "RUN", "TEST" } },
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[] {
                        new("JA In A Row", 0),
                        new("Slam In A Row", 0),
                        new("Jump Count", 0),
                        new("Stunned Amount", -9),
                    },
                },
                new PrintStatesModuleConfig(),
            } },
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
            { "GG_God_Tamer"         , new ModuleConfig[] {
                new DefaultConfig { GoName = "Entry Object/Lobster", FsmName = "Control" },
                new GenericAttackSelectorConfig { StateName = "Attack Choice", IgnoreEvents = new() { "FINISHED" } },
                new PrintStatesModuleConfig(),
            } },
            { "GG_Grey_Prince_Zote"  , null },
            { "GG_Grimm"             , null },
            { "GG_Grimm_Nightmare"   , null },
            { "GG_Gruz_Mother"       , new ModuleConfig[] {
                new DefaultConfig { GoName = "_Enemies/Giant Fly", FsmName = "Big Fly Control", StateName = "Super Choose" },
                new GenericAttackSelectorConfig(),
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Slams In A Row", 0),
                        new("Charges In A Row", 0),
                    },
                },
                new PrintStatesModuleConfig(),
            } },
            { "GG_Gruz_Mother_V"     , null },
            { "GG_Hive_Knight"       , null },
            { "GG_Hollow_Knight"     , new ModuleConfig[] {
                new DefaultConfig { GoName = "HK Prime", FsmName = "Control" },
                new EventEmitterConfig { StateName = "Phase?", EventName = "PHASE3" },
                new GenericAttackSelectorConfig { StateName = "Choice P3" },
                new PrintStatesModuleConfig(),
            } },
            { "GG_Hornet_1"          , null },
            { "GG_Hornet_2"          , null },
            { "GG_Lost_Kin"          , new ModuleConfig[] {
                new DefaultConfig { GoName = "Lost Kin", FsmName = "IK Control" },
                new GenericAttackSelectorConfig(),
                new PrintStatesModuleConfig(),
            } },
            { "GG_Mage_Knight"       , new ModuleConfig[] {
                new DefaultConfig { GoName = "Mage Knight", FsmName = "Mage Knight", StateName = "Move Decision" },
                new GenericAttackSelectorConfig(),
                new PrintStatesModuleConfig(),
            } },
            { "GG_Mage_Knight_V"     , null },
            { "GG_Mantis_Lords"      , null },
            { "GG_Mantis_Lords_V"    , null },
            { "GG_Mega_Moss_Charger" , new ModuleConfig[] {
                new DefaultConfig { GoName = "Mega Moss Charger", FsmName = "Mossy Control" },
                new GenericAttackSelectorConfig(),
                new PrintStatesModuleConfig(),
            } },
            { "GG_Nailmasters"       , null },
            { "GG_Nosk"              , null },
            { "GG_Nosk_Hornet"       , null },
            { "GG_Oblobbles"         , null },
            { "GG_Painter"           , null },
            { "GG_Radiance"          , null },
            { "GG_Sly"               , null },
            { "GG_Soul_Master"       , null },
            { "GG_Soul_Tyrant"       , null },
            { "GG_Traitor_Lord" , new ModuleConfig[] {
                new DefaultConfig { GoName = "Battle Scene/Wave 3/Mantis Traitor Lord", FsmName = "Mantis" },
                new GenericAttackSelectorConfig { IgnoreEvents = new() { "SLAM" } },
                new PrintStatesModuleConfig(),
            } },
            { "GG_Uumuu"             , null },
            { "GG_Uumuu_V"           , null },
            { "GG_Vengefly"          , null },
            { "GG_Vengefly_V"        , null },
            { "GG_Watcher_Knights"   , null },
            { "GG_White_Defender"    , null },
        };
    }
}
