using System;
using System.Collections;
using System.Collections.Generic;
using BossAttacks.Modules;
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

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

            new Debugger().Load();

            Log("Initialized mod");
        }

        private void SceneManager_activeSceneChanged(Scene from, Scene to)
        {
            ModuleManager.Instance.Load(to);
        }

        ///
        /// ICustomMenuMod
        ///
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggle) => ModMenu.GetMenu(modListMenu, toggle);
        public bool ToggleButtonInsideMenu => false;
    }
}