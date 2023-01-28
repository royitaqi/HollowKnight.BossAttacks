using System.Collections;
using System.Linq;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestFailedChampion : E2eBossFightTest
    {
        protected override string BossScene => "GG_Failed_Champion";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 5,
            IdleLog = "Boss entering state Move Choice SCP", // cannot use SCP state, because after switching to level 1 then level 0, the PrintStates won't be able to print the SCP state which is added by the AttackSelector.
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "JUMP", Log = "Boss entering state Jump Antic", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "JUMP ATTACK", Log = "Boss entering state JA Antic", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "SMASH", Log = "Boss entering state S Antic", Duration = 10,  RepeatTimes = 2 },
            }
        };

        protected override IEnumerator BossFightScript() => TestFreeAttacks();
    }
}
