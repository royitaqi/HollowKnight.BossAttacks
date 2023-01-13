using System;

namespace BossAttacks.Modules.Generic;

internal class PrintStatesModuleConfig : SingleFsmModuleConfig
{
    public override Type ModuleType => typeof(PrintStatesModule);
    public bool Verbose { get; set; }
}
