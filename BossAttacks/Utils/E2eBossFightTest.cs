using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Modding;
using UnityEngine;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace BossAttacks.Utils
{
    /**
     * Utility functions specific for boss fights.
     */
    internal abstract class E2eBossFightTest : E2eTest
    {
        protected abstract string BossScene { get; }
        protected virtual Vector3 StatuePos => GodhomeUtils.SceneToStatue[BossScene].StatuePos;
        protected virtual string ReturnDoor => GodhomeUtils.SceneToStatue[BossScene].ReturnDoor;
        protected virtual int ChallengeLevel => 0;

        protected abstract IEnumerator BossFightScript();

        protected override IEnumerator Script()
        {
            _fightInProgress = true;
            InParallel(SetInvincibility());

            TestCase("wait for fight and modules to load");
            yield return EnterFightViaStatueGo();
            yield return ExpectLog("[ModuleManager] Level is now 0", 10);

            // Run actual boss fight test script
            yield return BossFightScript();

            TestCase("leave fight");
            yield return LeaveFight();

            TestCase("verify module unload");
            yield return ExpectLog("[ModuleManager] Unload", 10);

            _fightInProgress = false;
            yield return new WaitForSeconds(1f); // wait for invincibility to recover and GG_Workshop to load
        }

        /**
         * Notes:
         *   - (good or bad) Hero stuck for a few seconds, then scene suddonly jump into fights. Fast, but abrupt.
         *   - (bad) Flash of black screen during win return's white screen.
         *   - (good) No save on win.
         *   - (bad) Save on death.
         */
        internal IEnumerator EnterFightViaSceneChange()
        {
            if (!_testInProgress) yield break;

            this.LogModTest($"EnterFightViaSceneChange()");

            // Death return
            PlayerData.instance.dreamReturnScene = "GG_Workshop";
            PlayerData.instance.bossReturnEntryGate = ReturnDoor;

            // Win return
            BossSceneController.SetupEvent = (self) => {
                self.BossLevel = ChallengeLevel;
                self.DreamReturnEvent = "DREAM RETURN";
                self.OnBossSceneComplete += () =>
                {
                    GameManager.instance.ChangeToScene("GG_Workshop", ReturnDoor, 0);
                };
            };

            // Start fight
            GameManager.instance.BeginSceneTransition(new GameManager.SceneLoadInfo
            {
                SceneName = BossScene,
                EntryGateName = "door_dreamEnter", // Don't miss this. Will cause fight to not start (boss and hero stuck).
                PreventCameraFadeOut = true,
                Visualization = GameManager.SceneLoadVisualizations.GodsAndGlory,
            });

            yield return 0;
        }

        /**
         * Notes:
         *   - (good) Smooth transition into fight. Not too slow.
         *   - (bad) Flash of black screen during win return's white screen.
         *   - (good) No save on win.
         *   - (bad) Save on death.
         * 
         * Assumptions:
         *   - Active scene is GG_Workshop (in order to get a statue GO).
         */
        internal IEnumerator EnterFightViaStatueGo()
        {
            if (!_testInProgress) yield break;

            this.LogModTest($"EnterFightViaStatueGo()");

            // Death return
            PlayerData.instance.dreamReturnScene = "GG_Workshop";
            PlayerData.instance.bossReturnEntryGate = ReturnDoor;

            // Win return
            BossSceneController.SetupEvent = (self) => {
                self.BossLevel = ChallengeLevel;
                self.DreamReturnEvent = "DREAM RETURN";
                self.OnBossSceneComplete += () =>
                {
                    GameManager.instance.ChangeToScene("GG_Workshop", ReturnDoor, 0);
                };
            };

            // Start fight
            GameObject statue = USceneManager.GetActiveScene()
                .GetRootGameObjects()
                .First(go => go.name == "GG_Statue_GreyPrince"); // statue name doesn't matter
            var statueControl = statue.transform.GetChild(0).gameObject.LocateMyFSM("GG Boss UI").Fsm;
            statueControl.GetState("Take Control").Transitions[0].ToFsmState = statueControl.GetState("Impact");
            statueControl.Variables.FindFsmString("Return Scene").Value = "GG_Workshop";
            statueControl.Variables.FindFsmString("To Scene").Value = BossScene;
            statueControl.SetState("Take Control");

            yield return 0;
        }

        /**
         * Notes:
         *   - (good) The most normal way to enter GG fights.
         *   - (good) No flash of black.
         *   - (bad) Save on win.
         *   - (bad) Save on death.
         *   
         * Assumptions:
         *   - The code which selects challenge level in the menu assumes that the menu started at challenge 0.
         *     This can be wrong assumption because menu can remember last challenge position.
         */
        internal IEnumerator EnterFightViaChallengeMenu()
        {
            if (!_testInProgress) yield break;

            this.LogModTest($"EnterFightViaChallengeMenu()");

            // warp
            HeroController.instance.gameObject.transform.position = StatuePos;

            // interact with menu
            yield return new WaitForSeconds(0.1f);
            yield return Up(0.1f);
            yield return new WaitForSeconds(1);
            for (int i =  0; i < ChallengeLevel; i++)
            {
                yield return Down(0.1f);
            }
            yield return Jump(0.1f);

            yield return 0;
        }

        internal IEnumerator SetInvincibility()
        {
            if (!_testInProgress) yield break;

            _workers++;
            var self = $"#{_nextWorkerId++}";
            this.LogModTest($"[{self}] SetInvincibility()");

            this.LogModTest($"[{self}] Noting down original invincibility: {PlayerData.instance.isInvincible}");
            bool orig = PlayerData.instance.isInvincible;

            while (_testInProgress && _fightInProgress)
            {
                PlayerData.instance.isInvincible = true;

                // wait before checking again
                //this.LogModTest($"        [{self}] Sleeping ...");
                const float sleep = 0.1f;
                yield return new WaitForSeconds(sleep);
            }

            _workers--;
            this.LogModTest($"[{self}] Recovering invincibility: {orig}");
            PlayerData.instance.isInvincible = orig;
        }

        // Tear downs don't need to check _testInProgress
        internal IEnumerator LeaveFight()
        {
            this.LogModTest($"LeaveFight()");
            GameManager.instance.BeginSceneTransition(new GameManager.SceneLoadInfo
            {
                SceneName = "GG_Workshop",
                EntryGateName = ReturnDoor,
                PreventCameraFadeOut = true,
                Visualization = GameManager.SceneLoadVisualizations.GodsAndGlory,
            });
            yield return 0;
        }

        private bool _fightInProgress;

        protected class BossAttackMetadata
        {
            internal string Name { get; set; }
            internal string Log { get; set; }
            internal float Duration { get; set; }
            internal int RepeatTimes { get; set; }
            internal BossAttackMetadata[] Parts { get; set; }
        }

        protected class BossMetadata
        {
            internal float StartIdle { get; set; }
            internal string IdleLog { get; set; }
            internal BossAttackMetadata[] Attacks { get; set; }
        }

        protected virtual BossMetadata BossMeta => null;

        protected IEnumerator TestFreeAttacks()
        {
            yield return TurnOffAllAttacks();

            _lastAttackDuration = Math.Max(BossMeta.StartIdle, BossMeta.Attacks.Max(a => a.Duration));
            yield return WaitForBossIdle("#1");

            for (int i = 0; i < BossMeta.Attacks.Length; i++) {
                yield return TestFreeAttack(i);
                yield return WaitForBossIdle($"#{i + 2}");
            }
        }

        protected IEnumerator TurnOffAllAttacks()
        {
            TestCase("turn off all attacks");
            for (int i = 0; i < BossMeta.Attacks.Length; i++)
            {
                yield return PressKey(KeyCode.Alpha1 + i, 0.1f);
            }
        }

        protected IEnumerator WaitForBossIdle(string id)
        {
            TestCase($"wait for boss idle {id}");
            yield return ExpectLog(BossMeta.IdleLog, _lastAttackDuration);

            var noLogs = new List<string>();
            foreach (var attack in BossMeta.Attacks)
            {
                // Parts has higher priority than Log/Duration
                if (attack.Parts != null)
                {
                    foreach (var part in attack.Parts)
                    {
                        noLogs.Add(part.Log);
                    }
                }
                else
                {
                    noLogs.Add(attack.Log);
                }
            }

            for (int i = 0; i < noLogs.Count - 1; i++)
            {
                ExpectNoLogInParallel(noLogs[i], 2);
            }
            yield return ExpectNoLog(noLogs.Last(), 2);
            _lastAttackDuration = 2; // no attack is going on
        }

        private IEnumerator TestFreeAttack(int idx)
        {
            TestCase($"test attack {idx + 1}");
            var attack = BossMeta.Attacks[idx];

            // enable attack
            yield return PressKey(KeyCode.Alpha1 + idx, 0.1f);

            for (int i = 0; i < attack.RepeatTimes; i++)
            {
                TestCase($"test attack {idx + 1} - {attack.Name} #{i + 1}");
                // Parts has higher priority than Log/Duration
                if (attack.Parts != null)
                {
                    foreach (var part in attack.Parts)
                    {
                        yield return ExpectLog(part.Log, _lastAttackDuration);
                        _lastAttackDuration = part.Duration; // this attack is going on
                    }
                }
                else
                {
                    yield return ExpectLog(attack.Log, _lastAttackDuration);
                    _lastAttackDuration = attack.Duration; // this attack is going on
                }
            }

            // disable attack
            yield return PressKey(KeyCode.Alpha1 + idx, 0.1f);
        }

        protected float _lastAttackDuration;
    }
}
