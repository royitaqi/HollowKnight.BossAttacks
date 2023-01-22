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

        internal IEnumerator Left(float seconds)
        {
            this.LogMod($"Left(seconds = {seconds})");
            InputUtils.PressDirection("left");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseDirection("left");
        }

        internal IEnumerator Up(float seconds)
        {
            this.LogMod($"Up(seconds = {seconds})");
            InputUtils.PressButton("up");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseButton("up");
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
        protected abstract Vector3 BossDoorPos { get; }

        internal virtual IEnumerator EnterFight()
        {
            //GameManager.instance.ChangeToScene(BossScene, "", 0);

            PlayerData.instance.dreamReturnScene = "GG_Workshop";
            PlayerData.instance.bossReturnEntryGate = "door_dreamReturnGG_GG_Statue_Gruz"; // "door_dreamReturnGG_GG_Statue_HollowKnight";

            BossSceneController.SetupEvent = (self) => {
                self.BossLevel = 0;
                self.DreamReturnEvent = "DREAM RETURN";
                self.OnBossSceneComplete += () =>
                {
                    GameManager.instance.ChangeToScene("GG_Workshop", "door_dreamReturnGG_GG_Statue_Gruz", 0);
                };
            };

            GameManager.instance.BeginSceneTransition(new GameManager.SceneLoadInfo
            {
                //SceneName = "GG_Radiance",
                //EntryGateName = "door_dreamEnter",
                //PreventCameraFadeOut = true,
                //Visualization = GameManager.SceneLoadVisualizations.GodsAndGlory,
                SceneName = "GG_Gruz_Mother",
                EntryGateName = "door_dreamEnter",
                PreventCameraFadeOut = true,
                Visualization = GameManager.SceneLoadVisualizations.GodsAndGlory,
            });


            //HeroController.instance.gameObject.transform.position = BossDoorPos;
            //yield return new WaitForSeconds(0.1f);
            //yield return Up(0.1f);
            //yield return new WaitForSeconds(2);
            //yield return Jump(0.1f);

            yield return 0;
        }

        internal virtual IEnumerator EnterFight2()
        {
            PlayerData.instance.dreamReturnScene = "GG_Workshop";
            PlayerData.instance.bossReturnEntryGate = "door_dreamReturnGG_GG_Statue_Gruz"; // "door_dreamReturnGG_GG_Statue_HollowKnight";

            BossSceneController.SetupEvent = (self) => {
                self.BossLevel = 0; // Set to your value: 0 - Attuned, 1 - Ascended, 2 - Radiant
                self.DreamReturnEvent = "DREAM RETURN";
                self.OnBossSceneComplete += () =>
                {
                    GameManager.instance.ChangeToScene("GG_Workshop", "door_dreamReturnGG_GG_Statue_Gruz", 0);
                };
            };

            GameObject statue = USceneManager.GetActiveScene()
                .GetRootGameObjects()
                .First(go => go.name == "GG_Statue_GreyPrince");

            var statueControl = statue.transform.GetChild(0).gameObject.LocateMyFSM("GG Boss UI").Fsm;
            statueControl.GetState("Take Control").Transitions[0].ToFsmState = statueControl.GetState("Impact");
            statueControl.Variables.FindFsmString("Return Scene").Value = "GG_Workshop";
            statueControl.Variables.FindFsmString("To Scene").Value = "GG_Gruz_Mother";
            statueControl.SetState("Take Control");

            yield return 0;
        }


        internal virtual IEnumerator EnterFight3()
        {
            HeroController.instance.gameObject.transform.position = new Vector3(28, 6.4f, 0);
            yield return new WaitForSeconds(0.1f);
            yield return Up(5f);
            yield return new WaitForSeconds(2);
            yield return Jump(0.1f);

            yield return 0;
        }

        internal virtual IEnumerator LeaveFight()
        {
            yield return 0;
        }
    }
}
