using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using BossAttacks.Utils;
using SFCore.Utils;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BossAttacks.Modules;

internal class WhiteDefender_InfiniteRollJump : Module
{
    protected override bool OnLoad(Scene scene)
    {
        if (scene.name != "GG_White_Defender")
        {
            return false;
        }

        _booleanOptions.Add("Infinite ROLL JUMP", new Option<bool> { Value = false });
        _booleanOptions.Add("Infinite ROLL JUMP 2", new Option<bool> { Value = false });
        return true;
    }

    protected override void OnUnload()
    {
        // Turn all options off when the module is unloaded.
        foreach (var kvp in _booleanOptions)
        {
            kvp.Value.Value = false;
        }
    }
}
