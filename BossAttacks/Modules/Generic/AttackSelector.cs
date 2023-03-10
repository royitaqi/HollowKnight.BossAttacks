using System.Linq;
using BossAttacks.Utils;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class AttackSelector : SingleStateModule
{
    public const string SHORT_CIRCUIT_PROTECTION_SUFFIX = " SCP";

    public AttackSelector(Scene scene, AttackSelectorConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name}");

        LoadSingleStateObjects(_scene, _config);

        string skipToState = null;
        if (_config.SkipToState != null)
        {
            skipToState = _config.SkipToState;
        }
        else
        {
            // Short circuit protection (SCP).
            // * Short circuit is when the Choice state has all the events connected back to itself, causing an infinite loop where the boss takes no action.
            skipToState = _state.Name + SHORT_CIRCUIT_PROTECTION_SUFFIX;
            var scpState = _fsm.AddState(skipToState);
            scpState.AddAction(new ShortCircuitProtectionAction());
            scpState.AddTransition("FINISHED", _state.Name);
        }

        foreach (var tran in _state.Transitions)
        {
            var eventName = tran.EventName;
            var attackName = _config.MapEvent(eventName);
            // Ignore specified events
            if (_config.IgnoreEvent(eventName))
            {
                continue;
            }

            var originalToStateName = tran.ToState;
            var opt = new BooleanOption { Display = attackName };
            opt.Interact(); // set value to true
            opt.Interacted += () =>
            {
                var toStateName = opt.Value ? originalToStateName : skipToState;
                _state.ChangeTransition(eventName, toStateName);
                this.LogModDebug($"Turning attack {attackName} to {(opt.Value ? "ON" : "OFF")} ({_state.Name}.{eventName} -> {toStateName})");
            };
            _options.Add(opt);
        }
        _options.Sort((a, b) => string.Compare(a.Display, b.Display));
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");

        // Turn all attacks back on when the module is unloaded.
        foreach (var o in _options)
        {
            var opt = o as BooleanOption;
            if (!opt.Value)
            {
                opt.Interact();
            }
        }

        // Remove SCP state
        _fsm.RemoveState(_config.StateName + SHORT_CIRCUIT_PROTECTION_SUFFIX);

        UnloadSingleStateObjects();
    }

    private Scene _scene;
    private AttackSelectorConfig _config;
}
