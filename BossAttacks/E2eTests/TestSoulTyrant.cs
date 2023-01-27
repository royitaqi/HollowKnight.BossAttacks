using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestSoulTyrant : E2eBossFightTest
    {
        protected override string BossScene => "GG_Soul_Tyrant";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 5,
            IdleLog = "Boss entering state Attack Choice SCP",
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "CHARGE", Log = "Boss entering state Charge Antic", Duration = 10,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "DIVE", Log = "Boss entering state Quake Antic", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "HIGH SPINNER", Log = "Boss entering state HS Summon", Duration = 10,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "SHOOT", Log = "Boss entering state Shot Antic", Duration = 5,  RepeatTimes = 2 },
            }
        };

        protected override IEnumerator BossFightScript()
        {
            yield return TestFreeAttacks();

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
            ExpectNoLogInParallel("Boss entering state Quake Antic", 5);
            yield return ExpectLog("Boss entering state Shot Antic", 5);
            yield return ExpectLog("Boss entering state Quake Antic", 20);
        }
    }
}
