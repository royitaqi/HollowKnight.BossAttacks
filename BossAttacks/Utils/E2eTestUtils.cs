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
     * Base class of E2E tests.
     * 
     * Also utility functions that are NOT specific to boss fights.
     */
    internal abstract class E2eTest
    {
        internal abstract IEnumerator Script();

        private IEnumerator Setup()
        {
            _testCase = "Setup";
            this.LogModTest("Setting up test");

            _workers = 0;
            _nextWorkerId = 0;
            _workerAllowed = true;

            // user input
            InputUtils.Load();
            KeyboardOverride.Load();

            // logging
            yield return InterceptLog();
            InParallel(ExpectNotLog("[E]", int.MaxValue));
            InParallel(ExpectLimitedLogSize());

            this.LogModTest($"Setup completed");
            yield return 0;
        }

        private IEnumerator TearDown()
        {
            _testCase = "TearDown";
            this.LogModTest("Tearing down test");

            _workerAllowed = false;
            while (_workers > 0)
            {
                this.LogModTest($"Waiting for {_workers} workers to finish");
                yield return new WaitForSeconds(0.1f);
            }
            this.LogModTest($"All workers finished");

            // user input
            InputUtils.Unload();
            KeyboardOverride.Unload();

            // logging
            yield return RecoverLog();

            this.LogModTest($"Tear down completed");
            yield return 0;
        }

        /**
         * Test WITH setup and tear down.
         */
        private IEnumerator FullTest()
        {
            yield return Setup();

            this.LogModTest("Starting script");
            yield return Script();
            _testCase = "Conclusion";
            this.LogModTest("TEST SUCCEEDED");

            yield return TearDown();
        }

        /**
         * Test WITHOUT setup and tear down.
         */
        internal IEnumerator LeanTest()
        {
            this.LogModTest("Starting script");
            yield return Script();
            _testCase = "Conclusion";
            this.LogModTest("TEST SUCCEEDED");
        }

        internal void InParallel(IEnumerator coroutine)
        {
            HeroController.instance.StartCoroutine(coroutine);
        }

        internal void TestCase(string s)
        {
            _testCase = s;
        }

        internal void RunFullTest() => InParallel(FullTest());
        internal void RunLeanTest() => InParallel(LeanTest());

        internal IEnumerator DLeft(float seconds)
        {
            this.LogModTest($"DLeft(seconds = {seconds})");
            InputUtils.PressDirection("left");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseDirection("left");
        }

        internal IEnumerator DUp(float seconds)
        {
            this.LogModTest($"DUp(seconds = {seconds})");
            InputUtils.PressDirection("up");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseDirection("up");
        }

        internal IEnumerator Up(float seconds)
        {
            this.LogModTest($"Up(seconds = {seconds})");
            InputUtils.PressButton("up");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseButton("up");
        }

        internal IEnumerator Down(float seconds)
        {
            this.LogModTest($"Down(seconds = {seconds})");
            InputUtils.PressButton("down");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseButton("down");
        }

        internal IEnumerator Jump(float seconds)
        {
            this.LogModTest($"Jump(seconds = {seconds})");
            InputUtils.PressButton("jump");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseButton("jump");
        }

        internal IEnumerator PressKey(KeyCode key, float seconds)
        {
            this.LogModTest($"PressKey(key = {key}, seconds = {seconds})");
            KeyboardOverride.PressKey(key, 1);
            yield return new WaitForSeconds(seconds);
            KeyboardOverride.ReleaseKey(key);
        }

        internal IEnumerator Teleport(float x, float y)
        {
            this.LogModTest($"Teleport(x = {x}, y = {y})");
            HeroController.instance.gameObject.transform.position = new Vector3(x, y, 0);
            yield return 0;
        }

        internal IEnumerator InterceptLog()
        {
            this.LogModTest("InterceptLog()");

            ModAssert.AllBuilds(_origLoggingFunction == null, "Can only set up log interception once");
            _origLoggingFunction = LoggingUtils.LoggingFunction;
            _origFilterFunction = LoggingUtils.FilterFunction;
            _origLogLevel = LoggingUtils.LogLevel;
            _logs = new();

            LoggingUtils.LoggingFunction = (log) =>
            {
                _origLoggingFunction($">>{GetType().Name}.{_testCase}<< {log}");
                _logs.AddLast(log);
            };
            LoggingUtils.FilterFunction = LoggingUtils.NoFilters;
            LoggingUtils.LogLevel = Modding.LogLevel.Fine;

            this.LogModTest("Log interceppted"); // print one line of log, so that ExpectLog() and ExpectNotLog() can assume there is at least one lie of log.
            yield return 0;
        }

        internal IEnumerator RecoverLog()
        {
            LoggingUtils.LoggingFunction = _origLoggingFunction;
            LoggingUtils.FilterFunction = _origFilterFunction;
            LoggingUtils.LogLevel = _origLogLevel;
            _logs = null;
            yield return 0;
        }

        internal IEnumerator ExpectLog(string content, float timeoutSeconds)
        {
            _workers++;
            var self = $"#{_nextWorkerId++}";
            this.LogModTest($"[{self}] ExpectLog(content = \"{content}\", timeoutSeconds = {timeoutSeconds})");

            var c = _logs.Last;
            while (_workerAllowed)
            {
                while (c != _logs.Last)
                {
                    c = c.Next;
                    if (c.Value.Contains(content) && !c.Value.Contains("[T]"))
                    {
                        // found log. good
                        this.LogModTest($"[{self}] Found expected content \"{content}\" in log \"{c.Value}\". Good.");
                        _workers--;
                        yield break;
                    }
                }

                // can no longer wait
                const float sleep = 0.1f;
                timeoutSeconds -= sleep;
                ModAssert.AllBuilds(timeoutSeconds > 0 || Mathf.Approximately(timeoutSeconds, 0), $"[{self}] TEST FAILED: Cannot find content \"{content}\" in logs");

                // wait for 1 sec
                //this.LogModTest($"        [{self}] Sleeping ...");
                yield return new WaitForSeconds(sleep);
            }

            _workers--;
        }

        internal IEnumerator ExpectNotLog(string content, float timeoutSeconds)
        {
            _workers++;
            var self = $"#{_nextWorkerId++}";
            this.LogModTest($"{self} ExpectNotLog(content = \"{content}\", timeoutSeconds = {timeoutSeconds})");

            var c = _logs.Last;
            while (_workerAllowed)
            {
                while (c != _logs.Last)
                {
                    c = c.Next;
                    ModAssert.AllBuilds(!c.Value.Contains(content) || c.Value.Contains("[T]"), $"[{self}] TEST FAILED: Unexpected content \"{content}\" found in log \"{c.Value}\"");
                }

                // time is up. no bad log found. good.
                const float sleep = 0.1f;
                timeoutSeconds -= sleep;
                if (timeoutSeconds < 0 || Mathf.Approximately(timeoutSeconds, 0))
                {
                    this.LogModTest($"[{self}] Didn't find unexpected content \"{content}\". Good.");
                    _workers--;
                    yield break;
                }

                // wait for 1 sec
                //this.LogModTest($"        [{self}] Sleeping ...");
                yield return new WaitForSeconds(sleep);
            }

            _workers--;
        }

        internal IEnumerator ExpectLimitedLogSize()
        {
            _workers++;
            var self = $"#{_nextWorkerId++}";
            this.LogModTest($"[{self}] ExpectLimitedLogSize()");

            const int limit = 99999;
            while (_workerAllowed)
            {
                ModAssert.AllBuilds(_logs.Count <= limit, $"[{self}] TEST FAILED: Too many logs have been generated ({_logs.Count} > {limit})");

                // wait before checking again
                //this.LogModTest($"        [{self}] Sleeping ...");
                const float sleep = 0.1f;
                yield return new WaitForSeconds(sleep);
            }

            _workers--;
        }

        private LinkedList<string> _logs;
        private Action<string> _origLoggingFunction;
        private Func<string, bool> _origFilterFunction;
        private LogLevel _origLogLevel;

        private string _testCase;
        private int _workers;
        private int _nextWorkerId;
        private bool _workerAllowed;
    }

    /**
     * Utility functions specific for boss fights.
     */
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
        internal IEnumerator EnterFightViaSceneChange()
        {
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

        internal IEnumerator InvincibleHero()
        {
            this.LogModTest($"InvincibleHero()");
            _origIsInvincible = PlayerData.instance.isInvincible;
            PlayerData.instance.isInvincible = true;
            yield return 0;
        }

        internal IEnumerator RecoverInvincibility()
        {
            this.LogModTest($"RecoverInvincibility()");
            PlayerData.instance.isInvincible = _origIsInvincible;
            yield return 0;
        }

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

        private bool _origIsInvincible;
    }
}
