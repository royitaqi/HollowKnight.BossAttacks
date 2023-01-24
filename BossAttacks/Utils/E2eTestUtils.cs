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
        protected abstract IEnumerator Script();

        private IEnumerator Setup()
        {
            _testCase = "Setup";
            this.LogModTest("Setting up test");

            _workers = 0;
            _nextWorkerId = 0;
            _testErrors = new();
            _testInProgress = true;

            // user input
            InputUtils.Load();
            KeyboardOverride.Load();

            // logging
            yield return InterceptLog();
            InParallel(ExpectNotLogInParallel("[E]", int.MaxValue));
            InParallel(ExpectLimitedLogSize());

            this.LogModTest($"Setup completed");
            yield return 0;
        }

        private IEnumerator TearDown()
        {
            _testCase = "TearDown";
            this.LogModTest("Tearing down test");

            _testInProgress = false;
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

        private IEnumerator Conclude()
        {
            _testCase = "Conclude";
            this.LogModTest("Concluding");

            if (_testErrors.Count == 0)
            {
                this.LogModTest("TEST SUCCEEDED");
            }
            else
            {
                this.LogModTest($"TEST FAILED with {_testErrors.Count} error(s):");
                foreach (var error in _testErrors)
                {
                    this.LogModTest($"    - {error}");
                }
            }
            _testErrors = null;

            yield return 0;
        }

        protected bool Assert(bool condition, string message)
        {
            if (!condition)
            {
                _testInProgress = false;
                _testErrors.Add(message);
                return true;
            }
            return false;
        }

        /**
         * Test WITH setup and tear down.
         */
        private IEnumerator FullTest()
        {
            yield return Setup();

            this.LogModTest("Starting script");
            yield return Script();

            yield return TearDown();
            yield return Conclude();
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
            if (!_testInProgress) yield break;

            this.LogModTest($"DLeft(seconds = {seconds})");
            InputUtils.PressDirection("left");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseDirection("left");
        }

        internal IEnumerator DUp(float seconds)
        {
            if (!_testInProgress) yield break;

            this.LogModTest($"DUp(seconds = {seconds})");
            InputUtils.PressDirection("up");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseDirection("up");
        }

        internal IEnumerator Up(float seconds)
        {
            if (!_testInProgress) yield break;

            this.LogModTest($"Up(seconds = {seconds})");
            InputUtils.PressButton("up");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseButton("up");
        }

        internal IEnumerator Down(float seconds)
        {
            if (!_testInProgress) yield break;

            this.LogModTest($"Down(seconds = {seconds})");
            InputUtils.PressButton("down");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseButton("down");
        }

        internal IEnumerator Jump(float seconds)
        {
            if (!_testInProgress) yield break;

            this.LogModTest($"Jump(seconds = {seconds})");
            InputUtils.PressButton("jump");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseButton("jump");
        }

        internal IEnumerator PressKey(KeyCode key, float seconds)
        {
            if (!_testInProgress) yield break;

            this.LogModTest($"PressKey(key = {key}, seconds = {seconds})");
            KeyboardOverride.PressKey(key, 1);
            yield return new WaitForSeconds(seconds);
            KeyboardOverride.ReleaseKey(key);
        }

        internal IEnumerator Teleport(float x, float y)
        {
            if (!_testInProgress) yield break;

            this.LogModTest($"Teleport(x = {x}, y = {y})");
            HeroController.instance.gameObject.transform.position = new Vector3(x, y, 0);
            yield return 0;
        }

        internal IEnumerator InterceptLog()
        {
            this.LogModTest("InterceptLog()");

            if (Assert(_origLoggingFunction == null, "Can only set up log interception once")) yield break;
            _origLoggingFunction = LoggingUtils.LoggingFunction;
            _origFilterFunction = LoggingUtils.FilterFunction;
            _origLogLevel = LoggingUtils.LogLevel;
            _logs = new();
            _logs.AddLast("BEGINNING OF TEST LOG");
            _logPointer = _logs.Last;
            _logPointerInUse = false;

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
            this.LogModTest("RecoverLog()");

            LoggingUtils.LoggingFunction = _origLoggingFunction;
            LoggingUtils.FilterFunction = _origFilterFunction;
            LoggingUtils.LogLevel = _origLogLevel;
            _logs = null;
            _logPointer = null;
            _logPointerInUse = false;
            yield return 0;
        }

        private class LogIterator
        {
            internal Func<bool> HasMoreLog;
            internal Action MoveToNextLog;
            internal Func<string> GetCurrentLog;
        }

        /// <param name="mode">0 = continue and exclusive, 1 = continue and parallel, 2 = from head and parallel.</param>
        private LogIterator GetLogIterator(int mode)
        {
            if (mode == 0)
            {
                return new LogIterator
                {
                    HasMoreLog = () => _logPointer != _logs.Last,
                    MoveToNextLog = () => _logPointer = _logPointer.Next,
                    GetCurrentLog = () => _logPointer.Value,
                };
            }
            else if (mode == 1 || mode == 2)
            {
                var c = mode == 1 ? _logPointer : _logs.First;
                return new LogIterator
                {
                    HasMoreLog = () => c != _logs.Last,
                    MoveToNextLog = () => c = c.Next,
                    GetCurrentLog = () => c.Value,
                };
            }
            else
            {
                throw new ModException($"Mode should be 0, 1, or 2 (got {mode})");
            }
        }

        internal IEnumerator ExpectLog(string content, float timeoutSeconds)
        {
            if (Assert(!_logPointerInUse, "Log pointer can only be used by one mode0 verifier")) yield break;
            _logPointerInUse = true;
            yield return ExpectLog(content, timeoutSeconds, GetLogIterator(0));
            _logPointerInUse = false;
        }
        internal IEnumerator ExpectLogInParallel(string content, float timeoutSeconds) => ExpectLog(content, timeoutSeconds, GetLogIterator(1));
        private IEnumerator ExpectLog(string content, float timeoutSeconds, LogIterator iter)
        {
            if (!_testInProgress) yield break;

            _workers++;
            var self = $"#{_nextWorkerId++}";
            this.LogModTest($"[{self}] ExpectLog(content = \"{content}\", timeoutSeconds = {timeoutSeconds})");

            while (_testInProgress)
            {
                while (iter.HasMoreLog())
                {
                    iter.MoveToNextLog();
                    // Skip test logs
                    if (iter.GetCurrentLog().Contains("[T]"))
                    {
                        continue;
                    }
                    if (iter.GetCurrentLog().Contains(content))
                    {
                        // found log. good
                        this.LogModTest($"[{self}] Found expected content \"{content}\" in log \"{iter.GetCurrentLog()}\". Good.");
                        _workers--;
                        yield break;
                    }
                }

                // can no longer wait
                const float sleep = 0.1f;
                timeoutSeconds -= sleep;
                if (Assert(timeoutSeconds > 0 || Mathf.Approximately(timeoutSeconds, 0), $"[{self}] TEST FAILED: Cannot find content \"{content}\" in logs"))
                {
                    _workers--;
                    yield break;
                }

                // wait before checking again
                //this.LogModTest($"        [{self}] Sleeping ...");
                yield return new WaitForSeconds(sleep);
            }

            _workers--;
        }

        internal IEnumerator ExpectNotLog(string content, float timeoutSeconds)
        {
            if (Assert(!_logPointerInUse, "Log pointer can only be used by one mode0 verifier")) yield break;
            _logPointerInUse = true;
            yield return ExpectNotLog(content, timeoutSeconds, GetLogIterator(0));
            _logPointerInUse = false;
        }
        internal IEnumerator ExpectNotLogInParallel(string content, float timeoutSeconds) => ExpectNotLog(content, timeoutSeconds, GetLogIterator(1));
        private IEnumerator ExpectNotLog(string content, float timeoutSeconds, LogIterator iter)
        {
            if (!_testInProgress) yield break;

            _workers++;
            var self = $"#{_nextWorkerId++}";
            this.LogModTest($"[{self}] ExpectNotLog(content = \"{content}\", timeoutSeconds = {timeoutSeconds})");

            while (_testInProgress)
            {
                while (iter.HasMoreLog())
                {
                    iter.MoveToNextLog();
                    // Skip test logs
                    if (iter.GetCurrentLog().Contains("[T]"))
                    {
                        continue;
                    }
                    if (Assert(!iter.GetCurrentLog().Contains(content), $"[{self}] TEST FAILED: Unexpected content \"{content}\" found in log \"{iter.GetCurrentLog()}\""))
                    {
                        _workers--;
                        yield break;
                    }
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

                // wait before checking again
                //this.LogModTest($"        [{self}] Sleeping ...");
                yield return new WaitForSeconds(sleep);
            }

            _workers--;
        }

        internal IEnumerator ExpectLimitedLogSize()
        {
            if (!_testInProgress) yield break;

            _workers++;
            var self = $"#{_nextWorkerId++}";
            this.LogModTest($"[{self}] ExpectLimitedLogSize()");

            const int limit = 99999;
            while (_testInProgress)
            {
                if (Assert(_logs.Count <= limit, $"[{self}] TEST FAILED: Too many logs have been generated ({_logs.Count} > {limit})"))
                {
                    _workers--;
                    yield break;
                }

                // wait before checking again
                //this.LogModTest($"        [{self}] Sleeping ...");
                const float sleep = 0.1f;
                yield return new WaitForSeconds(sleep);
            }

            _workers--;
        }

        private LinkedList<string> _logs;
        private LinkedListNode<string> _logPointer;
        private bool _logPointerInUse;
        private Action<string> _origLoggingFunction;
        private Func<string, bool> _origFilterFunction;
        private LogLevel _origLogLevel;

        private string _testCase;
        private int _workers;
        private int _nextWorkerId;
        private bool _testInProgress;
        protected bool TestInProgress => _testInProgress;
        private List<string> _testErrors;
    }

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
            TestCase("wait for fight and modules to load");
            yield return InvincibleHero();
            yield return EnterFightViaStatueGo();
            yield return ExpectLog("[ModuleManager] Level is now 0", 10);

            // Run actual boss fight test script
            yield return BossFightScript();

            TestCase("leave fight");
            yield return LeaveFight();
            yield return RecoverInvincibility();

            TestCase("verify module unload");
            yield return ExpectLog("[ModuleManager] Unload", 10);
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
            if (!TestInProgress) yield break;

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
            if (!TestInProgress) yield break;

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
            if (!TestInProgress) yield break;

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
            if (!TestInProgress) yield break;

            this.LogModTest($"InvincibleHero()");
            _origIsInvincible = PlayerData.instance.isInvincible;
            PlayerData.instance.isInvincible = true;
            yield return 0;
        }

        // Tear downs don't need to check TestInProgress
        internal IEnumerator RecoverInvincibility()
        {
            this.LogModTest($"RecoverInvincibility()");
            PlayerData.instance.isInvincible = _origIsInvincible;
            yield return 0;
        }

        // Tear downs don't need to check TestInProgress
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
