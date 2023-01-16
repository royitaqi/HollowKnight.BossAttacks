using System;
using System.Collections.Generic;
using System.Linq;
using BossAttacks.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class OptionCombiner : SingleStateModule
{
    public OptionCombiner(Scene scene, OptionCombinerConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading for scene {_scene.name}");

    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
    }

    public override void GetOptions(List<Option> options)
    {
        var keys = new List<string>();
        var map = new Dictionary<string, List<Option>>();
        
        foreach (var opt in options)
        {
            if (!map.ContainsKey(opt.Display))
            {
                map[opt.Display] = new();
                keys.Add(opt.Display);
            }
            map[opt.Display].Add(opt);
        }

        options.Clear();
        foreach (var key in keys) // Make sure to walk through option keys in the order that they came in
        {
            var opts = map[key];
            if (opts.Count == 1)
            {
                // There is only one option under this key. Just use it.
                options.Add(opts[0]);
            }
            else
            {
                // There are multiple options under the same key. Make sure they are the same things.
                ModAssert.AllBuilds(opts.All(o => o.GetType() == opts[0].GetType()), $"All options \"{key}\"'s type should agree ({String.Join(", ", opts.Select(o => o.GetType().Name))})");
                ModAssert.AllBuilds(opts.All(o => o.Interactive == opts[0].Interactive), $"All options \"{key}\"'s interactiveness should agree ({String.Join(", ", opts.Select(o => o.Interactive.ToString()))})");

                // Clone the first option to be the proxy. If it's interacted, interact the underlying options.
                var clone = opts[0].Clone();
                clone.Interacted += () =>
                {
                    foreach (var opt in opts)
                    {
                        opt.Interact();
                    }
                };
                options.Add(clone);
            }
        }
    }

    private Scene _scene;
    private OptionCombinerConfig _config;
}
