using BossAttacks.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class LevelChanger : SingleFsmModule
{
    public LevelChanger(Scene scene, LevelChangerConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
        _mgr = mgr;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading");

        Option opt = _config.Mode == LevelChangerConfig.Modes.Bidirection ? new BooleanOption() : new MonoOption();
        opt.Display = _config.Display;
        _targetL = _config.TargetL;
        opt.Interacted += () =>
        {
            this.LogModDebug($"Changing level to {_targetL}");
            var originalL = _mgr.ChangeLevel(_targetL);

            if (_config.Mode == LevelChangerConfig.Modes.OneTime)
            {
                opt.Interactive = false;
            }
            else if (_config.Mode == LevelChangerConfig.Modes.Bidirection)
            {
                _targetL = originalL;
            }
        };
        _options.Add(opt);
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
        _targetL = 0;
    }

    private Scene _scene;
    private LevelChangerConfig _config;
    private ModuleManager _mgr;
    private int _targetL;
}