using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestSoulTyrant : E2eBossFightTest
    {
        protected override string BossScene => "GG_Soul_Tyrant";

        protected override IEnumerator BossFightScript()
        {
            yield return TurnOffAllAttacks(4);

            yield return WaitForBossIdle("#1", 5);

            TestCase("test attack 1 - CHARGE #1");
            yield return PressKey(KeyCode.Alpha1, 0.1f);
            yield return ExpectLog("Boss entering state Charge Antic", 10);
            TestCase("test attack 1 - CHARGE #2");
            yield return ExpectLog("Boss entering state Charge Antic", 10);
            yield return PressKey(KeyCode.Alpha1, 0.1f);

            yield return WaitForBossIdle("#2", 10);

            TestCase("test attack 2 - DIVE #1");
            yield return PressKey(KeyCode.Alpha2, 0.1f);
            yield return ExpectLog("Boss entering state Quake Antic", 5);
            TestCase("test attack 2 - DIVE #2");
            yield return ExpectLog("Boss entering state Quake Antic", 5);
            yield return PressKey(KeyCode.Alpha2, 0.1f);

            yield return WaitForBossIdle("#2", 5);

            TestCase("test attack 3 - HIGH SPINNER #1");
            yield return PressKey(KeyCode.Alpha3, 0.1f);
            yield return ExpectLog("Boss entering state HS Summon", 10);
            TestCase("test attack 3 - HIGH SPINNER #2");
            yield return ExpectLog("Boss entering state HS Summon", 10);
            yield return PressKey(KeyCode.Alpha3, 0.1f);

            yield return WaitForBossIdle("#3", 10);

            TestCase("test attack 4 - SHOOT #1");
            yield return PressKey(KeyCode.Alpha4, 0.1f);
            yield return ExpectLog("Boss entering state Shot Antic", 5);
            TestCase("test attack 4 - SHOOT #2");
            yield return ExpectLog("Boss entering state Shot Antic", 5);
            yield return PressKey(KeyCode.Alpha4, 0.1f);

            yield return WaitForBossIdle("#4", 5);

            TestCase("test advance to phase 2");
            yield return PressKey(KeyCode.Alpha5, 0.1f);
            yield return ExpectLog("Boss entering state Quake Antic", 10);

            TestCase("test turn on infinite dive");
            yield return PressKey(KeyCode.Alpha1, 0.1f);
            yield return ExpectLog("[BossAttacks] User is changing option [     ] - Extra: Infinite DIVE", 2);
            yield return ExpectLog("[ModuleManager] Options are:", 2);
            yield return ExpectLog("[ModuleManager]     [ ✓ ] - Extra: Infinite DIVE", 2);

            for (int i = 1; i <= 11; i++)
            {
                TestCase($"test infinite dive #{i}");
                yield return ExpectLog("Boss entering state Quake Antic", 5);
            }

            TestCase("test turn off infinite dive");
            yield return PressKey(KeyCode.Alpha1, 0.1f);
            yield return ExpectLog("[BossAttacks] User is changing option [ ✓ ] - Extra: Infinite DIVE", 2);
            yield return ExpectLog("[ModuleManager] Options are:", 2);
            yield return ExpectLog("[ModuleManager]     [     ] - Extra: Infinite DIVE", 2);

            TestCase("test shoot");
            InParallel(ExpectNotLogInParallel("Boss entering state Quake Antic", 5));
            yield return ExpectLog("Boss entering state Shot Antic", 5);
            yield return ExpectLog("Boss entering state Quake Antic", 20);
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
            yield return ExpectLog("Boss entering state Attack Choice SCP", seconds);
            InParallel(ExpectNotLogInParallel("Boss entering state Charge Antic", 2));
            InParallel(ExpectNotLogInParallel("Boss entering state Quake Antic", 2));
            InParallel(ExpectNotLogInParallel("Boss entering state HS Summon", 2));
            yield return ExpectNotLog("Boss entering state Shot Antic", 2);
        }
    }
}
