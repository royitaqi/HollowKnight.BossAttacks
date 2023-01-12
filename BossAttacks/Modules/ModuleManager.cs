using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using BossAttacks.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal class ModuleManager {
    public static ModuleManager Instance = null;

    public static bool IsBossScene(Scene scene)
    {
        return GodhomeUtils.SceneToModuleConfigs.ContainsKey(scene.name);
    }

    public static bool IsSupportedBossScene(Scene scene)
    {
        return IsBossScene(scene) && GodhomeUtils.SceneToModuleConfigs[scene.name] != null;
    }

    public void Load(Scene scene)
    {
        this.LogMod("Load");
        Unload();

        Debug.Assert(IsSupportedBossScene(scene), "The current scene should be a supported boss scene.");

        var moduleConfigs = GodhomeUtils.SceneToModuleConfigs[scene.name];
        DefaultConfig defaultConfig = null;
        foreach (var config in moduleConfigs)
        {
            if (config is DefaultConfig)
            {
                defaultConfig = config as DefaultConfig;
            }
            else
            {
                PropagateConfig(defaultConfig, config);

                var type = config.ModuleType;
                var module = Activator.CreateInstance(type, config) as Module;
                module.Load(scene);
                _loadedModules.Add(module);
            }
        }

        this.LogModDebug($"Loaded modules: ({_loadedModules.Count}) {String.Join(", ", _loadedModules.Select(m => m.GetType().Name))}");

        // Add logic to subscribe to option change and implement inter-module logic
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

    public IEnumerable<Option> GetOptions()
    {
        return _loadedModules.SelectMany(m => m.Options);
    }

    private static void PropagateConfig(DefaultConfig from, ModuleConfig to)
    {
        foreach (var fromProp in from.GetType().GetProperties(BindingFlags.FlattenHierarchy))
        {
            var toProp = to.GetType().GetProperty(fromProp.Name, BindingFlags.FlattenHierarchy | BindingFlags.SetProperty);
            if (toProp == null)
            {
                continue;
            }
            // Skip propagate if `to` already have value
            if (toProp.GetValue(to) != null)
            {
                continue;
            }
            // Propagate
            toProp.SetValue(to, fromProp.GetValue(from));
        }
    }

    //public IEnumerable<Module> GetLoadedModules()
    //{
    //    return _loadedModules;
    //}

    //private static IEnumerable<Module> FindModules() => Assembly
    //    .GetExecutingAssembly()
    //    .GetTypes()
    //    .Where(type => type.IsSubclassOf(typeof(Module)))
    //    .Where(type => !type.IsAbstract)
    //    .Select(type => Activator.CreateInstance(type) as Module)
    //    .OrderBy(module => module.Priority);

    private List<Module> _loadedModules = new();
}
