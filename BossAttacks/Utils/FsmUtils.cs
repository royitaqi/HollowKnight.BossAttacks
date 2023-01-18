using System;
using System.Linq;
using HutongGames.PlayMaker;
using SFCore.Utils;

namespace BossAttacks.Utils
{
    internal static class FsmUtils
    {
        public static void InsertMethodWithName(this FsmState state, Action method, int index, string name)
        {
            state.InsertMethod(method, index);
            state.Actions[index].Name = name;
        }

        public static void RemoveActionByName(this FsmState state, string name)
        {
            for (int i = 0; i < state.Actions.Length; i++)
            {
                if (state.Actions[i].Name == name)
                {
                    // Replace the specified action with an no-op action instead of remove it out right.
                    // This is so that the indices of the trailing actions remain the same.
                    // Otherwise, if this removal is called during a middle of a loop through of the actions, some of the trailing actions won't be executed.
                    state.Actions[i] = new NoopAction();
                    state.Actions[i].Init(state);
                    return;
                }
            }
            ModAssert.AllBuilds(false, $"Cannot find action named \"{name}\" in state \"{state.Name}\" (GO = \"{state.Fsm.GameObject.name}\", FSM = \"{state.Fsm.Name}\")");
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

    internal class NoopAction : FsmStateAction
    {
        public override void OnEnter()
        {
            base.Finish();
        }
    }
}
