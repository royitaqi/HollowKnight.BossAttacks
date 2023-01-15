using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        this.LogModDebug($"Modules: ({_modules.Count})");
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

        ModAssert.AllBuilds(_modules.Count == _modules.Select(m => m.ID).Distinct().Count(), "All modules should have unique IDs");

        ChangeLevel(0);
    }

    public void Unload()
    {
        this.LogMod("Unload");
        foreach (var module in _modules.Reverse<Module>())
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
        // Unload in reverse order
        foreach (var m in _modules.Reverse<Module>())
        {
            bool shouldBeLoaded = m.Levels.Contains(_level);
            if (!shouldBeLoaded && m.Loaded)
            {
                m.Unload();
            }
        }
        // Load in order
        foreach (var m in _modules)
        {
            bool shouldBeLoaded = m.Levels.Contains(_level);
            if (shouldBeLoaded && !m.Loaded)
            {
                m.Load();
            }
        }
        OptionsChanged?.Invoke();
        this.LogModDebug($"Level is now {_level}");
        return originalLevel;
    }

    private Module CreateModule(Scene scene, ModuleConfig config)
    {
        var module = Activator.CreateInstance(config.ModuleType, scene, config, this) as Module;

        if (config.Levels != null)
        {
            module.Levels = config.Levels;
        }
        else
        {
            module.Levels = new HashSet<int>(Enumerable.Range(config.L, config.H - config.L + 1));
        }

        var mainID = (config.ID != null ? config.ID : config.ModuleType.Name);
        var levels = String.Join("", module.Levels.Select(l => l.ToString()));
        module.ID = $"{mainID} | {levels}";

        this.LogModDebug($"    {module.GetType().Name}: {module.ID} -- {ConfigToString(config)}");
        return module;
    }

    private string ConfigToString(ModuleConfig config)
    {
        var sb = new StringBuilder();
        foreach (var p in config.GetType().GetProperties().Where(p => p.Name != "ModuleType"))
        {
            var v = p.GetValue(config);
            if (v == null)
            {
                continue;
            }
            if (sb.Length != 0)
            {
                sb.Append(", ");
            }
            sb.Append($"{p.Name} = {p.GetValue(config)}");
        }
        return sb.ToString();
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
            // Can only propagate properies which are on the list
            if (!CanPropagate.Contains(prop.Name))
            {
                continue;
            }

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

    private static readonly HashSet<string> CanPropagate = new() { "GoName", "FsmName", "StateName", "EventName" };

    internal static IEnumerable<PrintStatesConfig> GetPrintStatesModuleConfigs(ModuleConfig[] configs)
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
            .Select((s, i) =>
            {
                var parts = s.Split('-');
                return new PrintStatesConfig
                {
                    L = l,
                    H = h,
                    GoName = parts[0],
                    FsmName = parts[1],
                    ID = names.Length > 1 ? $"{typeof(PrintStates).Name} #{i}" : null,
                };
            });
    }

    private List<Module> _modules = new();
    private int _level;
}
