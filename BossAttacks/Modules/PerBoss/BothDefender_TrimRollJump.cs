using BossAttacks.Modules.Generic;
using BossAttacks.Utils;
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

/**
 * Make the ROLL JUMP to be done without the initial throwing and the finishing diving.
 */
internal class BothDefender_TrimRollJump : SingleFsmModule
{
    public BothDefender_TrimRollJump(Scene scene, BothDefender_TrimRollJump_Config config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name}");

        LoadSingleFsmObjects(_scene, _config);

        var opt = new BooleanOption { Display = "Trim ROLL JUMP" };
        opt.Interacted += () =>
        {
            const string DROP_STATE_NAME = "BothDefender RJ Drop To Ground";
            if (opt.Value)
            {
                // Turning on. Wire events to trim ROLL JUMP.
                var drop = _fsm.AddState(DROP_STATE_NAME);
                drop.AddAction(new Wait() { time = 1f });
                _fsm.ChangeTransition("RJ Set", "FINISHED", "Roll Speed");
                _fsm.ChangeTransition("RJ In Air", "AIR DIVE", DROP_STATE_NAME);
                _fsm.AddTransition(DROP_STATE_NAME, "FINISHED", "Move Choice");
            }
            else
            {
                // Turning off. Recover.
                _fsm.ChangeTransition("RJ Set", "FINISHED", "RJ Speed Adjust");
                _fsm.ChangeTransition("RJ In Air", "AIR DIVE", "Air Dive Antic");
                _fsm.RemoveState(DROP_STATE_NAME);
            }
        };

        _options.Add(opt);
    }

    protected override void OnUnload()
    {
        // Turn all attacks back on when the module is unloaded.
        foreach (var o in _options)
        {
            var opt = o as BooleanOption;
            if (opt.Value)
            {
                opt.Interact();
            }
        }
    }

    private Scene _scene;
    private BothDefender_TrimRollJump_Config _config;
}
