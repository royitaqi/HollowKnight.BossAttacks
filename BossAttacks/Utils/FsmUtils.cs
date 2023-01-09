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
        public override void OnUpdate()
        {
            base.Finish();
        }
    }
}
