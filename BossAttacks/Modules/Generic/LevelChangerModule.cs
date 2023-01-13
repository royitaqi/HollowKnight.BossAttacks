using BossAttacks.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

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
        this.LogMod($"Loading (L = {L}..{H})");

        Option opt = _config.Reversible ? new BooleanOption() : new MonoOption();
        opt.Display = _config.Display;
        _targetL = _config.TargetL;
        opt.Interacted += () =>
        {
            this.LogModDebug($"Changing level to {_targetL}");
            _targetL = _mgr.ChangeLevel(_targetL);

            if (!_config.Reversible)
            {
                // Stop interaction if irreversible
                opt.Interactive = false;
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
    private LevelChangerModuleConfig _config;
    private ModuleManager _mgr;
    private int _targetL;
}