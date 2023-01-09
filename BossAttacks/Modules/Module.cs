using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

/**
 * The basic unit of modding the game logic.
 * 
 * Can be loaded/unloaded (from mod menu; from entering/leaving boss scene).
 */
internal abstract class Module {
	public virtual string Name => GetType().Name;
	public virtual int Priority => 0;

	public bool Load(Scene scene)
    {
		Debug.Assert(!Loaded, $"{GetType().Name} shouldn't be loaded");
		bool ret = OnLoad(scene);
		Loaded = true;
		return ret;
	}

	public void Unload()
    {
		Debug.Assert(!Loaded, $"{GetType().Name} should be loaded");
		OnUnload();
		_booleanOptions.Clear();
		Loaded = false;
    }

	protected virtual bool OnLoad(Scene scene) => true;
	protected virtual void OnUnload() { }
	internal bool Loaded { get; private set; }

	public Dictionary<string, Option<bool>> BooleanOptions { get => _booleanOptions; }
	protected readonly Dictionary<string, Option<bool>> _booleanOptions = new();
}