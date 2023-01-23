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
            TestCase("wait for fight and modules to load");
            yield return InvincibleHero();
            yield return EnterFightViaStatueGo();
            yield return ExpectLog("[ModuleManager] Level is now 0", 10);

            TestCase("turn off all attacks");
            yield return PressKey(KeyCode.Alpha1, 0.1f);
            yield return PressKey(KeyCode.Alpha2, 0.1f);

            TestCase("wait for boss idle #1");
            yield return ExpectLog("Boss entering state Super Choose SCP", 5);
            InParallel(ExpectNotLog("Boss entering state Charge Antic", 2));
            yield return ExpectNotLog("Boss entering state Slam Antic", 2);

            TestCase("test attack 1 - CHARGE #1");
            yield return PressKey(KeyCode.Alpha1, 0.1f);
            yield return ExpectLog("Boss entering state Charge Antic", 5);
            TestCase("test attack 1 - CHARGE #2");
            yield return ExpectLog("Boss entering state Charge Antic", 5);
            TestCase("test attack 1 - CHARGE #3");
            yield return ExpectLog("Boss entering state Charge Antic", 5);
            yield return PressKey(KeyCode.Alpha1, 0.1f);

            TestCase("wait for boss idle #2");
            yield return ExpectLog("Boss entering state Super Choose SCP", 5);
            InParallel(ExpectNotLog("Boss entering state Charge Antic", 2));
            yield return ExpectNotLog("Boss entering state Slam Antic", 2);

            TestCase("test attack 2 - SLAM #1");
            yield return PressKey(KeyCode.Alpha2, 0.1f);
            yield return ExpectLog("Boss entering state Slam Antic", 20);
            TestCase("test attack 2 - SLAM #2");
            yield return ExpectLog("Boss entering state Slam Antic", 20);
            TestCase("test attack 2 - SLAM #3");
            yield return ExpectLog("Boss entering state Slam Antic", 20);
            yield return PressKey(KeyCode.Alpha2, 0.1f);

            TestCase("wait for boss idle #3");
            yield return ExpectLog("Boss entering state Super Choose SCP", 20);
            InParallel(ExpectNotLog("Boss entering state Charge Antic", 2));
            yield return ExpectNotLog("Boss entering state Slam Antic", 2);

            TestCase("leave fight");
            yield return LeaveFight();
            yield return RecoverInvincibility();

            yield return 0;
        }
    }
}
