using System;
using System.Collections;
using System.Collections.Generic;
using Modding;
using UnityEngine;
using Vasi;
using UObject = UnityEngine.Object;

namespace BossAttacks
{
    public class BossAttacks : Mod
    {
        internal static BossAttacks Instance;

        ///
        /// Mod
        ///

        // <breaking change>.<non-breaking major feature/fix>.<non-breaking minor feature/fix>.<patch>
        public override string GetVersion() => VersionUtil.GetVersion<BossAttacks>();

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing mod");

            Instance = this;

            Log("Initialized mod");
        }
    }
}