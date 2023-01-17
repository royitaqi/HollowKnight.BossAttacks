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
        _state.InsertMethodWithName(() =>
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
            if (_config.FloatVariables != null)
            {
                foreach (var kv in _config.FloatVariables)
                {
                    _fsm.FsmVariables.GetFsmFloat(kv.Key).Value = kv.Value;
                }
            }
        }, index, ID);
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
        _state.RemoveActionByName(ID);
    }

    private Scene _scene;
    private VariableSetterConfig _config;
}
