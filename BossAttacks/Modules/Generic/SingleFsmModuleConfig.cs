namespace BossAttacks.Modules.Generic;

internal abstract class SingleFsmModuleConfig : ModuleConfig
{
    public string GoName { get; set; }
    public string FsmName { get; set; }
}