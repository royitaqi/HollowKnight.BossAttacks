using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;

internal class Option<T> {


	public Action<T> OnSet;
	
	public T Value {
		get
        {
			return _value;
        }
		set
        {
			_value = value;
			
			if (OnSet != null)
            {
				OnSet(Value);
			}
		}
	}

	private T _value;
}