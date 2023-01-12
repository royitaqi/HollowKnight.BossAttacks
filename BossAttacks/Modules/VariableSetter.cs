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
    internal VariableSetter(VariableSetterConfig config)
    {
        _config = config;
    }

    protected override bool OnLoad(Scene scene)
    {
        this.LogMod($"Loading for scene {scene.name}");

        if (!base.LoadSingleStateObjects(scene, _config))
        {
            return false;
        }

        _state.InsertMethod(() => {
            foreach (var kv in _config.BoolVariables)
            {
                _fsm.FsmVariables.GetFsmBool(kv.Key).Value = kv.Value;
            }
            foreach (var kv in _config.IntVariables)
            {
                _fsm.FsmVariables.GetFsmInt(kv.Key).Value = kv.Value;
            }
        }, 0);
        _state.Actions[0].Name = "VariableSetter";

        return true;
    }

    protected override void OnUnload()
    {
        _state.RemoveActionByName("VariableSetter");
    }

    private VariableSetterConfig _config;
}
