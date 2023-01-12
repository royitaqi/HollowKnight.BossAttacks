namespace UnitTests
{
    [TestClass]
    public class TestDefaultConfig
    {
        [TestMethod]
        public void TestPropagation()
        {
            var d = new DefaultConfig { GoName = "go", FsmName = "fsm" };
            var gas = new GenericAttackSelectorConfig();

            ModuleManager.PropagateConfig(d, gas);
            Assert.AreEqual(d.GoName, gas.GoName);
            Assert.AreEqual(d.FsmName, gas.FsmName);
            Assert.AreEqual(null, gas.StateName);
        }
    }
}
