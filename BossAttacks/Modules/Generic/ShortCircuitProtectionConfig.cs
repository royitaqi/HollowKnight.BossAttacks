using System;
using System.Collections.Generic;

namespace BossAttacks.Modules.Generic;

internal class ShortCircuitProtectionConfig : SingleStateModuleConfig
{
    public override Type ModuleType => typeof(ShortCircuitProtection);
    public string ScpStateName { get; set; }
}
