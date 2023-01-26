using System;
using System.Collections;
using System.Collections.Generic;
using BossAttacks.E2eTests;
using BossAttacks.Utils;
using InControl;
using Modding;
using MonoMod.RuntimeDetour;
using SFCore.Utils;
using UnityEngine;

namespace BossAttacks
{
    internal class Debugger
    {
        public static Debugger Instance;

        public void Load()
        {
            ModHooks.HeroUpdateHook += EachHeroUpdate;
            On.PlayMakerFSM.Start += EachFsmAtStart;
        }

        public void Unload()
        {
            ModHooks.HeroUpdateHook -= EachHeroUpdate;
            On.PlayMakerFSM.Start -= EachFsmAtStart;
        }

        private void EachFsmAtStart(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self)
        {
            if (self is
                {
                    name: "Knight",
                    FsmName: "Dream Nail"
                })
            {
                // do something for this FSM
            }
        }

        private void EachHeroUpdate()
        {
            var leftKey = KeyCode.Pause;
            var rightKey = KeyCode.Pause;
            var attackKey = KeyCode.Pause;

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                var t = new BatchTest();
                t.RunFullTest();
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                var t = new TestPureVessel();
                t.RunFullTest();
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                var t = new TestSoulTyrant();
                t.RunFullTest();
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                var t = new TestGruzMother();
                t.RunFullTest();
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
