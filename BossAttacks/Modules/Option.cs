using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal class Option<T> {
	public List<Func<T, bool>> CanSet = new();
	public List<Action<T>> OnSet = new();

	public T Value {
		get
        {
			return _value;
        }
		set
        {
			if (CanSet.All(oc => oc(value)))
            {
				_value = value;
			}
			OnSet.ForEach(os => os(value));
		}
	}
	private T _value;
}
