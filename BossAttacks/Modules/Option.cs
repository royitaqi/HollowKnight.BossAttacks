using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal class Option<T> {


	public Func<T, bool> CanSet;
	
	public T Value {
		get
        {
			return _value;
        }
		set
        {
			if (CanSet == null || CanSet(value))
            {
				_value = value;
			}
		}
	}

	private T _value;
}