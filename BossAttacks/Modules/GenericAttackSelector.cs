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

internal class GenericAttackSelector : SingleStateModule
{
    //public override string Name => "Select Attacks";

    const string SHORT_CIRCUIT_PROTECTION_SUFFIX = " SCP";

    public GenericAttackSelector(GenericAttackSelectorConfig config)
    {
        _config = config;
    }

    protected override void OnLoad(Scene scene)
    {
        this.LogMod($"Loading for scene {scene.name}");

        LoadSingleStateObjects(scene, _config);

        // Short circuit protection (SCP).
        // * Short circuit is when the Choice state has all the events connected back to itself, causing an infinite loop where the boss takes no action.
        var scpStateName = _state.Name + SHORT_CIRCUIT_PROTECTION_SUFFIX;
        var scpState = _fsm.AddState(scpStateName);
        scpState.AddAction(new ShortCircuitProtectionAction());
        scpState.AddTransition("FINISHED", _state.Name);

        foreach (var tran in _state.Transitions)
        {
            var eventName = tran.EventName;
            // Ignore specified events
            if (_config.IgnoreEvent(eventName))
            {
                continue;
            }

            var originalToStateName = tran.ToState;
            var opt = new BooleanOption { Display = _config.MapEvent(eventName), Value = true };
            opt.Mutated += () =>
            {
                var toStateName = opt.Value ? originalToStateName : scpStateName;
                _state.ChangeTransition(eventName, toStateName);
                this.LogModDebug($"Changing transition: {_state.Name}.{eventName} -> {toStateName}");
            };
            _options.Add(opt);
        }
    }

    protected override void OnUnload()
    {
        this.LogMod("Unload()");

        // Turn all attacks back on when the module is unloaded.
        foreach (var o in _options)
        {
            var opt = o as BooleanOption;
            if (!opt.Value)
            {
                opt.Value = true;
            }
        }

        // Remove SCP states
        _fsm.RemoveState(_config.StateName + SHORT_CIRCUIT_PROTECTION_SUFFIX);

        base.UnloadSingleStateObjects();
    }

    private GenericAttackSelectorConfig _config;
}
