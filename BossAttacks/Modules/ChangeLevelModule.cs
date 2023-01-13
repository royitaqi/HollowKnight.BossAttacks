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

        Option opt = _config.Reversable ? new MonoOption() : new BooleanOption();
        opt.Display = _config.OptionDisplay;
        _targetL = _config.TargetL;
        opt.Interacted += () =>
        {
            _targetL = _mgr.ChangeLevel(_targetL);
        };
    }

    protected override void OnUnload()
    {
    }

    private Scene _scene;
    private ChangeLevelModuleConfig _config;
    private ModuleManager _mgr;
    private int _targetL;
}