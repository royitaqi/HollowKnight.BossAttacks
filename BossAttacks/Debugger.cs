using System;
using System.Collections.Generic;
using BossAttacks.E2eTests;
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

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                new TestSoulTyrant().RunFullTest();
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                HeroController.instance.gameObject.transform.position += new Vector3(3, 3);
            }

            if (Input.GetKeyDown(leftKey))
            {
                InputUtils.Load();
                InputUtils.PressDirection("left");
                this.LogModTEMP($"fake left: down");
            }
            if (Input.GetKeyUp(leftKey))
            {
                InputUtils.Load();
                InputUtils.ReleaseDirection("left");
                this.LogModTEMP($"fake left: up");
            }
            if (Input.GetKeyDown(rightKey))
            {
                InputUtils.Load();
                InputUtils.PressDirection("right");
                this.LogModTEMP($"fake right: down");
            }
            if (Input.GetKeyUp(rightKey))
            {
                InputUtils.Load();
                InputUtils.ReleaseDirection("right");
                this.LogModTEMP($"fake right: up");
            }
            if (Input.GetKeyDown(attackKey))
            {
                InputUtils.Load();
                InputUtils.PressButton("attack");
                this.LogModTEMP($"fake attack: down");

                InputUtils.PressKey(KeyCode.J);
            }
            if (Input.GetKeyUp(attackKey))
            {
                InputUtils.Load();
                InputUtils.ReleaseButton("attack");
                this.LogModTEMP($"fake attack: up");
            }
        }
    }
}
