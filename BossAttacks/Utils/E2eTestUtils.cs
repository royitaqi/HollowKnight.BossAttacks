using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAttacks.Utils
{
    internal abstract class E2eTest
    {
        internal abstract IEnumerator Script();

        private IEnumerator Setup()
        {
            this.LogMod("Setting up test");
            InputUtils.Load();
            this.LogMod("Starting script");
            yield return 0;
        }

        private IEnumerator TearDown()
        {
            this.LogMod("Tearing down test");
            InputUtils.Unload();
            this.LogMod("TEST SUCCEEDED");
            yield return 0;
        }

        private IEnumerator FullTest()
        {
            yield return Setup();
            yield return Script();
            yield return TearDown();
        }

        internal void RunScript()
        {
            HeroController.instance.StartCoroutine(FullTest());
        }

        internal IEnumerator Left(float seconds)
        {
            this.LogMod($"Left(seconds = {seconds})");
            InputUtils.PressDirection("left");
            yield return new WaitForSeconds(seconds);
            InputUtils.ReleaseDirection("left");
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

    internal class E2eTestSoulTyrant : E2eTest
    {
        internal override IEnumerator Script()
        {
            yield return InterceptLog();
            yield return Left(3);
            yield return ExpectLog("Overriding left", 5);
        }
    }
}
