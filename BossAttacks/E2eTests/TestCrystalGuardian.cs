using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestCrystalGuardian : E2eBossFightTest
    {
        protected override string BossScene => "GG_Crystal_Guardian";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 5,
            ScpLog = "Boss entering state Choice SCP",
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "ATTACK", Log = "Boss entering state Range Check", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "JUMP", Log = "Boss entering state Aim Jump", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "ROAR", Log = "Boss entering state Roar Start", Duration = 5,  RepeatTimes = 2 },
            }
        };

        protected override IEnumerator BossFightScript() => TestFreeAttacks();
    }
}
