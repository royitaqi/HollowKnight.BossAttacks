using BossAttacks.Utils;
using HutongGames.PlayMaker;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class ActionEnabler : SingleStateModule
{
    public ActionEnabler(Scene scene, ActionEnablerConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name}");

        LoadSingleStateObjects(_scene, _config);

        int index = (_config.ActionType != null ? _state.FindActionIndexByType(_config.ActionType) : 0) + _config.IndexDelta;
        _action = _state.Actions[index];
        _originalEnabled = _action.Enabled;
        _action.Enabled = _config.ToEnabled;
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");

        _action.Enabled = _originalEnabled;
    }

    private Scene _scene;
    private ActionEnablerConfig _config;
    private FsmStateAction _action;
    private bool _originalEnabled;
}
