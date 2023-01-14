using BossAttacks.Utils;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class PrintStates : SingleFsmModule
{
    public PrintStates(Scene scene, PrintStatesConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name}");

        LoadSingleFsmObjects(_scene, _config);

        // Log boss states as they are being entered.
        // Putting it here at the end of the method, because the method body can be adding states, which should also generate such log.
        foreach (var state in _fsm.FsmStates)
        {
            state.InsertMethod(() =>
            {
                this.LogModFine($"Boss entering state {state.Name}");
            }, 0);
        }
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
    }

    private Scene _scene;
    private PrintStatesConfig _config;
}