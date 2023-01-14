using System;
using System.Collections.Generic;
using System.Linq;
using BossAttacks.Modules.Generic;
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

        ModAssert.AllBuilds(IsSupportedBossScene(scene), "The current scene should be a supported boss scene.");

        var dict = new Dictionary<string, object>();
        foreach (var config in GodhomeUtils.SceneToModuleConfigs[scene.name])
        {
            // Use L as default if H wasn't specified
            ModAssert.AllBuilds(config.L >= 0, "Config.L should be zero or positive");
            if (config.H == 0)
            {
                config.H = config.L;
            }
            else
            {
                ModAssert.AllBuilds(config.L <= config.H, "Config.L should be <= config.H");
            }

            // Propagate values between dict and config
            PropagateConfig(dict, config);

            // Create module
            var module = CreateModule(scene, config);
            _modules.Add(module);
        }

        // Create PrintStatesModules
        var printConfigs = GetPrintStatesModuleConfigs(GodhomeUtils.SceneToModuleConfigs[scene.name]).ToArray();
        foreach (var config in printConfigs)
        {
            var module = CreateModule(scene, config);
            _modules.Add(module);
        }

        this.LogModDebug($"Modules: ({_modules.Count}) {String.Join(", ", _modules.Select(m => m.GetType().Name))}");

        ChangeLevel(0);
    }

    public void Unload()
    {
        this.LogMod("Unload");
        foreach (var module in _modules)
        {
            if (module.Loaded)
            {
                module.Unload();
            }
        }
        _modules.Clear();
        _level = 0;
    }

    public IEnumerable<Option> GetOptions()
    {
        return _modules.SelectMany(m => m.Options);
    }

    public delegate void OptionsChangedHandler();
    public event OptionsChangedHandler OptionsChanged;

    public int ChangeLevel(int level)
    {
        this.LogModDebug($"Changing level from {_level} to {level}");
        int originalLevel = _level;
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
        OptionsChanged?.Invoke();
        this.LogModDebug($"Level is now {_level}");
        return originalLevel;
    }

    private Module CreateModule(Scene scene, ModuleConfig config)
    {
        var module = Activator.CreateInstance(config.ModuleType, scene, config, this) as Module;

        // Copy the config's ID, L, H over to the module
        module.ID = (config.ID != null ? config.ID : config.ModuleType.Name) + $" {config.L}-{config.H}";
        module.L = config.L;
        module.H = config.H;
        return module;
    }

    /**
     * How propagation works:
     * 
     * Config -> dict:
     * - All non-null values in config will be propagated to dict.
     * 
     * Dict -> config:
     * - Only null values in config will receive values from dict.
     */
    internal static void PropagateConfig(Dictionary<string, object> dict, ModuleConfig config)
    {
        foreach (var prop in config.GetType().GetProperties())
        {
            // Cannot propagate to something whose state cannot be verified
            if (!prop.CanRead)
            {
                continue;
            }
            var v = prop.GetValue(config);
            
            // Propagate: config -> dict
            if (v != null)
            {
                dict[prop.Name] = v;
            }

            // Propagate: dict -> config
            else if (prop.CanWrite && dict.ContainsKey(prop.Name) && v == null)
            {
                prop.SetValue(config, dict[prop.Name]);
            }
        }
    }

    internal static IEnumerable<PrintStatesModuleConfig> GetPrintStatesModuleConfigs(ModuleConfig[] configs)
    {
        int l = configs.Select(c => c.L).Min();
        int h = configs.Select(c => c.H).Max();
        var names = configs
            .Select(c => c as SingleFsmModuleConfig)
            .Where(c => c != null)
            .Select(c => c.GoName + "-" + c.FsmName)
            .Distinct()
            .ToArray();

        return names
            .Select(s =>
            {
                var parts = s.Split('-');
                return new PrintStatesModuleConfig
                {
                    L = l,
                    H = h,
                    GoName = parts[0],
                    FsmName = parts[1],
                    ID = names.Length > 1 ? s : null,
                };
            });
    }

    private List<Module> _modules = new();
    private int _level;
}
