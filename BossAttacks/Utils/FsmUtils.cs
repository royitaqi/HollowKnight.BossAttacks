using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BossAttacks.Modules;
using HutongGames.PlayMaker;
using SFCore.Utils;

namespace BossAttacks.Utils
{
    internal static class FsmUtils
    {
        public static void RemoveActionByName(this FsmState state, string actionName)
        {
            for (int i = 0; i < state.Actions.Length; i++)
            {
                if (state.Actions[i].Name == actionName)
                {
                    state.RemoveAction(i);
                    return;
                }
            }
            Debug.Assert(false, "Should never arrive here");
        }
    }

    internal class ShortCircuitProtectionAction : FsmStateAction
    {
        public override void OnUpdate()
        {
            base.Finish();
        }
    }
}
