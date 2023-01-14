using BossAttacks.Utils;
using HutongGames.PlayMaker;
using SFCore.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class Nosk : SingleFsmModule
{
    private const string IDLE_SCP_STATE_NAME = "Idle" + GenericAttackSelector.SHORT_CIRCUIT_PROTECTION_SUFFIX;

    public Nosk(Scene scene, NoskConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name}");

        LoadSingleFsmObjects(_scene, _config);
        _idle = _fsm.GetState("Idle");
        _idleNoSpit = _fsm.GetState("Idle No Spit");

        // SCP on Idle
        var scpState = _fsm.AddState(IDLE_SCP_STATE_NAME);
        scpState.AddAction(new ShortCircuitProtectionAction());
        scpState.AddTransition("FINISHED", "Idle");

        // All options
        var chargeOpt = new BooleanOption { Display = "CHARGE" };
        var spitOpt = new BooleanOption { Display = "SPIT (requires CHARGE)" };
        var jumpOpt = new BooleanOption { Display = "JUMP" };
        var rsOpt = new BooleanOption { Display = "ROOF SPIT (exclusive)" };
        _options.AddRange(new[] { chargeOpt, spitOpt, jumpOpt, rsOpt });

        // CHARGE
        chargeOpt.Interact(); // Initially turned on
        chargeOpt.Interacted += () =>
        {
            if (!chargeOpt.Value)
            {
                // Turned off 
                _idle.ChangeTransition("CHARGE", IDLE_SCP_STATE_NAME);
                _idleNoSpit.ChangeTransition("CHARGE", IDLE_SCP_STATE_NAME);

                // Turn off SPIT
                EnsureState(spitOpt, false);
            }
            else
            {
                // Turned on
                _idle.ChangeTransition("CHARGE", "Charge Init");
                _idleNoSpit.ChangeTransition("CHARGE", "Charge Init");

                // Turn off ROOF SPIT
                EnsureState(rsOpt, false);
            }
        };

        // SPIT
        spitOpt.Interact(); // Initially turned on
        spitOpt.Interacted += () =>
        {
            if (!spitOpt.Value)
            {
                // Turned off
                _idle.ChangeTransition("SPIT", IDLE_SCP_STATE_NAME);
            }
            else
            {
                // Turned on
                _idle.ChangeTransition("SPIT", "Set Spit");

                // Turn on CHARGE
                EnsureState(chargeOpt, true);

                // Turn off ROOF SPIT
                EnsureState(rsOpt, false);
            }
        };

        // JUMP
        jumpOpt.Interact(); // Initially turned on
        jumpOpt.Interacted += () =>
        {
            if (!jumpOpt.Value)
            {
                // Turned off
                _idle.ChangeTransition("JUMP", IDLE_SCP_STATE_NAME);
                _idleNoSpit.ChangeTransition("JUMP", IDLE_SCP_STATE_NAME);
            }
            else
            {
                // Turned on
                _idle.ChangeTransition("JUMP", "Aim Jump");
                _idleNoSpit.ChangeTransition("JUMP", "Aim Jump");

                // Turn off ROOF SPIT
                EnsureState(rsOpt, false);
            }
        };

        // ROOF SPIT
        rsOpt.Interacted += () =>
        {
            if (rsOpt.Value)
            {
                // Turned on
                _idle.ChangeTransition("ROAR", "RS Jump Antic");
                _idle.InsertMethodWithName(() =>
                {
                    _fsm.SendEvent("ROAR");
                }, 0, "ROAR Emittor");
                
                // Turn off all other attacks
                foreach (var opt in new[] { chargeOpt, spitOpt, jumpOpt })
                {
                    EnsureState(opt, false);
                }
            }
            else
            {
                // Turned off
                _idle.ChangeTransition("ROAR", "Roar Init");
                _idle.RemoveActionByName("ROAR Emittor");
            }
        };
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");

        _fsm.RemoveState(IDLE_SCP_STATE_NAME);
    }

    private static void EnsureState(BooleanOption opt, bool desiredState)
    {
        if (opt.Value != desiredState)
        {
            opt.Interact();
        }
    }

    private Scene _scene;
    private NoskConfig _config;
    private FsmState _idle;
    private FsmState _idleNoSpit;
}
