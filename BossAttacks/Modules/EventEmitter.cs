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
    public EventEmitter(EventEmitterConfig config)
    {
        _config = config;
    }

    protected override void OnLoad(Scene scene)
    {
        this.LogMod($"Loading for scene {scene.name}");

        LoadSingleStateObjects(scene, _config);

        var action = new NextFrameEvent { sendEvent = _fsm.FsmEvents.First(e => e.Name == _config.EventName) };
        action.Name = "EventEmitter";
        _state.InsertAction(action, 0);
    }

    protected override void OnUnload()
    {
        _state.RemoveActionByName("EventEmitter");
    }

    private EventEmitterConfig _config;
}
