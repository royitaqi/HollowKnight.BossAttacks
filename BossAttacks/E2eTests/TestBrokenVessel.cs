using System.Collections;
using System.Linq;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestBrokenVessel : E2eBossFightTest
    {
        protected override string BossScene => "GG_Broken_Vessel";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 10,
            IdleLog = "Boss entering state Attack Choice", // cannot use SCP state, because after switching to level 1 then level 0, the PrintStates won't be able to print the SCP state which is added by the AttackSelector.
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "DASH", Log = "Boss entering state Dash Antic 2", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "DOWNSTAB", Log = "Boss entering state Dstab Antic", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "JUMP", Log = "Boss entering state Land", Duration = 5,  RepeatTimes = 2 },
            }
        };

        protected override IEnumerator BossFightScript()
        {
            yield return TestFreeAttacks();

            // SHAKE
            TestCase($"test attack 4");
            yield return PressKey(KeyCode.Alpha4, 0.1f);
            TestCase($"test attack 4 - SHAKE #1");
            yield return ExpectLog("Boss entering state Shake Antic", _lastAttackDuration + BossMeta.Attacks.Max(a => a.Duration));
            _lastAttackDuration = 15; // this attack is going on
            TestCase($"test attack 4 - SHAKE #2");
            yield return ExpectLog("Boss entering state Shake Antic", _lastAttackDuration);
            _lastAttackDuration = 15; // this attack is going on
            
            // exit SHAKE EXCLUSIVE mode
            yield return PressKey(KeyCode.Alpha1, 0.1f);

            // turn off all attacks
            yield return TurnOffAllAttacks();

            _lastAttackDuration += BossMeta.Attacks.Max(a => a.Duration);
            yield return WaitForBossIdle("#5");
        }
    }
}
