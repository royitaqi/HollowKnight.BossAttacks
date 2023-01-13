using System;

namespace BossAttacks.Modules;

/**
 * Represents all module configs
 */
internal class ModuleConfig {
    public virtual Type ModuleType => null;
    public int L { get; set; }
    public int H { get; set; }
}
