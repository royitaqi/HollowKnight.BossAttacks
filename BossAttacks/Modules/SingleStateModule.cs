using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BossAttacks.Utils;
using HutongGames.PlayMaker;
using SFCore.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal abstract class SingleStateModule : Module
{
    protected bool LoadSingleStateObjects(Scene scene, SingleStateModuleConfig config)
    {
        // Inspect GO and FSM, and set up.
        _go = FindGameObject(scene, config.GoName);
        if (_go == null)
        {
            this.LogModWarn($"Cannot find GO {config.GoName} in scene");
            return false;
        }
        this.LogModDebug($"GO = {_go.name}  ({_go.GetInstanceID()})");

        _fsm = _go.LocateMyFSM(config.FsmName);
        if (_fsm == null)
        {
            this.LogModWarn($"Cannot find FSM {config.FsmName} in GO");
            return false;
        }
        this.LogModDebug($"FSM = {_fsm.FsmName} ({_fsm.GetInstanceID()})");

        if (config.StateName == null)
        {
            var states = _fsm.FsmStates.Where(s => s.Name.EndsWith("Choice") || s.Name.EndsWith("Decision")).ToArray();
            if (states.Length != 1)
            {
                this.LogModWarn($"Cannot find Choice/Decision state. Candidates are: ({states.Length}) {String.Join(", ", states.Select(s => s.Name))}.");
                return false;
            }
            _state = states[0];
        }
        else
        {
            _state = _fsm.GetState(config.StateName);
        }
        this.LogModDebug($"State: {_state.Name}");

        return true;
    }

    protected void UnloadSingleStateObjects()
    {
        _go = null;
        _fsm = null;
        _state = null;
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

    protected GameObject _go;
    protected PlayMakerFSM _fsm;
    protected FsmState _state;
}