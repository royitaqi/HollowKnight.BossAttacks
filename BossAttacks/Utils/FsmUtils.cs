using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BossAttacks.Modules;
using HutongGames.PlayMaker;

namespace BossAttacks.Utils
{
    internal class ShortCircuitProtectionAction : FsmStateAction
    {
        public override void OnEnter()
        {
            if (base.State.loopCount >= TriggeringLoopCount)
            {
                this.LogModFine($"Short circuit protection has been triggered. Loop count ({State.loopCount}) > trigger ({TriggeringLoopCount}). Will stall for {Stall}.");
                MinFinishTime = DateTime.Now + Stall;
            }
        }
        public override void OnUpdate()
        {
            if (DateTime.Now > MinFinishTime)
            {
                this.LogModFine($"Finished stall.");
                base.Finish();
            }
        }

        public int TriggeringLoopCount { get; set; }
        public TimeSpan Stall { get; set; }

        private DateTime MinFinishTime = DateTime.Now;
    }
}
