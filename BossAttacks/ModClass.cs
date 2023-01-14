﻿using System;
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

            USceneManager.activeSceneChanged += SceneManager_OnActiveSceneChanged;
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;

            new Debugger().Load();

            Log("Initialized mod");
        }

        private void ModHooks_HeroUpdateHook()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                UpdateOptionDisplay();
            }

            int i = 0;
            // Order MATTERS
            foreach (var opt in ModuleManager.Instance.GetOptions().ToArray())
            {
                if (opt.Interactive && Input.GetKeyDown(KeyCode.Alpha0 + ++i))
                {
                    this.LogModDebug($"User is changing option {opt.Display}");
                    opt.Interact();
                    break;
                }
            }
        }

        private void SceneManager_OnActiveSceneChanged(Scene from, Scene to)
        {
            if (!ModuleManager.IsBossScene(to))
            {
                ModDisplay.Instance.Display("Enter a boss fight to see boss attacks.");
                return;
            }
            if (!ModuleManager.IsSupportedBossScene(to))
            {
                ModDisplay.Instance.Display("This boss is not supported.");
                return;
            }
            // Now it's a supported boss scene

            ModuleManager.Instance.OptionsChanged += () =>
            {
                UpdateOptionDisplay();
                foreach (var opt in ModuleManager.Instance.GetOptions())
                {
                    if (opt.Interactive)
                    {
                        opt.Interacted += UpdateOptionDisplay;
                    }
                }
            };
            ModuleManager.Instance.Load(to);
        }

        private void UpdateOptionDisplay()
        {
            var sb = new StringBuilder();
            int i = 0;
            foreach (var opt in ModuleManager.Instance.GetOptions())
            {
                string hotkey = opt.Interactive ? $"\"{++i}\" - " : "";
                sb.AppendLine(hotkey + opt.Display);
            }
            this.LogModDebug("Updating option display");
            //this.LogModFine(sb.ToString());

            ModDisplay.Instance.Display(sb.ToString());
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
