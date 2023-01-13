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

internal class LevelChangerModule : SingleFsmModule
{
    public LevelChangerModule(Scene scene, LevelChangerModuleConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
        _mgr = mgr;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading");

        Option opt = _config.Reversable ? new BooleanOption() : new MonoOption();
        opt.Display = _config.Display;
        _targetL = _config.TargetL;
        opt.Interacted += () =>
        {
            this.LogModDebug($"Changing level to {_targetL}");
            _targetL = _mgr.ChangeLevel(_targetL);
        };
        _options.Add(opt);
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
    }

    private Scene _scene;
    private LevelChangerModuleConfig _config;
    private ModuleManager _mgr;
    private int _targetL;
}