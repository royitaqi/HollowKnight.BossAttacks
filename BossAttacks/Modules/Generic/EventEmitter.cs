using BossAttacks.Utils;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

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

        int index = (_config.ActionType != null ? _state.FindActionIndexByType(_config.ActionType) : 0) + _config.IndexDelta;
        _state.InsertMethod(() =>
        {
            _fsm.SendEvent(_config.EventName);
        }, index);
        _state.Actions[index].Name = "EventEmitter";
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
        _state.RemoveActionByName("EventEmitter");
    }

    private Scene _scene;
    private EventEmitterConfig _config;
}
