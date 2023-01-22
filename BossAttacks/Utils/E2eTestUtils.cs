using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace BossAttacks.Utils
{
    internal abstract class E2eTest
    {
        internal abstract IEnumerator Script();

        private IEnumerator Setup()
        {
            this.LogMod("Setting up test");
            InputUtils.Load();
            yield return 0;
        }

        private IEnumerator TearDown()
        {
            this.LogMod("Tearing down test");
            InputUtils.Unload();
            yield return 0;
        }

        /**
         * Test WITH setup and tear down.
         */
        private IEnumerator FullTest()
        {
            yield return Setup();

            this.LogMod("Starting script");
            yield return Script();

            yield return TearDown();

            this.LogMod("TEST SUCCEEDED");
        }

        /**
         * Test WITHOUT setup and tear down.
         */
        public IEnumerator LeanTest()
        {
            this.LogMod("Starting script");
            yield return Script();

            this.LogMod("TEST SUCCEEDED");
        }

        internal void InParallel(IEnumerator coroutine)
        {
            HeroController.instance.StartCoroutine(coroutine);
        }

        internal void RunFullTest() => InParallel(FullTest());
        internal void RunLeanTest() => InParallel(LeanTest());

        internal IEnumerator DLeft(float seconds)
        {
            this.LogMod($"DLeft(seconds = {seconds})");
            InputUtils.PressDirection("left");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseDirection("left");
        }

        internal IEnumerator DUp(float seconds)
        {
            this.LogMod($"DUp(seconds = {seconds})");
            InputUtils.PressDirection("up");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseDirection("up");
        }

        internal IEnumerator Up(float seconds)
        {
            this.LogMod($"Up(seconds = {seconds})");
            InputUtils.PressButton("up");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseButton("up");
        }

        internal IEnumerator Down(float seconds)
        {
            this.LogMod($"Down(seconds = {seconds})");
            InputUtils.PressButton("down");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseButton("down");
        }

        internal IEnumerator Jump(float seconds)
        {
            this.LogMod($"Jump(seconds = {seconds})");
            InputUtils.PressButton("jump");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseButton("jump");
        }

        internal IEnumerator Teleport(float x, float y)
        {
            this.LogMod($"Teleport(x = {x}, y = {y})");
            //var cp = HeroController.instance.gameObject.transform.position;
            HeroController.instance.gameObject.transform.position = new Vector3(x, y, 0);
            yield return 0;
        }

        internal IEnumerator InterceptLog()
        {
            this.LogMod("InterceptLog()");

            ModAssert.AllBuilds(_origLoggingFunction == null, "Can only set up log interception once");
            _origLoggingFunction = LoggingUtils.LoggingFunction;

            LoggingUtils.LoggingFunction = (log) =>
            {
                _origLoggingFunction(log);
                _logs.AddLast(log);
            };
            yield return 0;
        }

        internal IEnumerator ExpectLog(string expectedLog, int timeoutSeconds)
        {
            this.LogMod($"ExpectLog(expectedLog = \"{expectedLog}\", timeoutSeconds = {timeoutSeconds})");
            while (true)
            {
                while (_logs.Count != 0)
                {
                    if (_logs.First.Value.Contains(expectedLog))
                    {
                        this.LogModDebug($"Found log: \"{_logs.First.Value}\"");
                        // found log. good
                        _logs.RemoveFirst();
                        yield break;
                    }
                    _logs.RemoveFirst();
                }

                // can no longer wait
                ModAssert.AllBuilds(timeoutSeconds-- > 0, $"TEST FAILED: Cannot find log \"{expectedLog}\"");

                // wait for 1 sec
                this.LogModDebug($"Couldn't find log. Sleeping for 1 second.");
                yield return new WaitForSeconds(1);
            }
        }

        private readonly LinkedList<string> _logs = new();
        private Action<string> _origLoggingFunction;
    }

    internal abstract class E2eBossFightTest : E2eTest
    {
        protected abstract string BossScene { get; }
        protected abstract Vector3 StatuePos { get; }
        protected abstract int ChallengeLevel { get; }
        protected abstract string ReturnDoor { get; }

        /**
         * Notes:
         *   - (good or bad) Hero stuck for a few seconds, then scene suddonly jump into fights. Fast, but abrupt.
         *   - (bad) Flash of black screen during win return's white screen.
         *   - (good) No save on win.
         *   - (bad) Save on death.
         */
        internal virtual IEnumerator EnterFightViaSceneChange()
        {
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
        internal virtual IEnumerator EnterFightViaStatueGo()
        {
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
        internal virtual IEnumerator EnterFightViaChallengeMenu()
        {
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

        internal virtual IEnumerator LeaveFight()
        {
            yield return 0;
        }
    }
}
