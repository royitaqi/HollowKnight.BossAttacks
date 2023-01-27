using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestLostKin : E2eBossFightTest
    {
        protected override string BossScene => "GG_Lost_Kin";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 10,
            IdleLog = "Boss entering state Attack Choice SCP",
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "DASH", Log = "Boss entering state Dash Antic 2", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "DOWNSTAB", Log = "Boss entering state Dstab Antic", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "JUMP", Log = "Boss entering state Land", Duration = 5,  RepeatTimes = 2 },
            }
        };

        protected override IEnumerator BossFightScript() => TestFreeAttacks();
    }
}
