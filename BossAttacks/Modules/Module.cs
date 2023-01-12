using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;

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
	 * Mod the fight.
	 */
	public bool Load(Scene scene)
    {
		Debug.Assert(!Loaded, $"{GetType().Name} shouldn't be loaded");
		bool ret = OnLoad(scene);
		Loaded = true;
		return ret;
	}

	/**
	 * Un-mod the fight. This shouldn't be required in most cases.
	 */
	public void Unload()
    {
		Debug.Assert(!Loaded, $"{GetType().Name} should be loaded");
		OnUnload();
		_options.Clear();
		Loaded = false;
    }

	protected abstract bool OnLoad(Scene scene);
	protected abstract void OnUnload();
	internal bool Loaded { get; private set; }

	public List<Option> Options { get => _options; }
	protected readonly List<Option> _options = new();
}