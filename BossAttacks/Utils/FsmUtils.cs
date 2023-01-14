using System;
using System.Linq;
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
            ModAssert.AllBuilds(false, "Should never arrive here");
        }

        public static int FindActionIndexByType(this FsmState state, Type actionType)
        {
            return state.Actions.Select((a, i) => new { a, i }).First(ai => ai.a.GetType() == actionType).i;
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
