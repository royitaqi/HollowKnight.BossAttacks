using BossAttacks.Utils;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class TransitionRewirer : SingleStateModule
{
    public TransitionRewirer(Scene scene, TransitionRewirerConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name} (L = {L}..{H})");

        LoadSingleStateObjects(_scene, _config);

        _originalToState = _state.GetTransition(_config.EventName).ToState;
        _state.ChangeTransition(_config.EventName, _config.ToState);
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
        _state.ChangeTransition(_config.EventName, _originalToState);
        _originalToState = null;
    }

    private Scene _scene;
    private TransitionRewirerConfig _config;
    private string _originalToState;
}
