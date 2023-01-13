using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using BossAttacks.Utils;
using SFCore.Utils;
using System.Linq;
using System;
using System.Reflection;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace BossAttacks.Modules;

internal class TransitionRewirerConfig : SingleStateModuleConfig
{
    public override Type ModuleType => typeof(TransitionRewirer);
    public string EventName { get; set; }
    public string ToState { get; set; }
}
