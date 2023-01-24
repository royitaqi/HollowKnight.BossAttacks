using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestPureVessel : E2eBossFightTest
    {
        protected override string BossScene => "GG_Hollow_Knight";

        protected override IEnumerator BossFightScript()
        {
            yield return TurnOffAllAttacks(6);

            yield return WaitForBossIdle("#1", 20);

            TestCase("test attack 1 - DASH #1");
            yield return PressKey(KeyCode.Alpha1, 0.1f);
            yield return ExpectLog("Boss entering state Dash Antic", 5);
            TestCase("test attack 1 - DASH #2");
            yield return ExpectLog("Boss entering state Dash Antic", 5);
            yield return PressKey(KeyCode.Alpha1, 0.1f);

            yield return WaitForBossIdle("#2", 5);

            TestCase("test attack 2 - DSTAB #1");
            yield return PressKey(KeyCode.Alpha2, 0.1f);
            yield return ExpectLog("Boss entering state Stomp Antic", 5);
            TestCase("test attack 2 - DSTAB #2");
            yield return ExpectLog("Boss entering state Stomp Antic", 5);
            yield return PressKey(KeyCode.Alpha2, 0.1f);

            yield return WaitForBossIdle("#3", 5);

            TestCase("test attack 3 - FOCUS #1");
            yield return PressKey(KeyCode.Alpha3, 0.1f);
            yield return ExpectLog("Boss entering state Focus Charge", 10);
            TestCase("test attack 3 - FOCUS #2");
            yield return ExpectLog("Boss entering state Focus Charge", 10);
            yield return PressKey(KeyCode.Alpha3, 0.1f);

            yield return WaitForBossIdle("#4", 10);

            TestCase("test attack 4 - SLASH #1");
            yield return PressKey(KeyCode.Alpha4, 0.1f);
            yield return ExpectLog("Boss entering state Slash1 Antic", 5);
            TestCase("test attack 4 - SLASH #2");
            yield return ExpectLog("Boss entering state Slash1 Antic", 5);
            yield return PressKey(KeyCode.Alpha4, 0.1f);

            yield return WaitForBossIdle("#5", 5);

            TestCase("test attack 5 - SMALL SHOT #1");
            yield return PressKey(KeyCode.Alpha5, 0.1f);
            yield return ExpectLog("Boss entering state SmallShot Antic", 5);
            TestCase("test attack 5 - SMALL SHOT #2");
            yield return ExpectLog("Boss entering state SmallShot Antic", 5);
            yield return PressKey(KeyCode.Alpha5, 0.1f);

            yield return WaitForBossIdle("#6", 5);

            TestCase("test attack 6 - TENDRIL #1");
            yield return PressKey(KeyCode.Alpha6, 0.1f);
            yield return ExpectLog("Boss entering state Tendril Antic", 5);
            TestCase("test attack 6 - TENDRIL #2");
            yield return ExpectLog("Boss entering state Tendril Antic", 5);
            yield return PressKey(KeyCode.Alpha6, 0.1f);

            yield return WaitForBossIdle("#7", 5);
        }

        private IEnumerator TurnOffAllAttacks(int count)
        {
            TestCase("turn off all attacks");
            for (int i = 1; i <= count; i++)
            {
                yield return PressKey(KeyCode.Alpha0 + i, 0.1f);
            }
        }

        private IEnumerator WaitForBossIdle(string id, float seconds)
        {
            TestCase($"wait for boss idle {id}");
            yield return ExpectLog("Boss entering state Choice P3 SCP", seconds);
            InParallel(ExpectNotLogInParallel("Boss entering state Dash Antic", 2));
            InParallel(ExpectNotLogInParallel("Boss entering state Stomp Antic", 2));
            InParallel(ExpectNotLogInParallel("Boss entering state Focus Charge", 2));
            InParallel(ExpectNotLogInParallel("Boss entering state Slash1 Antic", 2));
            InParallel(ExpectNotLogInParallel("Boss entering state SmallShot Antic", 2));
            yield return ExpectNotLog("Boss entering state Tendril Antic", 2);
        }
    }
}
