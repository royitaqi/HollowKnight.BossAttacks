using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestGodTamer : E2eBossFightTest
    {
        protected override string BossScene => "GG_God_Tamer";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 10,
            IdleLog = "Boss entering state Attack Choice SCP",
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "ROLL CHARGE", Log = "Boss entering state RC Antic", Duration = 10,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "SPIT", Log = "Boss entering state Spit Antic", Duration = 5,  RepeatTimes = 2 },
            }
        };

        protected override IEnumerator BossFightScript() => TestFreeAttacks();
    }
}
