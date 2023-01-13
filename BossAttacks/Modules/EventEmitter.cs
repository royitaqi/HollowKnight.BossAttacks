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

internal class EventEmitter : SingleStateModule
{
    public EventEmitter(Scene scene, EventEmitterConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name}");

        LoadSingleStateObjects(_scene, _config);

        _state.InsertMethod(() =>
        {
            _fsm.SendEvent(_config.EventName);
        }, 0);
        _state.Actions[0].Name = "EventEmitter";
    }

    protected override void OnUnload()
    {
        _state.RemoveActionByName("EventEmitter");
    }

    private Scene _scene;
    private EventEmitterConfig _config;
}
