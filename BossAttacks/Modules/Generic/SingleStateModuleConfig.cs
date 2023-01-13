namespace BossAttacks.Modules.Generic;

internal abstract class SingleStateModuleConfig : SingleFsmModuleConfig
{
    public string StateName { get; set; }
}