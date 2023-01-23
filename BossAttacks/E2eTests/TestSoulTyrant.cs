using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class TestSoulTyrant : E2eBossFightTest
    {
        protected override string BossScene => "GG_Soul_Tyrant";
        protected override Vector3 StatuePos => new Vector3(0, 0, 0);
        protected override int ChallengeLevel => 0;
        protected override string ReturnDoor => "TBD";

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
