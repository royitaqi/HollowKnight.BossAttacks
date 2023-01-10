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
internal class Vengefly_InfiniteAttacks : Module
{
    protected override bool OnLoad(Scene scene)
    {
        // Can only accept GG_Vengefly and GG_Vengefly_V
        if (!scene.name.StartsWith("GG_Vengefly"))
        {
            return false;
        }

        _fsms = new List<PlayMakerFSM>();
        _fsms.Add(scene
            .Find("Giant Buzzer Col")
            .LocateMyFSM("Big Buzzer"));
        if (scene.name == "GG_Vengefly_V")
        {
            _fsms.Add(scene
                .Find("Giant Buzzer Col (1)")
                .LocateMyFSM("Big Buzzer"));
        }

        // Insert an action at the beginning of Idle state to clear counters. Disabled by default.
        foreach (var fsm in _fsms)
        {
            fsm.GetState("Idle").InsertMethod(() =>
            {
                fsm.GetIntVariable("Summons In A Row").Value = 0;
                fsm.GetIntVariable("Swoops in A Row").Value = 0;
            }, 0);
            fsm.GetState("Idle").Actions[0].Enabled = false;
        }

        var option = new Option<bool> { Value = false };
        option.OnSet.Add(onOff =>
        {
            foreach (var fsm in _fsms)
            {
                fsm.GetState("Idle").Actions[0].Enabled = onOff;

                // Turn built-in checks the other way
                fsm.GetState("Check Summon").Actions[0].Enabled = !onOff;
                fsm.GetState("Check Summon").Actions[4].Enabled = !onOff;
                fsm.GetState("Check Summon GG").Actions[2].Enabled = !onOff;
                fsm.GetState("Swoop Antic").Actions[0].Enabled = !onOff;
            }
        });

        _booleanOptions.Add("Check to allow more selected attacks to happen", option);
        return true;
    }

    protected override void OnUnload()
    {
        // Turn all options off when the module is unloaded.
        foreach (var kvp in _booleanOptions)
        {
            kvp.Value.Value = false;
        }

        // Restore the FSM
        foreach (var fsm in _fsms)
        {
            fsm.GetState("Idle").RemoveAction(0);

            // Restore built-in checks
            fsm.GetState("Check Summon").Actions[0].Enabled = true;
            fsm.GetState("Check Summon").Actions[4].Enabled = true;
            fsm.GetState("Check Summon GG").Actions[2].Enabled = true;
            fsm.GetState("Swoop Antic").Actions[0].Enabled = true;
        }
        _fsms = null;
    }

    private List<PlayMakerFSM> _fsms;
}
