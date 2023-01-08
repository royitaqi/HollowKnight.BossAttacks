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

namespace BossAttacks
{
    public class BossAttacks : Mod, ICustomMenuMod
    {
        internal static BossAttacks Instance;

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

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

            new Debugger().Load();
            ModDisplay.Instance.EnableDebugger();

            Log("Initialized mod");
        }

        internal void UpdateOptionDisplay()
        {
            var sb = new StringBuilder();
            int i = 0;
            foreach (var m in ModuleManager.Instance.GetLoadedModules().OrderBy(m => m.Name))
            {
                var options = m.GetBooleanOptions().OrderBy(kv => kv.Key).ToArray();

                if (options.Length == 0)
                {
                    continue;
                }
                else if (options.Length == 1)
                {
                    sb.AppendLine($"\"{++i}\" - {options[0].Key}: {options[0].Value.Value}");
                }
                else
                {
                    sb.AppendLine($"{m.Name}:");
                    foreach (var o in options)
                    {
                        sb.AppendLine($"    \"{++i}\" - {o.Key}: {o.Value.Value}");
                    }
                }
            }
            this.LogModDebug(sb.ToString());

            ModDisplay.Instance.Display(sb.ToString());
        }

        private void SceneManager_activeSceneChanged(Scene from, Scene to)
        {
            ModuleManager.Instance.Load(to);
            UpdateOptionDisplay();
        }

        ///
        /// ICustomMenuMod
        ///
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggle) => ModMenu.GetMenu(modListMenu, toggle);
        public bool ToggleButtonInsideMenu => false;
    }
}