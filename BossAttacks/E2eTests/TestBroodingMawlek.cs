using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestBroodingMawlek : E2eBossFightTest
    {
        protected override string BossScene => "GG_Brooding_Mawlek";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 10,
            IdleLog = "Boss entering state Super Select SCP",
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "JUMP", Log = "Boss entering state Super Jump", Duration = 10,  RepeatTimes = 4 },
                new BossAttackMetadata { Name = "SPIT", Log = "Boss entering state Shoot", Duration = 10,  RepeatTimes = 4 },
            }
        };

        protected override IEnumerator BossFightScript() => TestFreeAttacks();
    }
}
