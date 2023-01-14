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
        this.LogMod($"Loading for scene {_scene.name} (L = {L}..{H})");

        LoadSingleStateObjects(_scene, _config);

        _state.InsertMethod(() =>
        {
            _fsm.SendEvent(_config.EventName);
        }, _config.Index);
        _state.Actions[_config.Index].Name = "EventEmitter";
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
        _state.RemoveActionByName("EventEmitter");
    }

    private Scene _scene;
    private EventEmitterConfig _config;
}
