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
#if (DEBUG)
    internal class Debugger
    {
        public static Debugger Instance;

        public void Load()
        {
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
            On.PlayMakerFSM.Start += PlayMakerFSM_Start;
        }

        public void Unload()
        {
            ModHooks.HeroUpdateHook -= ModHooks_HeroUpdateHook;
            On.PlayMakerFSM.Start -= PlayMakerFSM_Start;
            _coroutineInProgress = false;
        }

        private void ModHooks_HeroUpdateHook()
        {
            if (!_coroutineInProgress && HeroController.instance != null)
            {
                _coroutineInProgress = true;
                HeroController.instance.StartCoroutine(CoroutineLoop());
            }

            EachHeroUpdate();
        }

        private void PlayMakerFSM_Start(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self)
        {
            orig(self);

            EachFsmAtStart(self);
        }

        private IEnumerator CoroutineLoop()
        {
            while (_coroutineInProgress)
            {
                EachCoroutineCall();
                yield return new WaitForSeconds(1f);
            }
        }
        private bool _coroutineInProgress;

        #region Playground
        private void EachCoroutineCall()
        {
            this.LogModDebug("EachCoroutineCall()");
        }

        private void EachFsmAtStart(PlayMakerFSM self)
        {
            // custom logic
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
        #endregion Playground
    }
#endif
}
