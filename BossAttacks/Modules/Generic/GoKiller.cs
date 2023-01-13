using BossAttacks.Utils;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class GoKiller : SingleFsmModule
{
    public GoKiller(Scene scene, GoKillerConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name} (L = {L}..{H})");

        LoadSingleFsmObjects(_scene, _config);

        _go.GetComponent<HealthManager>().Die(null, AttackTypes.Generic, true);
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
    }

    private Scene _scene;
    private GoKillerConfig _config;
}
