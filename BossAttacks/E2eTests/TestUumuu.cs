using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestUumuu : E2eBossFightTest
    {
        protected override string BossScene => "GG_Uumuu";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 10,
            ScpLog = "Boss entering state Choice SCP",
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "CHASE", Log = "Boss entering state Zapping", Duration = 10,  RepeatTimes = 6 }, // 3 Zapping per attack
                new BossAttackMetadata { Name = "MULTIZAP", Log = "Boss entering state Multizap", Duration = 10,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "SPAWN", Log = "Boss entering state Roar", Duration = 5,  RepeatTimes = 2 },
            }
        };

        protected override IEnumerator BossFightScript() => TestFreeAttacks();
    }
}
