using System;
using System.Collections.Generic;

namespace BossAttacks.Modules.Generic;

internal class VariableSetterConfig : SingleStateModuleConfig
{
    public override Type ModuleType => typeof(VariableSetter);
    internal KeyValuePair<string, int>[] IntVariables { get; set; }
    internal KeyValuePair<string, bool>[] BoolVariables { get; set; }
    public Type ActionType { get; set; }
    public int IndexDelta { get; set; }
}
