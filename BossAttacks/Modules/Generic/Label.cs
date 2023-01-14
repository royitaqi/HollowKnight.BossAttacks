using BossAttacks.Utils;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules.Generic;

internal class Label : SingleFsmModule
{
    public Label(Scene scene, LabelConfig config, ModuleManager mgr)
    {
        _scene = scene;
        _config = config;
        _mgr = mgr;
    }

    protected override void OnLoad()
    {
        this.LogMod($"Loading");

        Option opt =  new MonoOption { Display = _config.Display, Interactive = false };
        _options.Add(opt);
    }

    protected override void OnUnload()
    {
        this.LogMod($"Unloading");
    }

    private Scene _scene;
    private LabelConfig _config;
    private ModuleManager _mgr;
}