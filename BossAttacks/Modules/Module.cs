using System.Collections.Generic;
using BossAttacks.Utils;

namespace BossAttacks.Modules;

/**
 * The basic unit of modding the game logic.
 * 
 * Can be loaded/unloaded.
 */
internal abstract class Module {
    //public virtual string Name => GetType().Name;
    public virtual int Priority => 0;

    /**
     * The level range where this module should be loaded.
     * Low (L) and high (H) are both inclusive.
     */
    public int L { get; set; }
    public int H { get; set; }

    /**
     * Mod the fight.
     */
    public void Load()
    {
        ModAssert.AllBuilds(!Loaded, $"{GetType().Name} shouldn't be loaded");
        OnLoad();
        Loaded = true;
    }

    /**
     * Un-mod the fight. This shouldn't be required in most cases.
     */
    public void Unload()
    {
        ModAssert.AllBuilds(Loaded, $"{GetType().Name} should be loaded");
        OnUnload();
        _options.Clear();
        Loaded = false;
    }

    protected abstract void OnLoad();
    protected abstract void OnUnload();
    internal bool Loaded { get; private set; }

    public List<Option> Options { get => _options; }
    protected readonly List<Option> _options = new();
}