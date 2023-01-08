using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BossAttacks.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal class ModuleManager {
    public static ModuleManager Instance = null;

    public void Load(Scene scene)
    {
        this.LogMod("Load");
        Unload();

        foreach (var module in FindModules())
        {
            if (module.Load(scene))
            {
                _loadedModules.Add(module);
            }
        }
        this.LogModDebug($"Loaded modules: ({_loadedModules.Count}) {String.Join(", ", _loadedModules.Select(m => m.Name))}");
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
        return _loadedModules;
    }

    private static IEnumerable<Module> FindModules() => Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(type => type.IsSubclassOf(typeof(Module)))
        .Where(type => !type.IsAbstract)
        .Select(type => Activator.CreateInstance(type) as Module)
        .OrderBy(module => module.Priority);

    private List<Module> _loadedModules = new();
}
