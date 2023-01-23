using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BossAttacks.Modules;
using BossAttacks.Utils;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public override string GetVersion() => version.Value + "-DEBUG";
#else
        public override string GetVersion() => version.Value;
#endif
        private static readonly Lazy<string> version = new(() =>
        {
            Assembly asm = typeof(BossAttacks).Assembly;
            string ver = asm.GetName().Version.ToString();
            using var sha = SHA256.Create();
            using FileStream stream = File.OpenRead(asm.Location);
            byte[] hashBytes = sha.ComputeHash(stream);
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            return $"{ver}-{hash.Substring(0, 6)}";
        });

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log($"Initializing mod {GetVersion()}");

            // logger
            Instance = this;
            LoggingUtils.LoggingFunction = this.Log;
            LoggingUtils.LogLevel = LogLevel.Fine;
            LoggingUtils.FilterFunction = LoggingUtils.DontRepeatWithin1s;

            // display
            ModDisplay.Instance = new ModDisplay();

            // hooks
            USceneManager.activeSceneChanged += SceneManager_OnActiveSceneChanged;
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;

            // input overrides
            KeyboardOverride.Load();

            // debugger
            Debugger.Instance = new Debugger();
            Debugger.Instance.Load();

            Log("Initialized mod");
        }

        private void ModHooks_HeroUpdateHook()
        {
            if (ModuleManager.Instance == null)
            {
                return;
            }

            if (KeyboardOverride.GetKeyDown(KeyCode.Alpha0))
            {
                UpdateOptionDisplay();
            }

            int i = 0;
            // Order MATTERS
            foreach (var opt in ModuleManager.Instance.Options)
            {
                if (opt.Interactive && KeyboardOverride.GetKeyDown(KeyCode.Alpha0 + ++i))
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
            this.LogMod($"SceneManager_OnActiveSceneChanged: {from.name} -> {to.name}");

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
            // display
            if (ModDisplay.Instance != null)
            {
                ModDisplay.Instance.Destroy();
                ModDisplay.Instance = null;
            }

            // hooks
            USceneManager.activeSceneChanged -= SceneManager_OnActiveSceneChanged;
            ModHooks.HeroUpdateHook -= ModHooks_HeroUpdateHook;

            // input overrides
            KeyboardOverride.Unload();

            // debugger
            if (Debugger.Instance != null)
            {
                Debugger.Instance.Unload();
                Debugger.Instance = null;
            }

            // objeects
            if (ModuleManager.Instance != null)
            {
                ModuleManager.Instance.Unload();
                ModuleManager.Instance = null;
            }

            if (Debugger.Instance != null)
            {
                Debugger.Instance.Unload();
                Debugger.Instance = null;
            }
        }

        ///
        /// IGlobalSettings<GlobalData>
        ///
        public void OnLoadGlobal(GlobalData data) => GlobalData = data;
        public GlobalData OnSaveGlobal() => GlobalData;
    }
}
