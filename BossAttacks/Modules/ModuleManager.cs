using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BossAttacks.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal class ModuleManager {
    public static ModuleManager Instance = null;

    public bool Load(Scene scene)
    {
        this.LogMod("Load");
        Unload();

        if (!GodhomeUtils.SceneToModuleConfigs.ContainsKey(scene.name))
        {
            return false;
        }

        var moduleConfigs = GodhomeUtils.SceneToModuleConfigs[scene.name];
        foreach (var config in moduleConfigs)
        {
            var type = config.ModuleType;
            var module = Activator.CreateInstance(type, new[] { config }) as Module;
            _loadedModules.Add(module);
        }

        this.LogModDebug($"Loaded modules: ({_loadedModules.Count}) {String.Join(", ", _loadedModules.Select(m => m.GetType().Name))}");
        return true;
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

    //private static IEnumerable<Module> FindModules() => Assembly
    //    .GetExecutingAssembly()
    //    .GetTypes()
    //    .Where(type => type.IsSubclassOf(typeof(Module)))
    //    .Where(type => !type.IsAbstract)
    //    .Select(type => Activator.CreateInstance(type) as Module)
    //    .OrderBy(module => module.Priority);

    private List<Module> _loadedModules = new();
}
