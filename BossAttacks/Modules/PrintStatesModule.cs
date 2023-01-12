using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BossAttacks.Utils;
using HutongGames.PlayMaker;
using SFCore.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal class PrintStatesModule : SingleFsmModule
{
    public PrintStatesModule(PrintStatesModuleConfig config)
    {
        _config = config;
    }

    protected override void OnLoad(Scene scene)
    {
        LoadSingleFsmObjects(scene, _config);

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
    }

    private PrintStatesModuleConfig _config;
}