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
    public override bool Load(Scene scene)
    {
        return true;
    }

    public override Dictionary<string, Option<bool>> GetBooleanOptions()
    {
        return new()
        {
            { "Infinite ROLL JUMP", new Option<bool> { Value = false } },
            { "Infinite ROLL JUMP 2", new Option<bool> { Value = false } },
            { "Infinite ROLL JUMP 3", new Option<bool> { Value = false } },
        };
    }

    public override void Unload()
    {
    }
}
