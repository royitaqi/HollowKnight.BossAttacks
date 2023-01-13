using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using BossAttacks.Utils;
using SFCore.Utils;
using System.Linq;
using System;
using System.Reflection;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace BossAttacks.Modules;

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

        _state.InsertMethod(() => {
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
        }, 0);
        _state.Actions[0].Name = "VariableSetter";
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
        _state.RemoveActionByName("VariableSetter");
    }

    private Scene _scene;
    private VariableSetterConfig _config;
}
