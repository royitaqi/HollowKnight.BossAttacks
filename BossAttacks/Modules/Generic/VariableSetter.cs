using BossAttacks.Utils;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class VariableSetter : SingleStateModule
{
    public VariableSetter(Scene scene, VariableSetterConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name}");

        LoadSingleStateObjects(_scene, _config);

        int index = (_config.ActionType != null ? _state.FindActionIndexByType(_config.ActionType) : 0) + _config.IndexDelta;
        _state.InsertMethod(() =>
        {
            if (_config.BoolVariables != null)
            {
                foreach (var kv in _config.BoolVariables)
                {
                    _fsm.FsmVariables.GetFsmBool(kv.Key).Value = kv.Value;
                }
            }
            if (_config.IntVariables != null)
            {
                foreach (var kv in _config.IntVariables)
                {
                    _fsm.FsmVariables.GetFsmInt(kv.Key).Value = kv.Value;
                }
            }
        }, index);
        _state.Actions[index].Name = "VariableSetter";
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
        _state.RemoveActionByName("VariableSetter");
    }

    private Scene _scene;
    private VariableSetterConfig _config;
}
