using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal abstract class SingleStateModuleConfig : SingleFsmModuleConfig
{
    public string StateName { get; set; }
}