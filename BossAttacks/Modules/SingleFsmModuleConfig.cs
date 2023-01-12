using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal abstract class SingleFsmModuleConfig : ModuleConfig
{
    public string GoName { get; set; }
    public string FsmName { get; set; }
    public abstract Type ModuleType { get; }
}