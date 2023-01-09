using System.Collections.Generic;
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
		Unload();
		return OnLoad(scene);
	}

	public void Unload()
    {
		OnUnload();
		_booleanOptions.Clear();
    }

	protected virtual bool OnLoad(Scene scene) => true;
	protected virtual void OnUnload() { }

	public Dictionary<string, Option<bool>> BooleanOptions { get => _booleanOptions; }
	protected readonly Dictionary<string, Option<bool>> _booleanOptions = new();
}