using UnityEngine.SceneManagement;
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

    public override void Load(Scene scene)
    {
        this.LogMod($"Loading for scene {scene.name}");
        _scene = scene;
        _booleanOptions = new();

        // If there is no mapping, cannot find a config.
        if (!GodhomeUtils.SceneToBoss.ContainsKey(scene.name))
        {
            return;
        }
        var config = GodhomeUtils.SceneToBoss[scene.name];
        

        // Inspect GO and FSM, and set up.
        var go = scene.Find(config.GoName);
        this.LogModDebug($"GO = {go?.name}");
            
        var fsm = go?.LocateMyFSM(config.FsmName);
        this.LogModDebug($"FSM = {fsm?.name}");
            
        var states = fsm?.FsmStates.Where(s => s.Name.EndsWith("Choice") || s.Name.EndsWith("Decision")).ToList();
        if (states.Count != 1)
        {
            this.LogModWarn($"Cannot find Choice/Decision state. Candidates are: ({states.Count}) {String.Join(", ", states.Select(s => s.Name))}");
            Unload();
            return;
        }
        var state = states.First();
        this.LogModDebug($"State = {state.Name}");

        foreach (var t in state.Transitions)
        {
            var eventName = t.EventName;
            var originalToState = t.ToState;
            this.LogModDebug($"Transition: {eventName} -> {originalToState}");
            var opt = new Option<bool>
            {
                Value = true,
                OnSet = (attackIsOn) =>
                {
                    // If attack is on, connect the attack choice to the actual attack.
                    // Otherwise, connect the attack choice backto the Choice state itself.
                    var targetStateName = attackIsOn ? originalToState : state.Name;
                    state.ChangeTransition(eventName, targetStateName);
                    this.LogModDebug($"Changing transition: {eventName} -> {targetStateName}");
                }
            };
            _booleanOptions.Add(t.EventName, opt);
        }
    }

    public override Dictionary<string, Option<bool>> GetBooleanOptions()
    {
        return _booleanOptions;
    }

    public override void Unload()
    {
        this.LogMod("Unload()");

        // Restore everything
        foreach (var kvp in _booleanOptions)
        {
            // Turn all attacks back on when the module is unloaded.
            kvp.Value.Value = true;
        }

        _scene = null;
        _booleanOptions = null;
    }

    private Scene? _scene = null;
    private Dictionary<string, Option<bool>> _booleanOptions = null;
}
