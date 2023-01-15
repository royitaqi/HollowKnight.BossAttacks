using System;

namespace BossAttacks.Modules.Generic;

internal class ActionEnablerConfig : SingleStateModuleConfig
{
    public override Type ModuleType => typeof(ActionEnabler);
    public Type ActionType { get; set; }
    public int IndexDelta { get; set; }
    public bool ToEnabled { get; set; }
}
