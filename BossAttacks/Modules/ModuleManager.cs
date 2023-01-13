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

        DefaultConfig defaultConfig = null;
        foreach (var config in GodhomeUtils.SceneToModuleConfigs[scene.name])
        {
            if (config is DefaultConfig)
            {
                defaultConfig = config as DefaultConfig;
            }
            else
            {
                PropagateConfig(defaultConfig, config);

                var type = config.ModuleType;
                var module = Activator.CreateInstance(type, scene, config, this) as Module;
                _modules.Add(module);
            }
        }

        this.LogModDebug($"Modules: ({_modules.Count}) {String.Join(", ", _modules.Select(m => m.GetType().Name))}");

        // Add logic to subscribe to option change and implement inter-module logic
    }

    public void Unload()
    {
        this.LogMod("Unload");
        foreach (var module in _modules)
        {
            module.Unload();
        }
        _modules.Clear();
        _level = 0;
    }

    public IEnumerable<Option> GetOptions()
    {
        return _modules.SelectMany(m => m.Options);
    }

    public void ChangeLevel(int level)
    {
        _level = level;
        foreach (var m in _modules)
        {
            bool shouldBeLoaded = m.L <= _level && _level <= m.H;
            if (shouldBeLoaded && !m.Loaded)
            {
                m.Load();
            }
            else if (!shouldBeLoaded && m.Loaded)
            {
                m.Unload();
            }
        }
    }

    internal static void PropagateConfig(DefaultConfig from, ModuleConfig to)
    {
        if (from == null)
        {
            return;
        }

        var fromProps = from.GetType().GetProperties();
        foreach (var fp in fromProps)
        {
            if (!fp.CanRead)
            {
                continue;
            }
            var fv = fp.GetValue(from);
            if (fv == null)
            {
                continue;
            }

            var tp = to.GetType().GetProperty(fp.Name);
            if (tp == null || !fp.CanRead || !fp.CanWrite || tp.GetValue(to) != null)
            {
                continue;
            }

            // Propagate
            tp.SetValue(to, fv);
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

    private List<Module> _modules = new();
    private int _level;
}
