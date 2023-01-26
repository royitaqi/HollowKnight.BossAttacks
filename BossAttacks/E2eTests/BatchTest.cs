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
            typeof(TestGruzMother),
            typeof(TestPureVessel),
            typeof(TestSoulTyrant),
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
