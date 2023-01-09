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

internal class GenericAttackSelector : Module
{
    public override string Name => "Select Attacks";

    const string SHORT_CIRCUIT_PROTECTION_SUFFIX = " SCP";

    protected override bool OnLoad(Scene scene)
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
        var go = FindGameObject(scene, config.GoName);
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
        _fsm = fsm;

        FsmState[] states;
        if (config.StateNames == null)
        {
            states = fsm.FsmStates.Where(s => s.Name.EndsWith("Choice") || s.Name.EndsWith("Decision")).ToArray();
            if (states.Length != 1)
            {
                this.LogModWarn($"Cannot find Choice/Decision state. Candidates are: ({states.Length}) {String.Join(", ", states.Select(s => s.Name))}.");
                return false;
            }
        }
        else
        {
            states = fsm.FsmStates.Where(s => config.StateNames.Contains(s.Name)).ToArray();
            if (states.Length != config.StateNames.Length)
            {
                this.LogModWarn($"Some of the states cannot be found. Configured: ({config.StateNames.Length}) {String.Join(", ", config.StateNames)}. Found: ({states.Length}) {String.Join(", ", states.Select(s => s.Name))}.");
                return false;
            }
        }
        this.LogModDebug($"States: ({states.Length}) {String.Join(", ", states.Select(s => s.Name))}");

        // Short circuit protection (SCP).
        // * Short circuit is when the Choice state has all the events connected back to itself, causing an infinite loop where the boss takes no action.
        _scpStates = new();
        foreach (var s in states)
        {
            // Add a SCP state for each Choice state
            var scpState = fsm.AddState(s.Name + SHORT_CIRCUIT_PROTECTION_SUFFIX);
            scpState.AddAction(new ShortCircuitProtectionAction());
            scpState.AddTransition("FINISHED", s.Name);
            _scpStates.Add(scpState);
        }

        _originalTransition = states
            .Select(s => new
            {
                K = s.Name,
                V = s.Transitions.Select(t => new { K = t.EventName, V = t.ToState }).ToDictionary(kv => kv.K, kv => kv.V),
            })
            .ToDictionary(kv => kv.K, kv => kv.V);

        var eventNames = states.SelectMany(s => GetAttackEvents(s)).Distinct().OrderBy(n => n);
        foreach (var eventName in eventNames)
        {
            this.LogModDebug($"Event: {eventName}");

            var opt = new Option<bool>{Value = true};
            opt.OnSet.Add((attackIsOn) =>
            {
                // Turn on/off current attack in all states:
                // - If attack is to be on, connect the attack choice to the actual attack.
                // - Otherwise, connect the attack choice backto the Choice SCP state.
                foreach (var s in states)
                {
                    TurnAttack(s, eventName, attackIsOn);
                }
            });

            _booleanOptions.Add(eventName, opt);
        }

#if (DEBUG)
        // Log boss states as they are being entered.
        // Putting it here at the end of the method, because the method body can be adding states, which should also generate such log.
        foreach (var state in fsm.FsmStates)
        {
            state.InsertMethod(() =>
            {
                this.LogModFine($"Boss entering state {state.Name}");
            }, 0);
        }
#endif

        return true;
    }

    protected override void OnUnload()
    {
        this.LogMod("Unload()");

        // Turn all attacks back on when the module is unloaded.
        foreach (var kvp in _booleanOptions)
        {
            kvp.Value.Value = true;
        }

        // Remove SCP states
        foreach (var s in _scpStates)
        {
            _fsm.RemoveState(s.Name);
        }
        _fsm = null;
        _scpStates = null;

        _originalTransition = null;
    }

    // Helper functions
    private static GameObject FindGameObject(Scene scene, string path)
    {
        string[] parts = path.Split('/');
        var go = scene.Find(parts[0]);
        for (int i = 1; i < parts.Length; i++)
        {
            go = go?.Find(parts[i]);
        }
        return go;
    }

    private static IEnumerable<string> GetAttackEvents(FsmState state)
    {
        var choiceAction = state.Actions.First(a => a.GetType().Name.StartsWith("SendRandomEvent"));
        FsmEvent[] events = choiceAction.GetType().Name switch
        {
            "SendRandomEvent" => (choiceAction as SendRandomEvent).events,
            "SendRandomEventV2" => (choiceAction as SendRandomEventV2).events,
            "SendRandomEventV3" => (choiceAction as SendRandomEventV3).events,
            _ => throw new NotImplementedException()
        };
        return events.Select(e => e.Name);
    }

    private void TurnAttack(FsmState state, string eventName, bool onOff)
    {
        // Skip if the state doesn't have this event
        if (!_originalTransition[state.Name].ContainsKey(eventName))
        {
            return;
        }

        var toStateName = onOff ? _originalTransition[state.Name][eventName] : (state.Name + SHORT_CIRCUIT_PROTECTION_SUFFIX);
        state.ChangeTransition(eventName, toStateName);
        this.LogModDebug($"Changing transition: {state.Name}.{eventName} -> {toStateName}");
    }

    private Dictionary<string, Dictionary<string, string>> _originalTransition; // fromState -> event -> toState
    private List<FsmState> _scpStates;
    private PlayMakerFSM _fsm;
}
