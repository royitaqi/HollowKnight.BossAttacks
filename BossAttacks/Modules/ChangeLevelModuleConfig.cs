using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal class ChangeLevelModuleConfig : ModuleConfig
{
    public override Type ModuleType => typeof(ChangeLevelModule);
}
