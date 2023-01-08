using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using BossAttacks.Utils;
using SFCore.Utils;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BossAttacks.Modules;

/**
 * The basic unit of modding the game logic.
 * 
 * Can be loaded/unloaded (from mod menu; from entering/leaving boss scene).
 */
internal class WhiteDefender_InfiniteRollJumps : Module
{
    protected override bool OnLoad(Scene scene)
    {
        _booleanOptions = new()
        {
            { "Infinite ROLL JUMP", new Option<bool> { Value = false } },
            { "Infinite ROLL JUMP 2", new Option<bool> { Value = false } },
            { "Infinite ROLL JUMP 3", new Option<bool> { Value = false } },
        };
        return true;
    }
}
