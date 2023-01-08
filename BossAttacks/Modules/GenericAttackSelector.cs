using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using BossAttacks.Utils;
using SFCore.Utils;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BossAttacks.Modules;

/**
 * The basic unit of modding the game logic.
 * 
 * Can be loaded/unloaded (from mod menu; from entering/leaving boss scene).
 */
internal class GenericAttackSelector : Module
{
    public override string Name => "Attacks";

    public override bool Load(Scene scene)
    {
        this.LogMod($"Loading for scene {scene.name}");

        // If there is no mapping, cannot find a config.
        if (!GodhomeUtils.SceneToBoss.ContainsKey(scene.name))
        {
            this.LogModWarn($"Scene {scene.name} not configured.");
            return false;
        }
        var config = GodhomeUtils.SceneToBoss[scene.name];
        if (config == null)
        {
            this.LogModWarn($"Scene {scene.name} configured to be null.");
            return false;
        }


        // Inspect GO and FSM, and set up.
        var go = scene.Find(config.GoName);
        if (go == null)
        {
            this.LogModWarn($"Cannot find GO {config.GoName} in scene");
            return false;
        }
        this.LogModDebug($"GO = {go.name}");
            
        var fsm = go.LocateMyFSM(config.FsmName);
        if (fsm == null)
        {
            this.LogModWarn($"Cannot find FSM {config.FsmName} in GO");
            return false;
        }
        this.LogModDebug($"FSM = {fsm.FsmName}");

        FsmState[] states;
        if (config.StateNames == null)
        {
            states = fsm.FsmStates.Where(s => s.Name.EndsWith("Choice") || s.Name.EndsWith("Decision")).ToArray();
            if (states.Length != 1)
            {
                this.LogModWarn($"Cannot find Choice/Decision state. Candidates are: ({states.Length}) {String.Join(", ", states.Select(s => s.Name))}.");
                return false;
            }
        } else
        {
            states = fsm.FsmStates.Where(s => config.StateNames.Contains(s.Name)).ToArray();
            if (states.Length != config.StateNames.Length)
            {
                this.LogModWarn($"Some of the states cannot be found. Configured: ({config.StateNames.Length}) {String.Join(", ", config.StateNames)}. Found: ({states.Length}) {String.Join(", ", states.Select(s => s.Name))}.");
                return false;
            }
        }
        this.LogModDebug($"States: ({states.Length}) {String.Join(", ", states.Select(s => s.Name))}");

        _originalTransition = states
            .Select(s => new
            {
                K = s.Name,
                V = s.Transitions.Select(t => new { K = t.EventName, V = t.ToState }).ToDictionary(kv => kv.K, kv => kv.V),
            })
            .ToDictionary(kv => kv.K, kv => kv.V);

        var eventNames = states.SelectMany(s => s.Transitions.Select(t => t.EventName)).Distinct().OrderBy(n => n);
        _booleanOptions = new();
        foreach (var eventName in eventNames)
        {
            this.LogModDebug($"Event: {eventName}");

            var opt = new Option<bool>
            {
                Value = true,
                CanSet = (attackIsOn) =>
                {
                    // If turning off current attack AND there are some state which cannot turn off this attack, cannot set.
                    if (!attackIsOn && states.Any(s => !CanTurnOffAttack(s, eventName)))
                    {
                        return false;
                    }

                    // Turn off current attack in all states:
                    // - If attack is on, connect the attack choice to the actual attack.
                    // - Otherwise, connect the attack choice backto the Choice state itself.
                    foreach (var s in states)
                    {
                        TurnAttack(s, eventName, attackIsOn);
                    }
                    return true;
                }
            };

            _booleanOptions.Add(eventName, opt);
        }

        return true;
    }

    // Helper functions
    private bool CanTurnOffAttack(FsmState state, string eventName)
    {
        return state.Transitions.Select(t => t.EventName).Where(n => n != eventName).Count(n => _booleanOptions[n].Value) > 0;
    }

    private void TurnAttack(FsmState state, string eventName, bool onOff)
    {
        // Skip if the state doesn't have this event
        if (!_originalTransition[state.Name].ContainsKey(eventName))
        {
            return;
        }

        var toStateName = onOff ? _originalTransition[state.Name][eventName] : state.Name;
        state.ChangeTransition(eventName, toStateName);
        this.LogModDebug($"Changing transition: {state.Name}.{eventName} -> {toStateName}");
    }

    public override Dictionary<string, Option<bool>> GetBooleanOptions()
    {
        return _booleanOptions;
    }

    public override void Unload()
    {
        this.LogMod("Unload()");

        if (_booleanOptions != null)
        {
            foreach (var kvp in _booleanOptions)
            {
                // Turn all attacks back on when the module is unloaded.
                kvp.Value.Value = true;
            }
            _booleanOptions = null;
        }

        _originalTransition = null;
    }

    private Dictionary<string, Option<bool>> _booleanOptions = null;
    // fromState -> event -> toState
    private Dictionary<string, Dictionary<string, string>> _originalTransition = null;
}
