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

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

            new Debugger().Load();

            Log("Initialized mod");
        }

        private void UpdateOptionDisplay()
        {
            var toOnOff = (bool b) =>
            {
                return b ? "On" : "Off";
            };

            var sb = new StringBuilder();
            int i = 0;
            foreach (var m in ModuleManager.Instance.GetLoadedModules().OrderBy(m => m.Name))
            {
                var options = m.BooleanOptions.OrderBy(kv => kv.Key).ToArray();

                if (options.Length == 0)
                {
                    continue;
                }
                else if (options.Length == 1)
                {
                    sb.AppendLine($"\"{++i}\" - {options[0].Key}: {toOnOff(options[0].Value.Value)}");
                }
                else
                {
                    sb.AppendLine($"{m.Name}:");
                    foreach (var o in options)
                    {
                        sb.AppendLine($"    \"{++i}\" - {o.Key}: {toOnOff(o.Value.Value)}");
                    }
                }
            }
            this.LogModDebug(sb.ToString());

            ModDisplay.Instance.Display(sb.ToString());
        }

        private void UpdateOptionDisplayWithError()
        {
            ModDisplay.Instance.Notify("Cannot make change. Most commonly because there needs to be one active attack.");
        }

        private void SceneManager_activeSceneChanged(Scene from, Scene to)
        {
            ModuleManager.Instance.Load(to);
            UpdateOptionDisplay();

            foreach (var m in ModuleManager.Instance.GetLoadedModules())
            {
                foreach (var o in m.BooleanOptions.Values)
                {
                    o.OnSet.Add(_ => UpdateOptionDisplay());
                    o.OnCannotSet.Add(_ => UpdateOptionDisplayWithError());
                }
            }
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
