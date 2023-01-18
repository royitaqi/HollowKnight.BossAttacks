using System.Collections.Generic;
using System.Linq;
using BossAttacks.Modules;
using BossAttacks.Modules.Generic;
using HutongGames.PlayMaker.Actions;

namespace BossAttacks.Utils
{
    static class GodhomeUtils
    {
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
            { "GG_Brooding_Mawlek"   , GetBroodingMawlekConfigs() },
            { "GG_Brooding_Mawlek_V" , GetBroodingMawlekConfigs() },
            { "GG_Collector"         , GetTheCollectorConfigs() },
            { "GG_Collector_V"       , GetTheCollectorConfigs() },
            { "GG_Crystal_Guardian"  , new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Mega Zombie Beam Miner (1)", FsmName = "Beam Miner" },
            } },
            { "GG_Crystal_Guardian_2", new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "Battle Scene/Zombie Beam Miner Rematch", FsmName = "Beam Miner" },
            } },
            { "GG_Dung_Defender"     , GetDungDefenderAndWhiteDefenderConfigs(false) },
            { "GG_Failed_Champion"   , GetFalseKnightAndFailedChampionConfigs(true) },
            { "GG_False_Knight"      , GetFalseKnightAndFailedChampionConfigs(false) },
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
                new LevelChangerConfig { ID = "option RING", L = 0, H = 2, Display = "RING", Mode = LevelChangerConfig.Modes.OneDirection, TargetL = 1 },
                new EventEmitterConfig { ID = "skip MEGA event", L = 1, GoName = "Ghost Warrior Hu", FsmName = "Attacking", StateName = "Mega Warp Out", EventName = "SKIP" },
                new TransitionRewirerConfig { ID = "skip MEGA route", L = 1, ToState = "Choice 2" },
                // MEGA
                new LevelChangerConfig { ID = "option MEGA", L = 0, H = 2, Display = "MEGA", Mode = LevelChangerConfig.Modes.OneDirection, TargetL = 2 },
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
            { "GG_Gruz_Mother"       , GetGruzMotherConfigs() },
            { "GG_Gruz_Mother_V"     , GetGruzMotherConfigs() },
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
            { "GG_Mantis_Lords"      , new ModuleConfig[] {
                /**
                 *                             Phase 1 normal fight
                 *                            /    Phase 1 kill boss
                 *                           |    /    Phase 2 coordinated fight
                 *                           |   |    /    Phase 2 uncoordinated fight
                 *                           |   |   |    /
                 *                         | 0 | 1 | 2 | 3 |
                 * -----------------------------------------
                 * phase 1 AS              | - |   |   |   |
                 * phase 1->2 manual       | - | t |   |   |
                 * phase 1 boss killer     |   | - |   |   |
                 * phase 1->2 auto         | ----- | t |   |
                 * phase 2c AS             |   |   | - |   |
                 * phase 2c<->2u           |   |   | t - T |
                 * phase 2u s1             |   |   |   | - |
                 * phase 2u s2             |   |   |   | - |
                 */
                new AttackSelectorConfig { ID = "phase 1 AS", GoName = "Mantis Battle/Battle Main/Mantis Lord", FsmName = "Mantis Lord", MapEvents = new() { { "HIGH THROW", "HIGH THROW (only when hero is off platform or too high)" } } },
                new LevelChangerConfig { ID = "phase 1->2 manual", Display = "Advance to Phase 2", TargetL = 1, Mode = LevelChangerConfig.Modes.OneTime },
                new GoKillerConfig { ID = "phase 1 boss killer", L = 1 },
                new AutoLevelChangerConfig { ID = "phase 1->2 auto", L = 0, H = 1, GoName = "Mantis Battle/Battle Sub", FsmName = "Start", OnEnterState = "Start", TargetL = 2 },
                new AttackSelectorConfig { ID = "phase 2c AS", L = 2, StateName = "Choose Move" },
                new LevelChangerConfig { ID = "phase 2c<->2u", L = 2, H = 3, Display = "Extra: Uncoordinated Attacks", TargetL = 3, Mode = LevelChangerConfig.Modes.Bidirection },
                new ActionEnablerConfig { ID = "phase 2u s1", L = 3, GoName = "Mantis Battle/Battle Sub/Mantis Lord S1", FsmName = "Mantis Lord", StateName = "Idle", ActionType = typeof(BoolTest), ToEnabled = false },
                new ActionEnablerConfig { ID = "phase 2u s2", L = 3, GoName = "Mantis Battle/Battle Sub/Mantis Lord S2", FsmName = "Mantis Lord", StateName = "Idle", ActionType = typeof(BoolTest), ToEnabled = false },
            } },
            { "GG_Mantis_Lords_V"    , new ModuleConfig[] {
                new AttackSelectorConfig { ID = "phase 1 AS", GoName = "Mantis Battle/Battle Main/Mantis Lord", FsmName = "Mantis Lord", MapEvents = new() { { "HIGH THROW", "HIGH THROW (only when hero is off platform or too high)" } } },
                new LevelChangerConfig { ID = "phase 1->2 manual", Display = "Advance to Phase 2", TargetL = 1, Mode = LevelChangerConfig.Modes.OneTime },
                new GoKillerConfig { ID = "phase 1 boss killer", L = 1 },
                new AutoLevelChangerConfig { ID = "phase 1->2 auto", L = 0, H = 1, GoName = "Mantis Battle/Battle Sub", FsmName = "Start", OnEnterState = "Start", TargetL = 2 },
                new AttackSelectorConfig { ID = "phase 2c AS", L = 2, StateName = "Choose Move Triple" },
                new LevelChangerConfig { ID = "phase 2c<->2u", L = 2, H = 3, Display = "Extra: Uncoordinated Attacks", TargetL = 3, Mode = LevelChangerConfig.Modes.Bidirection },
                new ActionEnablerConfig { ID = "phase 2u s1", L = 3, GoName = "Mantis Battle/Battle Sub/Mantis Lord S1", FsmName = "Mantis Lord", StateName = "Idle", ActionType = typeof(BoolTest), ToEnabled = false },
                new ActionEnablerConfig { ID = "phase 2u s2", L = 3, GoName = "Mantis Battle/Battle Sub/Mantis Lord S2", FsmName = "Mantis Lord", StateName = "Idle", ActionType = typeof(BoolTest), ToEnabled = false },
                new ActionEnablerConfig { ID = "phase 2u s3", L = 3, GoName = "Mantis Battle/Battle Sub/Mantis Lord S3", FsmName = "Mantis Lord", StateName = "Idle", ActionType = typeof(BoolTest), ToEnabled = false },
            } },
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
            { "GG_Sly"               , new ModuleConfig[] {
                /**
                 *                             All Attacks
                 *                            /    COMBO only
                 *                           |    /    STOMP only
                 *                           |   |    /    NA only (can select)
                 *                           |   |   |    /    Phase 2
                 *                           |   |   |   |    /
                 *                         | 0 | 1 | 2 | 3 | 4 |
                 * ---------------------------------------------
                 * label all               | - |   |   |   |   |
                 * label COMBO             |   | - |   |   |   |
                 * label STOMP             |   |   | - |   |   |
                 * label NA                |   |   |   | - |   |
                 * option all              | T ----------- |   |
                 * option COMBO            | --- T ------- |   |
                 * option STOMP            | ------- T --- |   |
                 * option NA               | ----------- T |   |
                 * block COMBO             |   |   | - |   |   |
                 * block STOMP             |   | - |   |   |   |
                 * block NA                |   | ----- |   |   |
                 * force NA                |   |   |   | - |   |
                 * force NA no evade       |   |   |   | - |   |
                 * NA attack selector      |   |   |   | - |   |
                 * phase 1->2              | --------------- t |
                 */
                new LabelConfig { ID = "label all", Display = "Current attack: All" },
                new LabelConfig { ID = "label COMBO", L = 1, Display = "Current attack: COMBO" },
                new LabelConfig { ID = "label STOMP", L = 2, Display = "Current attack: STOMP" },
                new LabelConfig { ID = "label NA", L = 3, Display = "Current attack: Nail Arts" },
                new LevelChangerConfig { ID = "option all", H = 3, Display = "All", TargetL = 0, Mode = LevelChangerConfig.Modes.OneDirection },
                new LevelChangerConfig { ID = "option COMBO", H = 3, Display = "COMBO", TargetL = 1, Mode = LevelChangerConfig.Modes.OneDirection },
                new LevelChangerConfig { ID = "option STOMP", H = 3, Display = "STOMP", TargetL = 2, Mode = LevelChangerConfig.Modes.OneDirection },
                new LevelChangerConfig { ID = "option NA", H = 3, Display = "Nail Arts", TargetL = 3, Mode = LevelChangerConfig.Modes.OneDirection },
                // block COMBO
                new TransitionRewirerConfig { ID = "block COMBO near", L = 2, GoName = "Battle Scene/Sly Boss", FsmName = "Control", StateName = "Near", EventName = "COMBO", ToState = "Idle" },
                new TransitionRewirerConfig { ID = "block COMBO mid", L = 2, StateName = "Mid", EventName = "COMBO", ToState = "Idle" },
                new TransitionRewirerConfig { ID = "block COMBO far", L = 2, StateName = "Far", EventName = "CHASE", ToState = "Idle" },
                // block STOMP
                new TransitionRewirerConfig { ID = "block COMBO near", L = 1, StateName = "Near", EventName = "STOMP", ToState = "Idle" },
                new TransitionRewirerConfig { ID = "block COMBO mid", L = 1, StateName = "Mid", EventName = "STOMP", ToState = "Idle" },
                new TransitionRewirerConfig { ID = "block COMBO far", L = 1, StateName = "Far", EventName = "STOMP", ToState = "Idle" },
                // block NA
                new VariableSetterConfig { ID = "block NA", L = 1, H = 2, StateName = "Idle",
                    IntVariables = new KeyValuePair<string, int>[] {
                        new("Art Counter", 1), // larger than 0 to not trigger NAIL ART
                    },
                },
                // force NA
                new VariableSetterConfig { ID = "force NA", L = 3, StateName = "Idle",
                    IntVariables = new KeyValuePair<string, int>[] {
                        new("Art Counter", 0), // equal 0 to trigger NAIL ART
                    },
                },
                new TransitionRewirerConfig { ID = "force NA no evade", L = 3, StateName = "Evade Cancel", EventName = "FINISHED", ToState = "Idle" },
                // NA AS
                new AttackSelectorConfig { ID = "NA attack selector", L = 3, StateName = "NA Choice" },
                // phase 1->2
                new AutoLevelChangerConfig { ID = "phase 1->2", H = 4, OnEnterState = "Death Reset", TargetL = 4 },
            } },
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
                    GetVengeflyConfigs("Giant Buzzer Col", "Big Buzzer", "SWOOP", "SUMMON"),
                    GetVengeflyConfigs("Giant Buzzer Col (1)", "Big Buzzer", "SWOOP", "SUMMON"),
                    new[] { new OptionCombinerConfig() },
                }.SelectMany(c => c).ToArray()
            },
            { "GG_Watcher_Knights"   , new[] {
                    GetWatcherKnightConfigs("1"),
                    GetWatcherKnightConfigs("2"),
                    GetWatcherKnightConfigs("3"),
                    GetWatcherKnightConfigs("4"),
                    GetWatcherKnightConfigs("5"),
                    GetWatcherKnightConfigs("6"),
                    new[] { new OptionCombinerConfig() },
                }.SelectMany(c => c).ToArray()
            },
            { "GG_White_Defender"    , GetDungDefenderAndWhiteDefenderConfigs(true) },
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
                new LevelChangerConfig { ID = "option SHOOT", H = 3, Display = "SHOOT", TargetL = 1, Mode = LevelChangerConfig.Modes.OneDirection },
                new LevelChangerConfig { ID = "option SLASH", H = 3, Display = "SLASH", TargetL = 2, Mode = LevelChangerConfig.Modes.OneDirection },
                new LevelChangerConfig { ID = "option STOMP", H = 3, Display = "STOMP", TargetL = 3, Mode = LevelChangerConfig.Modes.OneDirection },
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
                new AttackSelectorConfig { ID = "AS", GoName = m["phase 1 GoName"], FsmName = m["phase 1 FsmName"],
                    MapEvents = new() {
                        { "QUAKE", "DIVE" },
                    },
                }, // AS
                new LevelChangerConfig { ID = "manual phase changer", Display = "Advance to Phase 2", TargetL = 1, Mode = LevelChangerConfig.Modes.OneTime }, // manual phase changer
                new GoKillerConfig { ID = "boss killer", L = 1 }, // boss killer
                new AutoLevelChangerConfig { ID = "auto level changer", L = 0, H = 1, GoName = m["phase 2 GoName"], FsmName = m["phase 2 FsmName"], OnEnterState = m["phase 2 OnEnterState"], TargetL = 2 }, // auto level changer
                new LevelChangerConfig { ID = "infinite dive option", L = 2, H = 3, Display = "Extra: Infinite DIVE", TargetL = 3, Mode = LevelChangerConfig.Modes.Bidirection }, // infinite dive option
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

        private static ModuleConfig[] GetGruzMotherConfigs()
        {
            return new ModuleConfig[] {
                new AttackSelectorConfig { GoName = "_Enemies/Giant Fly", FsmName = "Big Fly Control", StateName = "Super Choose" },
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Slams In A Row", 0),
                        new("Charges In A Row", 0),
                    },
                },
            };
        }

        private static ModuleConfig[] GetFalseKnightAndFailedChampionConfigs(bool v)
        {
            var falseKnight = new Dictionary<string, string>
            {
                { "GoName", "False Knight Dream" },
            };
            var failedChampion = new Dictionary<string, string>
            {
                { "GoName", "Battle Scene/False Knight New" },
            };
            var boss = v ? failedChampion : falseKnight;

            return new ModuleConfig[] {
                new AttackSelectorConfig { GoName = boss["GoName"], FsmName = "FalseyControl", IgnoreEvents = new() { "RUN", "TEST" } },
                new VariableSetterConfig {
                    IntVariables = new KeyValuePair<string, int>[] {
                        new("JA In A Row", 0), // row count for JUMP ATTACK
                        new("Slam In A Row", 0), // row count for SMASH
                        new("Jump Count", 0), // row check for JUMP
                        new("Stunned Amount", -9), // Needed for JUMP. See Determine Jump-1.
                    },
                },
            };
        }

        private static ModuleConfig[] GetBroodingMawlekConfigs()
        {
            return new ModuleConfig[]
            {
                new AttackSelectorConfig { GoName = "Battle Scene/Mawlek Body", FsmName = "Mawlek Control", StateName = "Super Select",
                    MapEvents = new() {
                        { "TRUE", "SPIT" },
                        { "FALSE", "JUMP" },
                    },
                },
                new VariableSetterConfig
                {
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Jumps In A Row", 0),
                        new("Spits In A Row", 0),
                    },
                },
            };
        }

        private static ModuleConfig[] GetWatcherKnightConfigs(string number1to6)
        {
            return new ModuleConfig[]
            {
                new SingleFsmModuleConfig { GoName = "Battle Control/Black Knight " + number1to6, FsmName = "Black Knight" },
                new LabelConfig { ID = $"WK{number1to6} label all", L = 0, Display = "Current attack: All" },
                new LabelConfig { ID = $"WK{number1to6} label SLASH", L = 1, Display = "Current attack: SLASH" },
                new LabelConfig { ID = $"WK{number1to6} label CHARGE ROLL", L = 2, Display = "Current attack: CHARGE ROLL" },
                new LabelConfig { ID = $"WK{number1to6} label JUMP ROLL", L = 3, Display = "Current attack: JUMP ROLL" },
                new LevelChangerConfig { ID = $"WK{number1to6} option all", H = 3, Display = "All", TargetL = 0, Mode = LevelChangerConfig.Modes.OneDirection },
                new LevelChangerConfig { ID = $"WK{number1to6} option SLASH", H = 3, Display = "SLASH (JUMP ROLL when overhead)", TargetL = 1, Mode = LevelChangerConfig.Modes.OneDirection },
                new LevelChangerConfig { ID = $"WK{number1to6} option CHARGE ROLL", H = 3, Display = "CHARGE ROLL", TargetL = 2, Mode = LevelChangerConfig.Modes.OneDirection },
                new LevelChangerConfig { ID = $"WK{number1to6} option JUMP ROLL", H = 3, Display = "JUMP ROLL", TargetL = 3, Mode = LevelChangerConfig.Modes.OneDirection },
                // SLASH
                new EventEmitterConfig { ID = $"WK{number1to6} SLASH in range double", L = 1, StateName = "In Range Double", ActionType = typeof(SendRandomEventV2), EventName = "SLASH" },
                new EventEmitterConfig { ID = $"WK{number1to6} SLASH in range choice", L = 1, StateName = "In Range Choice", ActionType = typeof(SendRandomEventV2), EventName = "SLASH" },
                new EventEmitterConfig { ID = $"WK{number1to6} SLASH move choice", L = 1, StateName = "Move Choice", ActionType = typeof(SendRandomEventV2), EventName = "SLASH" },
                new ActionEnablerConfig { ID = $"WK{number1to6} SLASH run to action 6", L = 1, StateName = "Run To", ActionType = typeof(WaitRandom), ToEnabled = false },
                new ActionEnablerConfig { ID = $"WK{number1to6} SLASH run to action 7", L = 1, StateName = "Run To", ActionType = typeof(WaitRandom), IndexDelta = 1, ToEnabled = false },
                // CHARGE ROLL
                new EventEmitterConfig { ID = $"WK{number1to6} CHARGE ROLL in range double", L = 2, StateName = "In Range Double", ActionType = typeof(SendRandomEventV2), EventName = "CHARGE ROLL" },
                new EventEmitterConfig { ID = $"WK{number1to6} CHARGE ROLL in range choice", L = 2, StateName = "In Range Choice", ActionType = typeof(SendRandomEventV2), EventName = "CHARGE ROLL" },
                new EventEmitterConfig { ID = $"WK{number1to6} CHARGE ROLL move choice", L = 2, StateName = "Move Choice", ActionType = typeof(SendRandomEventV2), EventName = "CHARGE ROLL" },
                new ActionEnablerConfig { ID = $"WK{number1to6} CHARGE ROLL run to action 3", L = 2, StateName = "Run To", ActionType = typeof(BoolTestMulti), ToEnabled = false },
                new ActionEnablerConfig { ID = $"WK{number1to6} CHARGE ROLL run to action 4", L = 2, StateName = "Run To", ActionType = typeof(BoolTestMulti), IndexDelta = 1, ToEnabled = false },
                new ActionEnablerConfig { ID = $"WK{number1to6} CHARGE ROLL run to action 5", L = 2, StateName = "Run To", ActionType = typeof(BoolTest), ToEnabled = false },
                new ActionEnablerConfig { ID = $"WK{number1to6} CHARGE ROLL run to action 7", L = 2, StateName = "Run To", ActionType = typeof(WaitRandom), IndexDelta = 1, ToEnabled = false },
                // JUMP ROLL
                new EventEmitterConfig { ID = $"WK{number1to6} JUMP ROLL in range double", L = 3, StateName = "In Range Double", ActionType = typeof(SendRandomEventV2), EventName = "JUMP ROLL" },
                new EventEmitterConfig { ID = $"WK{number1to6} JUMP ROLL in range choice", L = 3, StateName = "In Range Choice", ActionType = typeof(SendRandomEventV2), EventName = "JUMP ROLL" },
                new EventEmitterConfig { ID = $"WK{number1to6} JUMP ROLL move choice", L = 3, StateName = "Move Choice", ActionType = typeof(SendRandomEventV2), EventName = "JUMP ROLL" },
                new ActionEnablerConfig { ID = $"WK{number1to6} JUMP ROLL run to action 5", L = 3, StateName = "Run To", ActionType = typeof(BoolTest), ToEnabled = false },
                new ActionEnablerConfig { ID = $"WK{number1to6} JUMP ROOL run to action 6", L = 3, StateName = "Run To", ActionType = typeof(WaitRandom), ToEnabled = false },
            };
        }

        private static ModuleConfig[] GetTheCollectorConfigs()
        {
            return new ModuleConfig[] {
                new SingleFsmModuleConfig { GoName = "Jar Collector", FsmName = "Control" },
                // Enter Phase 2
                new LabelConfig { H = 1, Display = "Extra: Entered Phase 2" },
                new VariableSetterConfig { StateName = "Summon?", ActionType = typeof(GetTagCount),
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Enemies Max", 8),
                        new("Spawn Min", 2),
                        new("Spawn Max", 3),
                        new("Spawns Total", 3),
                    },
                    FloatVariables = new KeyValuePair<string, float>[]
                    {
                        new("Resummon Pause", 0.35f),
                        new("Spawn Recover Time", 0.75f),
                    },
                },
                new EventEmitterConfig { FsmName = "Damage Control", StateName = "Check", ActionType = typeof(BoolTest), EventName = "DECREMENT" },
                // Spawn choices
                new AttackSelectorConfig { H = 1, FsmName = "Control", StateName = "Phase 1", IgnoreEvents = new() { "NONE" } },
                // More Jars
                new LevelChangerConfig { H = 1, Display = "Extra: More Jars", TargetL = 1, Mode = LevelChangerConfig.Modes.Bidirection },
                new VariableSetterConfig { L = 1, StateName = "Summon?", ActionType = typeof(GetTagCount),
                    IntVariables = new KeyValuePair<string, int>[]
                    {
                        new("Enemies Max", 50),
                        new("Spawn Min", 8),
                        new("Spawn Max", 12),
                    },
                },
                new ActionEnablerConfig { L = 1, StateName = "Summon?", ActionType = typeof(IntCompare), ToEnabled = false }, // never cancel summon
            };
        }

        private static ModuleConfig[] GetDungDefenderAndWhiteDefenderConfigs(bool v)
        {
            var dungDefender = new Dictionary<string, string>
            {
                { "GoName", "Dung Defender" },
            };
            var whiteDefender = new Dictionary<string, string>
            {
                { "GoName", "White Defender" },
            };
            var boss = v ? whiteDefender : dungDefender;

            var configs = new List<ModuleConfig>
            {
                new AttackSelectorConfig { L = 0, H = 1, GoName = boss["GoName"], FsmName = "Dung Defender", IgnoreEvents = new() { "GROUND SLAM" } },
                new LevelChangerConfig { L = 0, H = 1, Display = "Extra: Trim ROLL JUMP", TargetL = 1, Mode = LevelChangerConfig.Modes.Bidirection },
                // trim head throw
                new TransitionRewirerConfig { L = 1, StateName = "RJ Set", EventName = "FINISHED", ToState = "RJ Antic" },
                // make sure roll speed is 12
                new VariableSetterConfig { L = 1, StateName = "RJ Antic", ActionType = typeof(FaceObject),
                    FloatVariables = new KeyValuePair<string, float>[]
                    {
                        new("Throw Speed", 12f),
                    },
                },
            };

            if (boss == whiteDefender)
            {
                // trim tail dive
                configs.Add(new EventEmitterConfig { L = 1, StateName = "Air Dive?", ActionType = typeof(SendRandomEvent), IndexDelta = 2, EventName = "FINISHED" });
            }

            return configs.ToArray();
        }
    }
}
