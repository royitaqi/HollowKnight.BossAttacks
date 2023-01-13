using BossAttacks.Utils;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class PrintStatesModule : SingleFsmModule
{
    public PrintStatesModule(Scene scene, PrintStatesModuleConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name} (L = {L}..{H})");

        LoadSingleFsmObjects(_scene, _config);

        // Log boss states as they are being entered.
        // Putting it here at the end of the method, because the method body can be adding states, which should also generate such log.
        foreach (var state in _fsm.FsmStates)
        {
            state.InsertMethod(() =>
            {
                if (_config.Verbose)
                {
                    this.LogModFine($"{_config.GoName}-{_config.FsmName}: Boss entering state {state.Name}");
                }
                else
                {
                    this.LogModFine($"Boss entering state {state.Name}");
                }
            }, 0);
        }
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
    }

    private Scene _scene;
    private PrintStatesModuleConfig _config;
}