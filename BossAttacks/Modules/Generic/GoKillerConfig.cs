using System;

namespace BossAttacks.Modules.Generic;

internal class GoKillerConfig : SingleFsmModuleConfig // Should be Single"Go"ModuleConfig, but we don't have that, so just use this for now
{
    public override Type ModuleType => typeof(GoKiller);
}
