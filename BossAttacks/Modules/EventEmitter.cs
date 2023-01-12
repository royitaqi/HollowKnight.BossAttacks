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
    internal EventEmitter(EventEmitterConfig config)
    {
        _config = config;
    }

    protected override bool OnLoad(Scene scene)
    {
        this.LogMod($"Loading for scene {scene.name}");
        
        if (!base.LoadSingleStateObjects(scene, _config))
        {
            return false;
        }

        var action = new NextFrameEvent { sendEvent = _fsm.FsmEvents.First(e => e.Name == _config.EventName) };
        action.Name = "EventEmitter";
        _state.InsertAction(action, 0);

        return true;
    }

    protected override void OnUnload()
    {
        _state.RemoveActionByName("EventEmitter");
    }

    private EventEmitterConfig _config;
}
