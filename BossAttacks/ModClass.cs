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
    public class BossAttacks : Mod, ICustomMenuMod, IGlobalSettings<GlobalData>
    {
        internal static BossAttacks Instance;
        internal GlobalData GlobalData { get; set; } = new GlobalData();

        ///
        /// Mod
        ///
        public override string GetVersion() => VersionUtil.GetVersion<BossAttacks>();

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing mod");

            Instance = this;
            ModuleManager.Instance = new ModuleManager();
            ModDisplay.Instance = new ModDisplay();

            USceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;

            new Debugger().Load();

            Log("Initialized mod");
        }

        private void ModHooks_HeroUpdateHook()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                UpdateOptionDisplay(USceneManager.GetActiveScene());
            }

            int i = 0;
            // Order MATTERS
            foreach (var m in ModuleManager.Instance.GetLoadedModules().OrderBy(m => m.Name))
            {
                foreach (var kv in m.BooleanOptions.OrderBy(kv => kv.Key))
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + ++i))
                    {
                        this.LogModDebug($"Changing module {m.Name} option {kv.Key} from {kv.Value.Value} to {!kv.Value.Value}");
                        kv.Value.Value = !kv.Value.Value;
                    }
                }
            }
        }

        private void SceneManager_activeSceneChanged(Scene from, Scene to)
        {
            ModuleManager.Instance.Load(to);
            UpdateOptionDisplay(to);

            // Order doesn't matter
            foreach (var m in ModuleManager.Instance.GetLoadedModules())
            {
                foreach (var o in m.BooleanOptions.Values)
                {
                    o.OnSet.Add(_ => UpdateOptionDisplay(to));
                    o.OnCannotSet.Add(_ => UpdateOptionDisplayWithError());
                }
            }
        }

        private void UpdateOptionDisplay(Scene to)
        {
            var toOnOff = (bool b) =>
            {
                return b ? "[ ✓ ]" : "[     ]";
            };

            var sb = new StringBuilder();
            int i = 0;
            // Order MATTERS
            foreach (var m in ModuleManager.Instance.GetLoadedModules().OrderBy(m => m.Name))
            {
                var options = m.BooleanOptions.OrderBy(kv => kv.Key).ToArray();

                if (options.Length == 0)
                {
                    continue;
                }
                else if (options.Length == 1)
                {
                    // "1" - [   ] - DIVE IN
                    // "2" - [ ✓ ] - DOLPHIN
                    // "3" - [   ] - GROUND SLAM
                    // "4" - [ ✓ ] - ROLL JUMP
                    sb.AppendLine($"\"{++i}\" - {toOnOff(options[0].Value.Value)} - {options[0].Key}");
                }
                else
                {
                    sb.AppendLine($"{m.Name}:");
                    foreach (var o in options)
                    {
                        sb.AppendLine($"        \"{++i}\" - {toOnOff(o.Value.Value)} - {o.Key}");
                    }
                }
            }
            this.LogModDebug(sb.ToString());

            if (i == 0)
            {
                // No option was loaded. Display an introduction text.
                if (GodhomeUtils.SceneToBoss.ContainsKey(to.name))
                {
                    ModDisplay.Instance.Display("This boss is not supported yet.");
                }
                else
                {
                    ModDisplay.Instance.Display("Enter a boss fight to see boss attacks.");
                }
            }
            else
            {
                // Some option was loaded. Display the options.
                ModDisplay.Instance.Display(sb.ToString());
            }
        }

        private void UpdateOptionDisplayWithError()
        {
            ModDisplay.Instance.Notify("Cannot make change. Most commonly because there needs to be one active attack.");
        }

        ///
        /// ICustomMenuMod
        ///
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggle) => ModMenu.GetMenu(modListMenu, toggle);
        public bool ToggleButtonInsideMenu => false;

        ///
        /// IGlobalSettings<GlobalData>
        ///
        public void OnLoadGlobal(GlobalData data) => GlobalData = data;
        public GlobalData OnSaveGlobal() => GlobalData;
    }
}
