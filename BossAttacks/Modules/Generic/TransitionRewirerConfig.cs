using System;

namespace BossAttacks.Modules.Generic;

internal class TransitionRewirerConfig : SingleStateModuleConfig
{
    public override Type ModuleType => typeof(TransitionRewirer);
    public string EventName { get; set; }
    public string ToState { get; set; }
}
