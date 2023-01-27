using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestPureVessel : E2eBossFightTest
    {
        protected override string BossScene => "GG_Hollow_Knight";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 20,
            IdleLog = "Boss entering state Choice P3 SCP",
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "DASH", Log = "Boss entering state Dash Antic", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "DSTAB", Log = "Boss entering state Stomp Antic", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "FOCUS", Log = "Boss entering state Focus Charge", Duration = 10,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "SLASH", Log = "Boss entering state Slash1 Antic", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "SMALL SHOT", Log = "Boss entering state SmallShot Antic", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "TENDRIL", Log = "Boss entering state Tendril Antic", Duration = 5,  RepeatTimes = 2 },
            }
        };

        protected override IEnumerator BossFightScript() => TestFreeAttacks();
    }
}
