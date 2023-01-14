using BossAttacks.Utils;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class AutoLevelChanger : SingleFsmModule
{
    public AutoLevelChanger(Scene scene, AutoLevelChangerConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
        _mgr = mgr;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading");

        LoadSingleFsmObjects(_scene, _config);

        if (_config.OnEnterState != null)
        {
            var state = _fsm.GetState(_config.OnEnterState);
            state.InsertMethodWithName(() =>
            {
                _mgr.ChangeLevel(_config.TargetL);
            }, 0, "AutoLevelChanger");
        }
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");

        if (_config.OnEnterState != null)
        {
            _fsm.GetState(_config.OnEnterState).RemoveActionByName("AutoLevelChanger");
        }
    }

    private Scene _scene;
    private AutoLevelChangerConfig _config;
    private ModuleManager _mgr;
}