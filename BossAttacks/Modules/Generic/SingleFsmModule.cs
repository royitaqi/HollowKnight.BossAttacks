using BossAttacks.Utils;
using SFCore.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal abstract class SingleFsmModule : Module
{
    protected void LoadSingleFsmObjects(Scene scene, SingleFsmModuleConfig config)
    {
        _go = FindGameObject(scene, config.GoName);
        if (_go == null)
        {
            this.LogModError($"Cannot find GO {config.GoName} in scene");
        }
        this.LogModDebug($"GO = {_go.name}  ({_go.GetInstanceID()})");

        _fsm = _go.LocateMyFSM(config.FsmName);
        if (_fsm == null)
        {
            this.LogModError($"Cannot find FSM {config.FsmName} in GO");
        }
        this.LogModDebug($"FSM = {_fsm.FsmName} ({_fsm.GetInstanceID()})");
    }

    protected void UnloadSingleFsmObjects()
    {
        _go = null;
        _fsm = null;
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
}