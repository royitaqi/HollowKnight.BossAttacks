using Modding;
using Satchel.BetterMenus;
using MenuButton = Satchel.BetterMenus.MenuButton;

namespace BossAttacks
{
    public sealed partial class BossAttacks : ICustomMenuMod
    {
        public bool ToggleButtonInsideMenu => true;

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggle)
        {
            _menuRef = new Menu("Boss Attacks", new Element[]
            {
                toggle!.Value.CreateToggle(
                    "Mod",
                    "Enter a new fight to apply change"
                ),
                new HorizontalOption(
                    "Fade Bottom Left Display",
                    "Press \"0\" on main keyboard to bring up the display",
                    new []{ "Off", "On" },
                    selectedIndex => {
                        BossAttacks.Instance.GlobalData.FadeDisplay = selectedIndex == 1;
                        ModDisplay.Instance?.Update();
                    },
                    () => BossAttacks.Instance.GlobalData.FadeDisplay ? 1 : 0
                ),
#if DEBUG
                new MenuButton(
                    "DEBUG: Move & Resize Display",
                    "",
                    _ => ModDisplay.Instance?.EnableDebugger()
                ),
#endif
            });

            _menuRef.SetMenuButtonNameAndDesc(BossAttacks.Instance, "Boss Attacks");
            return _menuRef.GetMenuScreen(modListMenu);
        }

        private Menu _menuRef;
    }
}
