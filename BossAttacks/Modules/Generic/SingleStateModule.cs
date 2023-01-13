using System.Linq;
using BossAttacks.Utils;
using HutongGames.PlayMaker;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal abstract class SingleStateModule : SingleFsmModule
{
    protected void LoadSingleStateObjects(Scene scene, SingleStateModuleConfig config)
    {
        LoadSingleFsmObjects(scene, config);

        if (config.StateName == null)
        {
            var states = _fsm.FsmStates.Where(s => s.Name.EndsWith("Choice") || s.Name.EndsWith("Decision")).ToArray();
            if (states.Length != 1)
            {
                this.LogModError($"Cannot find Choice/Decision state. Candidates are: ({states.Length}) {string.Join(", ", states.Select(s => s.Name))}.");
            }
            _state = states[0];
        }
        else
        {
            _state = _fsm.GetState(config.StateName);
        }
        this.LogModDebug($"State = {_state.Name}");
    }

    protected void UnloadSingleStateObjects()
    {
        _state = null;

        UnloadSingleFsmObjects();
    }

    protected FsmState _state;
}