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

	public abstract bool Load(Scene scene);
	public abstract void Unload();

	public abstract Dictionary<string, Option<bool>> GetBooleanOptions();
}