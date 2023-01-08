using System;
using System.Collections.Generic;
using System.Linq;
using BossAttacks.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal class ModuleManager {
    public static ModuleManager Instance = null;

    public void Load(Scene scene)
    {
        this.LogMod("Load");
        Unload();

        foreach (var module in ModuleList)
        {
            if (module.Load(scene))
            {
                _loadedModules.Add(module);
            }
        }
    }

    public void Unload()
    {
        this.LogMod("Unload");
        foreach (var module in _loadedModules)
        {
            module.Unload();
        }
        _loadedModules.Clear();
    }

    public IEnumerable<Module> GetLoadedModules()
    {
        this.LogModDebug($"Loaded modules: ({_loadedModules.Count}) {String.Join(", ", _loadedModules.Select(m => m.Name))}");
        return _loadedModules;
    }

    private readonly Module[] ModuleList = new Module[]
    {
        new GenericAttackSelector(),
        new WhiteDefender_InfiniteRollJumps(),
    };

    private List<Module> _loadedModules = new List<Module>();
}
