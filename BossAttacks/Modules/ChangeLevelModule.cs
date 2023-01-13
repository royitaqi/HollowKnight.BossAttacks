using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BossAttacks.Utils;
using HutongGames.PlayMaker;
using SFCore.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal class ChangeLevelModule : SingleFsmModule
{
    public ChangeLevelModule(Scene scene, ChangeLevelModuleConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
        _mgr = mgr;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading module");

    }

    protected override void OnUnload()
    {
    }

    private Scene _scene;
    private ChangeLevelModuleConfig _config;
    private ModuleManager _mgr;
}