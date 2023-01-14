using System;

namespace BossAttacks.Modules;

/**
 * Represents all module configs
 */
internal class ModuleConfig {
    public virtual Type ModuleType => null;
    public string ID { get; set; }
    public int L { get; set; }
    public int H { get; set; }
}
