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
                fsm.GetIntVariable("Summons").Value = 15;
            }, 0);
            fsm.GetState("Idle").Actions[0].Name = "Clear Counters";
            fsm.GetState("Idle").Actions[0].Enabled = false;
        }

        var option = new Option<bool> { Value = false };
        option.OnSet.Add(onOff =>
        {
            foreach (var fsm in _fsms)
            {
                fsm.GetState("Idle").Actions.First(a => a.Name == "Clear Counters").Enabled = onOff;
                fsm.GetState("Check Summon").ChangeTransition("CANCEL", "Idle");
                fsm.GetState("Check Summon GG").ChangeTransition("CANCEL", "Idle");
                fsm.GetState("Summon Antic").ChangeTransition("CANCEL", "Idle");
                fsm.GetState("Swoop Antic").ChangeTransition("CANCEL", "Idle");
            }
        });

        _booleanOptions.Add("Allow more summons (up to 15) and swoops (infinite)", option);
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
            for (int i = 0; i < fsm.GetState("Idle").Actions.Length; i++)
            {
                if (fsm.GetState("Idle").Actions[i].Name == "Clear Counters")
                {
                    fsm.GetState("Idle").RemoveAction(i);
                    break;
                }
            }
            fsm.GetState("Check Summon").ChangeTransition("CANCEL", "Swoop Antic");
            fsm.GetState("Check Summon GG").ChangeTransition("CANCEL", "Swoop Antic");
            fsm.GetState("Summon Antic").ChangeTransition("CANCEL", "Swoop Antic");
            fsm.GetState("Swoop Antic").ChangeTransition("CANCEL", "Check Summon");
        }
        _fsms = null;
    }

    private List<PlayMakerFSM> _fsms;
}
