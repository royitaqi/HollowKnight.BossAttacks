using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestMassiveMossCharger : E2eBossFightTest
    {
        protected override string BossScene => "GG_Mega_Moss_Charger";

        protected override BossMetadata BossMeta => new BossMetadata
        {
            StartIdle = 10,
            IdleLog = "Boss entering state Attack Choice SCP",
            Attacks = new[]
            {
                new BossAttackMetadata { Name = "CHARGE", Log = "Boss entering state Charge", Duration = 5,  RepeatTimes = 2 },
                new BossAttackMetadata { Name = "LEAP", Log = "Boss entering state Leap Start", Duration = 5,  RepeatTimes = 2 },
            }
        };

        protected override IEnumerator BossFightScript() => TestFreeAttacks();
    }
}
