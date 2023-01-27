using System;
using System.Collections;
using System.Diagnostics.Contracts;
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
            success = new Counter { InProgress = true };
            failure = new Counter { InProgress = true };
            CountLogInParallel("TEST SUCCEEDED", float.MaxValue, success);
            CountLogInParallel(ToContain, "TEST FAILED with", float.MaxValue, failure);

            foreach (var type in ToTest)
            {
                TestCase(type.Name);
                E2eBossFightTest inst = Activator.CreateInstance(type) as E2eBossFightTest;
                yield return inst.FullTest();

                TestCase("Intermission");
                yield return new WaitForSeconds(0.2f);
                this.LogModTest($"Overall progress update:{(success.Value > 0 ? $"  ✅ {success.Value}" : "")}{(failure.Value > 0 ? $"  ❌ {failure.Value}" : "")}");
            }

            success.InProgress = false;
            failure.InProgress = false;
            yield return new WaitForSeconds(0.2f);
        }

        protected override IEnumerator Conclude()
        {
            TestCase("Conclude");
            this.LogModTest("Concluding");

            if (failure.Value == 0)
            {
                this.LogModTest($"TEST SUCCEEDED:  ✅ {success.Value}");
            }
            else
            {
                this.LogModTest($"TEST FAILED:{(success.Value > 0 ? $"  ✅ {success.Value}" : "")}{(failure.Value > 0 ? $"  ❌ {failure.Value}" : "")}");
            }

            success = null;
            failure = null;
            yield return 0;
        }

        private Counter success;
        private Counter failure;
    }
}
