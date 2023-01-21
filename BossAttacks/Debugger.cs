using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BossAttacks.Modules;
using BossAttacks.Utils;
using Modding;
using Newtonsoft.Json;
using UnityEngine;

namespace BossAttacks
{
    internal class Debugger
    {
        public static Debugger Instance;

        public void Load()
        {
            ModHooks.HeroUpdateHook += OnHeroUpdate;
        }

        public void Unload()
        {
            ModHooks.HeroUpdateHook -= OnHeroUpdate;
        }
        
        private void OnHeroUpdate()
        {
        }
    }
}
