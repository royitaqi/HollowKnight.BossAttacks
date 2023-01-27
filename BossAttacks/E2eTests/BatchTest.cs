using System;
using System.Collections;
using BossAttacks.Utils;
using UnityEngine;

namespace BossAttacks.E2eTests
{
    internal class BatchTest: E2eTest
    {
        private Type[] ToTest => new[]
        {
            typeof(TestCrystalGuardian),
            typeof(TestEnragedGuardian),
            typeof(TestGodTamer),
            typeof(TestGruzMother),
            typeof(TestLostKin),
            typeof(TestMegaMossCharger),
            typeof(TestPureVessel),
            typeof(TestSoulTyrant),
            typeof(TestTraitorLord),
            typeof(TestUumuu),
            typeof(TestUumuuV),
        };

        protected override IEnumerator Script()
        {
            foreach (var type in ToTest)
            {
                TestCase(type.Name);
                E2eBossFightTest inst = Activator.CreateInstance(type) as E2eBossFightTest;
                yield return inst.FullTest();
            }
        }
    }
}
