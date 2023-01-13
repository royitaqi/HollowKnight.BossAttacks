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

internal class TransitionRewirer : SingleStateModule
{
    public TransitionRewirer(Scene scene, TransitionRewirerConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name}");

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
