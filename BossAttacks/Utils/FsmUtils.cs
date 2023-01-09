using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BossAttacks.Modules;
using HutongGames.PlayMaker;

namespace BossAttacks.Utils
{
    internal class ConditionedStop : FsmStateAction
    {
        public override void OnEnter() => MinFinishTime = DateTime.Now + Wait;
        public override void OnUpdate() => CheckForCondition();

        public void CheckForCondition()
        {
            if (DateTime.Now > MinFinishTime)
            {
                base.Finish();
            }
        }

        public TimeSpan Wait { get; set; }

        private DateTime MinFinishTime;
    }
}
