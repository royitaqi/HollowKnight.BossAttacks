using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestGruzMotherV : E2eBossFightTest
    {
        protected override string BossScene => "GG_Gruz_Mother_V";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 5,
            IdleLog = "Boss entering state Super Choose SCP",
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "CHARGE", Log = "Boss entering state Charge Antic", Duration = 5,  RepeatTimes = 4 },
                new BossAttackMetadata { Name = "SLAM", Log = "Boss entering state Slam Antic", Duration = 20,  RepeatTimes = 3 },
            }
        };

        protected override IEnumerator BossFightScript() => TestFreeAttacks();
    }
}
