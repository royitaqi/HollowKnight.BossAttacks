using System.Collections.Generic;
using System.Linq;
using BossAttacks.Modules;
using BossAttacks.Utils;
using Modding;
using Osmi.Utils;
using Satchel.BetterMenus;
using UnityEngine;
using UnityEngine.UI;
using MenuButton = Satchel.BetterMenus.MenuButton;

namespace BossAttacks
{
    public static class ModMenu
    {
        public static MenuScreen GetMenu(MenuScreen modListMenu, ModToggleDelegates? toggle)
        {
            // Create the mod.
            // Note that we don't cache this menu instance, because the localization setting may have been updated between two menu invocations.
            MenuRef = PrepareMenu();
            MenuRef.SetMenuButtonNameAndDesc(BossAttacks.Instance, "Boss Attacks");
            var menuScreen = MenuRef.GetMenuScreen(modListMenu);

            MenuRef.OnBuilt += MenuRef_OnBuilt;
            MenuRef.OnReflow += MenuRef_OnReflow;
            MenuRef.OnUpdate += MenuRef_OnUpdate;
            MenuRef.OnVisibilityChange += Menu_OnVisibilityChange;

            return menuScreen;
        }

        private static void MenuRef_OnUpdate(object sender, UpdateEventArgs e)
        {
            MenuRef.LogModTEMP("MenuRef_OnUpdate");
            //MenuRef.AddElement(new MenuButton(
            //    "DEBUG MenuRef_OnUpdate",
            //    "",
            //    _ => { }
            //));
        }

        private static void MenuRef_OnReflow(object sender, ReflowEventArgs e)
        {
            MenuRef.LogModTEMP("MenuRef_OnReflow");
            //MenuRef.AddElement(new MenuButton(
            //    "DEBUG MenuRef_OnReflow",
            //    "",
            //    _ => { }
            //));
        }

        private static void MenuRef_OnBuilt(object sender, ContainerBuiltEventArgs e)
        {
            MenuRef.LogModTEMP("MenuRef_OnBuilt");
        }

        private static void Menu_OnVisibilityChange(object sender, VisibilityChangeEventArgs e)
        {
            MenuRef.LogModTEMP("Menu_OnVisibilityChange");
        }

        private static Menu PrepareMenu()
        {
            MenuRef.LogModTEMP("PrepareMenu");
            var moduleOptions = ModuleManager.Instance.GetLoadedModules().SelectMany(
                m => m.GetBooleanOptions().Select(
                    kvp => new HorizontalOption(
                        m.Name + " - " + kvp.Key,
                        "",
                        new[] { "Off", "On" },
                        selectedIndex => {
                            kvp.Value.Value = selectedIndex == 1;
                        },
                        () => kvp.Value.Value ? 1 : 0
                    )
                )
            ).ToArray();

            var elements = new List<Element>();
            elements.AddRange(moduleOptions);
            elements.Add(new MenuButton(
                "DEBUG Button",
                "",
                _ => { }
            ));

            return new Menu("Boss Attack", elements.ToArray());
        }

        public static Menu MenuRef;
    }
}
