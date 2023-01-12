using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal class PrintStatesModuleConfig : SingleFsmModuleConfig
{
    public override Type ModuleType { get => typeof(PrintStatesModule); }
}