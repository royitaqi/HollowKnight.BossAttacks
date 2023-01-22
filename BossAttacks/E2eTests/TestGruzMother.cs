using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestGruzMother : E2eBossFightTest
    {
        protected override string BossScene => "GG_Gruz_Mother";
        protected override Vector3 StatuePos => new Vector3(28, 6.4f, 0);
        protected override int ChallengeLevel => 0;
        protected override string ReturnDoor => "door_dreamReturnGG_GG_Statue_Gruz";

        internal override IEnumerator Script()
        {
            //yield return EnterFight();
            //yield return new WaitForSeconds(5f);
            //yield return InterceptLog();
            //yield return Left(3);
            //yield return ExpectLog("Overriding left", 5);
            yield return 0;
        }
    }
}
