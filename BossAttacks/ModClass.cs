using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BossAttacks.Modules;
using BossAttacks.Utils;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vasi;
using UObject = UnityEngine.Object;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace BossAttacks
{
    public sealed partial class BossAttacks : Mod, ITogglableMod, IGlobalSettings<GlobalData>
    {
        internal static BossAttacks Instance;
        internal GlobalData GlobalData { get; set; } = new GlobalData();

        ///
        /// Mod
        ///
#if (DEBUG)
        public override string GetVersion() => VersionUtil.GetVersion<BossAttacks>() + "-DEBUG";
#else
        public override string GetVersion() => VersionUtil.GetVersion<BossAttacks>();
#endif

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing mod");

            Instance = this;
            LoggingUtils.LoggingFunction = this.Log;

            ModDisplay.Instance = new ModDisplay();

            USceneManager.activeSceneChanged += SceneManager_OnActiveSceneChanged;
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;

            new Debugger().Load();

            Log("Initialized mod");
        }

        private void ModHooks_HeroUpdateHook()
        {
            if (ModuleManager.Instance == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                UpdateOptionDisplay();
            }

            int i = 0;
            // Order MATTERS
            foreach (var opt in ModuleManager.Instance.Options)
            {
                if (opt.Interactive && Input.GetKeyDown(KeyCode.Alpha0 + ++i))
                {
                    if (ModAssert.DebugBuild(i <= 9, $"Cannot have more than 9 interactive options (got {i}: {opt.Display})"))
                    {
                        continue;
                    }
                    this.LogModDebug($"User is changing option {opt.Display}");
                    opt.Interact();
                    break;
                }
            }
        }

        private void SceneManager_OnActiveSceneChanged(Scene from, Scene to)
        {
            if (ModuleManager.Instance != null)
            {
                ModuleManager.Instance.Unload();
                ModuleManager.Instance = null;
            }

            if (!ModuleManager.IsBossScene(to) || (from != null && from.name != "GG_Workshop"))
            {
                ModDisplay.Instance.Display("Enter a boss fight in Hall of Gods to see boss attacks.");
                return;
            }
            if (!ModuleManager.IsSupportedBossScene(to))
            {
                ModDisplay.Instance.Display("This boss is not supported.");
                return;
            }

            // Now it's a supported boss scene in HoG
            ModuleManager.Instance = new ModuleManager();
            ModuleManager.Instance.OptionsChanged += () =>
            {
                UpdateOptionDisplay();
                foreach (var opt in ModuleManager.Instance.Options)
                {
                    if (opt.Interactive)
                    {
                        opt.Interacted -= UpdateOptionDisplay;
                        opt.Interacted += UpdateOptionDisplay;
                    }
                }
            };
            ModuleManager.Instance.Load(to);
        }

        private void UpdateOptionDisplay()
        {
            if (ModuleManager.Instance == null)
            {
                return;
            }

            var sb = new StringBuilder();
            int i = 0;
            foreach (var opt in ModuleManager.Instance.Options)
            {
                string hotkey = opt.Interactive ? $"\"{++i}\" - " : "";
                if (ModAssert.DebugBuild(i <= 9, $"Cannot have more than 9 interactive options (got {i}: {opt.Display})"))
                {
                    continue;
                }
                sb.AppendLine(hotkey + opt.Display);
            }
            this.LogModDebug("Updating option display");

            ModDisplay.Instance.Display(sb.ToString());
        }

        ///
        /// ITogglableMod
        ///
        public void Unload()
        {
            USceneManager.activeSceneChanged -= SceneManager_OnActiveSceneChanged;
            ModHooks.HeroUpdateHook -= ModHooks_HeroUpdateHook;

            if (ModuleManager.Instance != null)
            {
                ModuleManager.Instance.Unload();
                ModuleManager.Instance = null;
            }

            if (ModDisplay.Instance != null)
            {
                ModDisplay.Instance.Destroy();
                ModDisplay.Instance = null;
            }
        }

        ///
        /// IGlobalSettings<GlobalData>
        ///
        public void OnLoadGlobal(GlobalData data) => GlobalData = data;
        public GlobalData OnSaveGlobal() => GlobalData;
    }
}
