using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

/**
 * Represents all module configs
 */
internal interface ModuleConfig {
    public abstract Type ModuleType { get; }
}