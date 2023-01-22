using System;
using System.Collections.Generic;
using BossAttacks.Utils;
using InControl;
using Modding;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace BossAttacks
{
    internal class Debugger
    {
        public static Debugger Instance;

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
            var leftKey = KeyCode.Alpha1;
            var rightKey = KeyCode.Alpha2;
            var attackKey = KeyCode.Alpha3;

            if (Input.GetKeyDown(leftKey))
            {
                InputUtils.Load();
                InputUtils.PressControllerDirection("left");
                this.LogModTEMP($"fake left: down");
            }
            if (Input.GetKeyUp(leftKey))
            {
                InputUtils.Load();
                InputUtils.ReleaseControllerDirection("left");
                this.LogModTEMP($"fake left: up");
            }
            if (Input.GetKeyDown(rightKey))
            {
                InputUtils.Load();
                InputUtils.PressControllerDirection("right");
                this.LogModTEMP($"fake right: down");
            }
            if (Input.GetKeyUp(rightKey))
            {
                InputUtils.Load();
                InputUtils.ReleaseControllerDirection("right");
                this.LogModTEMP($"fake right: up");
            }
            if (Input.GetKeyDown(attackKey))
            {
                InputUtils.Load();
                InputUtils.PressControllerButton("attack");
                this.LogModTEMP($"fake attack: down");
            }
            if (Input.GetKeyUp(attackKey))
            {
                InputUtils.Load();
                InputUtils.ReleaseControllerButton("attack");
                this.LogModTEMP($"fake attack: up");
            }
        }
    }
}
