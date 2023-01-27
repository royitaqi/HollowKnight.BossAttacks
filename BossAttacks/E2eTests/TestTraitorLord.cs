using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestTraitorLord : E2eBossFightTest
    {
        protected override string BossScene => "GG_Traitor_Lord";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 10,
            IdleLog = "Boss entering state Attack Choice SCP",
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "DSLASH", Log = "Boss entering state Jump Antic", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "SLASH", Log = "Boss entering state Attack Antic", Duration = 5,  RepeatTimes = 2 },
            }
        };

        protected override IEnumerator BossFightScript() => TestFreeAttacks();
    }
}
