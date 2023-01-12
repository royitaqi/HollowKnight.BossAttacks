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

internal class VariableSetterConfig : SingleStateModuleConfig
{
    public override Type ModuleType { get => typeof(VariableSetter); }
    internal KeyValuePair<string, int>[] IntVariables { get; set; }
    internal KeyValuePair<string, bool>[] BoolVariables { get; set; }
}
