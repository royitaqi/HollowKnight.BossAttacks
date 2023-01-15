using System.Collections.Generic;
using System.Linq;
using BossAttacks.Modules;
using BossAttacks.Modules.Generic;
using HutongGames.PlayMaker.Actions;

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
            { "GG_Broken_Vessel"     , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Infected Knight", FsmName = "IK Control" },
                new LevelChangerConfig { L = 0, H = 1, Display = "SHAKE (exclusive)", TargetL = 1, Mode = LevelChangerConfig.Modes.Bidirection },
                new VariableSetterConfig { L = 1, H = 1, StateName = "Idle",
                    BoolVariables = new KeyValuePair<string, bool>[]
                    {
                        new("Do Shake", true),
                    },
                },
            } },
            { "GG_Brooding_Mawlek"   , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Battle Scene/Mawlek Body", FsmName = "Mawlek Control", StateName = "Super Select",
                    MapEvents = new() {
                        { "TRUE", "SPIT" },
                        { "FALSE", "JUMP" },
                    },
                },
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Jumps In A Row", 0),
                        new("Spits In A Row", 0),
                    },
                },
            } },
            { "GG_Brooding_Mawlek_V" , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Battle Scene/Mawlek Body", FsmName = "Mawlek Control", StateName = "Super Select",
                    MapEvents = new() {
                        { "TRUE", "SPIT" },
                        { "FALSE", "JUMP" },
                    },
                },
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Jumps In A Row", 0),
                        new("Spits In A Row", 0),
                    },
                },
            } },
            { "GG_Collector"         , null },
            { "GG_Collector_V"       , null },
            { "GG_Crystal_Guardian"  , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Mega Zombie Beam Miner (1)", FsmName = "Beam Miner" },
            } },
            { "GG_Crystal_Guardian_2", new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Battle Scene/Zombie Beam Miner Rematch", FsmName = "Beam Miner" },
            } },
            { "GG_Dung_Defender"     , new ModuleConfig[] {
                new AttackSelectorConfig { L = 0, H = 1, GoName = "Dung Defender", FsmName = "Dung Defender" },
                new LevelChangerConfig { L = 0, H = 1, Display = "Trim ROLL JUMP", TargetL = 1, Mode = LevelChangerConfig.Modes.Bidirection },
                new TransitionRewirerConfig { L = 1, StateName = "RJ Set", EventName = "FINISHED", ToState = "Roll Speed" }, // trim head throw
            } },
            { "GG_Failed_Champion"   , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "False Knight Dream", FsmName = "FalseyControl", IgnoreEvents = new() { "RUN", "TEST" } },
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[] {
                        new("JA In A Row", 0), // row count for JUMP ATTACK
                        new("Slam In A Row", 0), // row count for SMASH
                        new("Jump Count", 0), // row check for JUMP
                        new("Stunned Amount", -9), // Needed for JUMP. See Determine Jump-1.
                    },
                },
            } },
            { "GG_False_Knight"      , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Battle Scene/False Knight New", FsmName = "FalseyControl", IgnoreEvents = new() { "RUN", "TEST" } },
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[] {
                        new("JA In A Row", 0), // row count for JUMP ATTACK
                        new("Slam In A Row", 0), // row count for SMASH
                        new("Jump Count", 0), // row check for JUMP
                        new("Stunned Amount", -9), // Needed for JUMP. See Determine Jump-1.
                    },
                },
            } },
            { "GG_Flukemarm"         , null }, // X not interesting
            { "GG_Ghost_Galien"      , null }, // X not interesting
            { "GG_Ghost_Gorb"        , null }, // X not interesting
            { "GG_Ghost_Gorb_V"      , null }, // X not interesting
            { "GG_Ghost_Hu"          , new ModuleConfig[] {
                new LabelConfig { ID = "label all", Display = "Current attack: All" },
                new LabelConfig { ID = "label RING", L = 1, Display = "Current attack: RING" },
                new LabelConfig { ID = "label MEGA", L = 2, Display = "Current attack: META" },
                // all
                new LevelChangerConfig { ID = "option all", L = 0, H = 2, Display = "All", Mode = LevelChangerConfig.Modes.OneDirection, TargetL = 0 },
                // RING
                new LevelChangerConfig { ID = "option RING", L = 0, H = 2, Display = "RING (exclusive)", Mode = LevelChangerConfig.Modes.OneDirection, TargetL = 1 },
                new EventEmitterConfig { ID = "skip MEGA event", L = 1, GoName = "Ghost Warrior Hu", FsmName = "Attacking", StateName = "Mega Warp Out", EventName = "SKIP" },
                new TransitionRewirerConfig { ID = "skip MEGA route", L = 1, ToState = "Choice 2" },
                // MEGA
                new LevelChangerConfig { ID = "option MEGA", L = 0, H = 2, Display = "MEGA (exclusive)", Mode = LevelChangerConfig.Modes.OneDirection, TargetL = 2 },
                new EventEmitterConfig { ID = "force MEGA in C2", L = 2, StateName = "Choice 2", EventName = "MEGA" },
            } },
            { "GG_Ghost_Markoth"     , null }, // X not interesting
            { "GG_Ghost_Markoth_V"   , null }, // X not interesting
            { "GG_Ghost_Marmu"       , null }, // X not interesting
            { "GG_Ghost_Marmu_V"     , null }, // X not interesting
            { "GG_Ghost_No_Eyes"     , null }, // X not interesting
            { "GG_Ghost_No_Eyes_V"   , null }, // X not interesting
            { "GG_Ghost_Xero"        , null }, // X not interesting
            { "GG_Ghost_Xero_V"      , null }, // X not interesting
            { "GG_God_Tamer"         , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Entry Object/Lobster", FsmName = "Control", StateName = "Attack Choice", IgnoreEvents = new() { "FINISHED" } },
            } },
            { "GG_Grey_Prince_Zote"  , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Grey Prince", FsmName = "Control", StateName = "Move Choice 3" },
                new EventEmitterConfig { StateName = "Move Level", EventName = "3" },
                new TransitionRewirerConfig { StateName = "Spit Antic", EventName = "CANCEL", ToState = "Idle Start" },
            } },
            { "GG_Grimm"             , null },
            { "GG_Grimm_Nightmare"   , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Grimm Control/Nightmare Grimm Boss", FsmName = "Control", StateName = "Move Choice", IgnoreEvents = new() { "BALLOON" } },
                new EventEmitterConfig { StateName = "After Evade", EventName = "SLASH" }, // Skip possible firebats after evade in SLASH
                new VariableSetterConfig {
                    BoolVariables = new KeyValuePair<string, bool>[]
                    {
                        new("First Move", true), // Remove the constraint that "first attack must be SLASH", because the user could have already unselected SLASH
                    },
                },
                new LevelChangerConfig { L = 0, H = 1, Display = "BALLOON (exclusive)", TargetL = 1, Mode = LevelChangerConfig.Modes.Bidirection },
                new EventEmitterConfig { L = 1, H = 1, EventName = "BALLOON" }, // enable BALLOON
            } },
            { "GG_Gruz_Mother"       , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "_Enemies/Giant Fly", FsmName = "Big Fly Control", StateName = "Super Choose" },
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Slams In A Row", 0),
                        new("Charges In A Row", 0),
                    },
                },
            } },
            { "GG_Gruz_Mother_V"     , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "_Enemies/Giant Fly", FsmName = "Big Fly Control", StateName = "Super Choose" },
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Slams In A Row", 0),
                        new("Charges In A Row", 0),
                    },
                },
            } },
            { "GG_Hive_Knight"       , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Battle Scene/Hive Knight", FsmName = "Control", StateName = "Phase 3", IgnoreEvents = new() { "JUMP" } },
                new EventEmitterConfig { StateName = "Phase Check", EventName = "P3" },
            } },
            { "GG_Hollow_Knight"     , new ModuleConfig[] {
                new EventEmitterConfig { GoName = "HK Prime", FsmName = "Control", StateName = "Phase?", EventName = "PHASE3" },
                new AttackSelectorConfig { StateName = "Choice P3" },
            } },
            { "GG_Hornet_1"          , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Boss Holder/Hornet Boss 1", FsmName = "Control", StateName = "Move Choice A" },
                new EventEmitterConfig { StateName = "Can Throw?", EventName = "CAN THROW" },
            } },
            { "GG_Hornet_2"          , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Boss Holder/Hornet Boss 2", FsmName = "Control", StateName = "Move Choice A" },
                new EventEmitterConfig { StateName = "Can Throw?", EventName = "CAN THROW" },
            } },
            { "GG_Lost_Kin"          , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Lost Kin", FsmName = "IK Control" },
            } },
            { "GG_Mage_Knight"       , GetSoulWarriorConfigs() },
            { "GG_Mage_Knight_V"     , GetSoulWarriorConfigs() },
            { "GG_Mantis_Lords"      , null },
            { "GG_Mantis_Lords_V"    , null },
            { "GG_Mega_Moss_Charger" , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Mega Moss Charger", FsmName = "Mossy Control" },
            } },
            { "GG_Nailmasters"       , null },
            { "GG_Nosk"              , new ModuleConfig[] {
                new NoskConfig { GoName = "Mimic Spider", FsmName = "Mimic Spider" }, // custom module
            } },
            { "GG_Nosk_Hornet"       , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Battle Scene/Hornet Nosk", FsmName = "Hornet Nosk", StateName = "Choose Attack 2" },
                new EventEmitterConfig { StateName = "Choose Attack", EventName = "HALF" },
            } },
            { "GG_Oblobbles"         , null }, // X not interesting
            { "GG_Painter"           , new ModuleConfig[] {
                new LabelConfig { Display = "Attacks:" },
                new AttackSelectorConfig { GoName = "Battle Scene/Sheo Boss", FsmName = "nailmaster_sheo", StateName = "Choice", IgnoreEvents = new() { "GREAT SLASH" } },
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Art Counter", 1),
                    },
                },
                new LabelConfig { Display = "After Evade:" },
                new AttackSelectorConfig { StateName = "Evade Choice", IgnoreEvents = new() { "GREAT SLASH" } },
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Art Counter", 1),
                    },
                },
                new LabelConfig { Display = "Exclusive:" },
                new LevelChangerConfig { L = 0, H = 1, Display = "GREAT SLASH", TargetL = 1, Mode = LevelChangerConfig.Modes.Bidirection },
                new VariableSetterConfig { L = 1, H = 1, StateName = "Choice",
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Art Counter", 0),
                    },
                },
                new VariableSetterConfig { L = 1, H = 1, StateName = "Evade Choice",
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Art Counter", 0),
                    },
                },
            } },
            { "GG_Radiance"          , new ModuleConfig[] {
                // Map phase transit to levels
                new AutoLevelChangerConfig { ID = "transit 0->1", GoName = "Boss Control/Absolute Radiance", FsmName = "Control", TargetL = 1, OnEnterState = "Arena 1 Start" }, // this state sends "ARENA 1 START"
                new AutoLevelChangerConfig { ID = "transit 1->2", H = 1, FsmName = "Phase Control", TargetL = 2, OnEnterState = "Set Phase 2" },
                new AutoLevelChangerConfig { ID = "transit 2->3", H = 2, FsmName = "Phase Control", TargetL = 3, OnEnterState = "Set Phase 3" },
                new AutoLevelChangerConfig { ID = "transit 3->4", H = 3, FsmName = "Control", TargetL = 4, OnEnterState = "A1 Tele 3" }, // this state sends "ARENA 2 START"
                new AutoLevelChangerConfig { ID = "transit 4->5", H = 4, FsmName = "Phase Control", TargetL = 5, OnEnterState = "Set Ascend" },
                // Attack selections
                new AttackSelectorConfig { ID = "AS in 1 and 2", L = 1, H = 2, FsmName = "Attack Choices", StateName = "A1 Choice" },
                new AttackSelectorConfig { ID = "AS in 4", L = 4, FsmName = "Attack Choices", StateName = "A2 Choice" },
            } },
            { "GG_Sly"               , null },
            { "GG_Soul_Master"       , GetSoulMasterAndTyrantConfigs(false) },
            { "GG_Soul_Tyrant"       , GetSoulMasterAndTyrantConfigs(true) },
            { "GG_Traitor_Lord"      , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Battle Scene/Wave 3/Mantis Traitor Lord", FsmName = "Mantis", IgnoreEvents = new() { "SLAM" } },
            } },
            { "GG_Uumuu"             , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Mega Jellyfish GG", FsmName = "Mega Jellyfish", StateName = "Choice" },
            } },
            { "GG_Uumuu_V"           , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Mega Jellyfish GG", FsmName = "Mega Jellyfish", StateName = "Choice" },
            } },
            { "GG_Vengefly"          , GetVengeflyConfigs("Giant Buzzer Col", "Big Buzzer", "SWOOP", "SUMMON") },
            { "GG_Vengefly_V"        , new[] {
                    GetVengeflyConfigs("Giant Buzzer Col", "Big Buzzer", "Boss #1 SWOOP", "Boss #1 SUMMON"),
                    GetVengeflyConfigs("Giant Buzzer Col (1)", "Big Buzzer", "Boss #2 SWOOP", "Boss #2 SUMMON"),
                }.SelectMany(c => c).ToArray()
            },
            { "GG_Watcher_Knights"   , null },
            { "GG_White_Defender"    , new ModuleConfig[] {
                new AttackSelectorConfig { L = 0, H = 1, GoName = "White Defender", FsmName = "Dung Defender", IgnoreEvents = new() { "GROUND SLAM" } },
                new LevelChangerConfig { L = 0, H = 1, Display = "Trim ROLL JUMP", TargetL = 1, Mode = LevelChangerConfig.Modes.Bidirection },
                new TransitionRewirerConfig { L = 1, StateName = "RJ Set", EventName = "FINISHED", ToState = "Roll Speed" }, // trim head throw
                new EventEmitterConfig { L = 1, StateName = "Air Dive?", ActionType = typeof(SendRandomEvent), IndexDelta = 2, EventName = "FINISHED" }, // trim tail dive
            } },
        };

        private static ModuleConfig[] GetSoulWarriorConfigs()
        {
            /**
             *                             All Attacks
             *                            /    SHOOT
             *                           |    /    SLASH
             *                           |   |    /    STOMP
             *                           |   |   |    /
             *                         | 0 | 1 | 2 | 3 |
             * -----------------------------------------
             * label all               | t ----------- |
             * label SHOOT             | --- t ------- |
             * label SLASH             | ------- t --- |
             * label STOMP             | ----------- t |
             * option all              | t ----------- |
             * option SHOOT            | --- t ------- |
             * option SLASH            | ------- t --- |
             * option STOMP            | ----------- t |
             * SCP                     | ------------- |
             * force SHOOT after tele  |   | - |   |   |
             * force side tele         |   | ----- |   |
             * cancel up tele          |   | ----- |   |
             * force SLASH after tele  |   |   | - |   |
             * block SHOOT in MD       |   |   | ----- |
             * force STOMP tele        |   |   |   | - |
             * block SLASH in MD       |   | - |   | - |
             */

            // for both non-V and V
            var goName = "Mage Knight";
            var fsmName = "Mage Knight";
            return new ModuleConfig[] {
                new LabelConfig { ID = "label all", L = 0, Display = "Current attack: All" },
                new LabelConfig { ID = "label SHOOT", L = 1, Display = "Current attack: SHOOT" },
                new LabelConfig { ID = "label SLASH", L = 2, Display = "Current attack: SLASH" },
                new LabelConfig { ID = "label STOMP", L = 3, Display = "Current attack: STOMP" },
                new LevelChangerConfig { ID = "option all", H = 3, Display = "All", TargetL = 0, Mode = LevelChangerConfig.Modes.OneDirection },
                new LevelChangerConfig { ID = "option SHOOT", H = 3, Display = "SHOOT (exclusive)", TargetL = 1, Mode = LevelChangerConfig.Modes.OneDirection },
                new LevelChangerConfig { ID = "option SLASH", H = 3, Display = "SLASH (exclusive)", TargetL = 2, Mode = LevelChangerConfig.Modes.OneDirection },
                new LevelChangerConfig { ID = "option STOMP", H = 3, Display = "STOMP (exclusive)", TargetL = 3, Mode = LevelChangerConfig.Modes.OneDirection },
                new ShortCircuitProtectionConfig { ID = "SCP", H = 3, GoName = goName, FsmName = fsmName, StateName = "Move Decision", ScpStateName = "Move Decision SCP" },
                // SHOOT & SLASH
                new EventEmitterConfig { ID = "force side tele", Levels = new() { 1, 2 }, StateName = "Tele Choice", EventName = "SIDE" },
                new TransitionRewirerConfig { ID = "cancel up tele", Levels = new() { 1, 2 }, StateName = "Up Tele Aim", EventName = "FINISHED", ToState = "Idle" },
                // SHOOT
                new TransitionRewirerConfig { ID = "block SLASH in MD", Levels = new() { 1, 3 }, StateName = "Move Decision", EventName = "SLASH", ToState = "Move Decision SCP" },
                new EventEmitterConfig { ID = "force SHOOT after tele", Levels = new() { 1 }, StateName = "After Side Tele", EventName = "SHOOT" },
                // SLASH
                new TransitionRewirerConfig { ID = "block SHOOT in MD", Levels = new() { 2, 3 }, StateName = "Move Decision", EventName = "SHOOT", ToState = "Move Decision SCP" },
                new EventEmitterConfig { ID = "force SLASH after tele", Levels = new() { 2 }, StateName = "After Side Tele", EventName = "SLASH" },
                // STOMP
                new EventEmitterConfig { ID = "force STOMP tele", Levels = new() { 3 }, StateName = "Tele Choice", EventName = "STOMP" },
            };
        }

        private static ModuleConfig[] GetSoulMasterAndTyrantConfigs(bool v)
        {
            /**
             *     | 0 | 1 | 2 | 3 |   GO/FSM
             * --------------------------------
             * AS  | x |   |   |   |   1
             * MPC | x | t |   |   |   n/a
             * BK  |   | x |   |   |   1
             * ALC | x | x | t |   |   2
             * IDO |   |   | x | x |   n/a
             * ID  |   |   |   | x |   2
             * ID2 |   |   |   | x |   2
             */
            var soulMaster = new Dictionary<string, string>
            {
                { "phase 1 GoName", "Mage Lord" },
                { "phase 1 FsmName", "Mage Lord" },
                { "phase 2 GoName", "Mage Lord Phase2" },
                { "phase 2 FsmName", "Mage Lord 2" },
                { "phase 2 OnEnterState", "Arrive Pause" },
            };
            var soulTyrant = new Dictionary<string, string>
            {
                { "phase 1 GoName", "Dream Mage Lord" },
                { "phase 1 FsmName", "Mage Lord" },
                { "phase 2 GoName", "Dream Mage Lord Phase2" },
                { "phase 2 FsmName", "Mage Lord 2" },
                { "phase 2 OnEnterState", "Wait" },
            };
            var m = v ? soulTyrant : soulMaster;

            return new ModuleConfig[] {
                new AttackSelectorConfig { ID = "AS", GoName = m["phase 1 GoName"], FsmName = m["phase 1 FsmName"] }, // AS
                new LevelChangerConfig { ID = "manual phase changer", Display = "Advance to Phase 2", TargetL = 1, Mode = LevelChangerConfig.Modes.OneTime }, // manual phase changer
                new GoKillerConfig { ID = "boss killer", L = 1 }, // boss killer
                new AutoLevelChangerConfig { ID = "auto level changer", L = 0, H = 1, GoName = m["phase 2 FsmName"], FsmName = m["phase 2 FsmName"], OnEnterState = m["phase 2 OnEnterState"], TargetL = 2 }, // auto level changer
                new LevelChangerConfig { ID = "infinite dive option", L = 2, H = 3, Display = "Infinite QUAKE", TargetL = 3, Mode = LevelChangerConfig.Modes.Bidirection }, // infinite dive option
                new EventEmitterConfig { ID = "infinite dive dive", L = 3, StateName = "Shoot?", ActionType = typeof(IntCompare), EventName = "FINISHED" }, // infinite dive dive
                new EventEmitterConfig { ID = "infinite dive orb", L = 3, StateName = "Orb Check", EventName = "END" }, // infinite dive orb
            };
        }

        private static ModuleConfig[] GetVengeflyConfigs(string goName, string fsmName, string swoopName, string summonName)
        {
            return new ModuleConfig[] {
                new AttackSelectorConfig { GoName = goName, FsmName = fsmName, StateName = "Choose Attack",
                    MapEvents = new()
                    {
                        { "SWOOP", swoopName },
                        { "SUMMON", summonName + " (up to 15 roars)" },
                    },
                },
                new VariableSetterConfig
                {
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Summons In A Row", 0),
                        new("Swoops in A Row", 0),
                    },
                },
                new TransitionRewirerConfig { StateName = "Check Summon", EventName = "CANCEL", ToState = "Idle" },
                new TransitionRewirerConfig { StateName = "Check Summon GG", EventName = "CANCEL", ToState = "Idle" },
            };
        }
    }
}
