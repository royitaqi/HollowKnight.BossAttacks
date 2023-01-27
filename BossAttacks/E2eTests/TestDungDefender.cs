using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestDungDefender : E2eBossFightTest
    {
        protected override string BossScene => "GG_Dung_Defender";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 20,
            IdleLog = "Boss entering state Move Choice SCP",
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "DIVE IN", Log = "Boss entering state Dive In 1", Duration = 10,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "DOLPHIN", Log = "Boss entering state Dolph Antic", Duration = 10,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "ROLL JUMP", RepeatTimes = 2, Parts = new[] {
                    new BossAttackMetadata { Log = "Boss entering state Throw Antic 1", Duration = 5 },
                    new BossAttackMetadata { Log = "Boss entering state RJ Antic", Duration = 15 },
                } },
                new BossAttackMetadata { Name = "THROW DIVE", RepeatTimes = 2, Parts = new[] {
                    new BossAttackMetadata { Log = "Boss entering state Throw Antic 1", Duration = 5 },
                    new BossAttackMetadata { Log = "Boss entering state Dive In 1", Duration = 10 },
                } },
                new BossAttackMetadata { Name = "THROW DOLPH", RepeatTimes = 2, Parts = new[] {
                    new BossAttackMetadata { Log = "Boss entering state Throw Antic 1", Duration = 5 },
                    new BossAttackMetadata { Log = "Boss entering state Dolph Antic", Duration = 10 },
                } },
            }
        };

        protected override IEnumerator BossFightScript()
        {
            yield return TestFreeAttacks();

            // Trim ROLL JUMP
            TestCase($"test Trim ROLL JUMP");
            yield return PressKey(KeyCode.Alpha6, 0.1f);
            yield return PressKey(KeyCode.Alpha3, 0.1f);
            _lastAttackDuration = 15; // this attack is going on
            TestCase($"test Trim ROLL JUMP - #1");
            ExpectNoLogInParallel("Boss entering state Throw Antic 1", _lastAttackDuration);
            yield return ExpectLog("Boss entering state RJ Antic", _lastAttackDuration);
            TestCase($"test Trim ROLL JUMP - #2");
            ExpectNoLogInParallel("Boss entering state Throw Antic 1", _lastAttackDuration);
            yield return ExpectLog("Boss entering state RJ Antic", _lastAttackDuration);
            yield return PressKey(KeyCode.Alpha6, 0.1f);
            yield return PressKey(KeyCode.Alpha3, 0.1f);

            yield return WaitForBossIdle("#7");
        }
    }
}
