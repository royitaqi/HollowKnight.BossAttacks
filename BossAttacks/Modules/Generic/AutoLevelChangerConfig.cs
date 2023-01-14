using System;

namespace BossAttacks.Modules.Generic;

internal class AutoLevelChangerConfig : SingleFsmModuleConfig
{
    public override Type ModuleType => typeof(AutoLevelChanger);

    /**
     * What level to change into when condition is met.
     */
    public int TargetL { get; set; }

    /**
     * Condition: when an FSM state is entered.
     */
    public string OnEnterState { get; set; }
}
