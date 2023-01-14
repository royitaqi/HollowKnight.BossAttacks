using System.Linq;
using BossAttacks.Utils;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

/**
 * Creates a SCP state for a given state.
 */
internal class ShortCircuitProtection : SingleStateModule
{
    public ShortCircuitProtection(Scene scene, ShortCircuitProtectionConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name}");

        LoadSingleStateObjects(_scene, _config);

        // Short circuit protection (SCP).
        // * Short circuit is when the Choice state has all the events connected back to itself, causing an infinite loop where the boss takes no action.
        var scpStateName = _config.ScpStateName;
        var scpState = _fsm.AddState(scpStateName);
        scpState.AddAction(new ShortCircuitProtectionAction());
        scpState.AddTransition("FINISHED", _state.Name);
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");

        // Remove SCP state
        _fsm.RemoveState(_config.ScpStateName);

        UnloadSingleStateObjects();
    }

    private Scene _scene;
    private ShortCircuitProtectionConfig _config;
}
