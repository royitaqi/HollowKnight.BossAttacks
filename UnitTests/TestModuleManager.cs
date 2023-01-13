using BossAttacks.Modules.Generic;

namespace UnitTests
{
    [TestClass]
    public class TestModuleManager
    {
        [TestMethod]
        public void TestPropagateConfig()
        {
            var dict = new Dictionary<string, object>();
            var configs = new GenericAttackSelectorConfig[] {
                new GenericAttackSelectorConfig { }, // null start
                new GenericAttackSelectorConfig { GoName = "go" }, // provide one
                new GenericAttackSelectorConfig { FsmName = "fsm" }, // accept one, provide another
                new GenericAttackSelectorConfig { H = 1 }, // accept two, provide int (which should not propagate because it's non-null)
                new GenericAttackSelectorConfig { FsmName = "fsm1" }, // override one
            };

            foreach (var c in configs)
            {
                ModuleManager.PropagateConfig(dict, c);
            }

            var last = configs.Last();
            Assert.AreEqual(0, last.L);
            Assert.AreEqual(0, last.H);
            Assert.AreEqual("go", last.GoName);
            Assert.AreEqual("fsm1", last.FsmName);
            Assert.AreEqual(null, last.StateName);
        }

        [TestMethod]
        public void TestCreatePrintStatesModules()
        {
            var configs = new ModuleConfig[] {
                new GenericAttackSelectorConfig { GoName = "go1", FsmName = "fsm1" },
                new GenericAttackSelectorConfig { L = 0, H = 1, GoName = "go1", FsmName = "fsm1" },
                new GenericAttackSelectorConfig { L = 1, H = 1, GoName = "go2", FsmName = "fsm2" },
            };

            var ret = ModuleManager.GetPrintStatesModuleConfigs(configs).ToArray();

            Assert.AreEqual(2, ret.Length);
            Assert.AreEqual(0, ret[0].L);
            Assert.AreEqual(1, ret[0].H);
            Assert.AreEqual("go1", ret[0].GoName);
            Assert.AreEqual("fsm1", ret[0].FsmName);
            Assert.AreEqual(0, ret[1].L);
            Assert.AreEqual(1, ret[1].H);
            Assert.AreEqual("go2", ret[1].GoName);
            Assert.AreEqual("fsm2", ret[1].FsmName);
        }
    }
}
