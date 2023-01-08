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

            return menuScreen;
        }

        private static Menu PrepareMenu()
        {
            MenuRef.LogModTEMP("PrepareMenu");
            return new Menu("Boss Attacks", new Element[]
            {
                new HorizontalOption(
                    "Display",
                    "",
                    new []{ "Off", "Auto-hide", "On" },
                    selectedIndex => {
                        BossAttacks.Instance.GlobalData.DisplayMode = selectedIndex;
                        ModDisplay.Instance.Update();
                    },
                    () => BossAttacks.Instance.GlobalData.DisplayMode
                ),
#if DEBUG
                new MenuButton(
                    "DEBUG: Display",
                    "",
                    _ => ModDisplay.Instance.EnableDebugger()
                ),
#endif
            });
        }

        public static Menu MenuRef;
    }
}
