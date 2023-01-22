using System;
using System.Collections.Generic;
using BossAttacks.Utils;
using InControl;
using Modding;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace BossAttacks
{
    internal class Debugger2
    {
        public static Debugger2 Instance;

        public void Load()
        {
            ModHooks.HeroUpdateHook += OnHeroUpdate;
        }

        public void Unload()
        {
            ModHooks.HeroUpdateHook -= OnHeroUpdate;
        }

        private void OnHeroUpdate()
        {
            if (InputUtils.GetKeyDown(KeyCode.J))
            {
                this.LogModTEMP("J is pressed!");
            }
        }
    }
}
