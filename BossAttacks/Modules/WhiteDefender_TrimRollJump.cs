using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using BossAttacks.Utils;
using SFCore.Utils;
using System.Linq;
using System;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;

namespace BossAttacks.Modules;

/**
 * Make the ROLL JUMP to be done without the initial throwing and the finishing diving.
 */
internal class WhiteDefender_TrimRollJump : Module
{
    protected override bool OnLoad(Scene scene)
    {
        if (scene.name != "GG_White_Defender")
        {
            return false;
        }

        var fsm = scene
            .Find("White Defender")
            .LocateMyFSM("Dung Defender");

        var option = new Option<bool> { Value = false };
        option.OnSet.Add(onOff =>
        {
            const string DROP_STATE_NAME = "BossAttacks RJ Drop To Ground";
            if (onOff)
            {
                // Turning on. Wire events to trim ROLL JUMP.
                var drop = fsm.AddState(DROP_STATE_NAME);
                drop.AddAction(new Wait() { time = 1f });
                fsm.ChangeTransition("RJ Set", "FINISHED", "Roll Speed");
                fsm.ChangeTransition("RJ In Air", "AIR DIVE", DROP_STATE_NAME);
                fsm.AddTransition(DROP_STATE_NAME, "FINISHED", "Move Choice");
            }
            else
            {
                // Turning off. Recover.
                fsm.ChangeTransition("RJ Set", "FINISHED", "RJ Speed Adjust");
                fsm.ChangeTransition("RJ In Air", "AIR DIVE", "Air Dive Antic");
                fsm.RemoveState(DROP_STATE_NAME);
            }
        });

        _booleanOptions.Add("Trim ROLL JUMP", option);
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
