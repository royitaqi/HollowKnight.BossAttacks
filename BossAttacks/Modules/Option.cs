using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine.SceneManagement;

namespace BossAttacks.Modules;


internal abstract class Option
{
	public abstract string Display { get; set; }
	public abstract bool Mutable { get; }
	public abstract void Mutate();

	public delegate void MutatedHandler();
	public event MutatedHandler Mutated;
}

internal class ReadMe : Option
{
	public override string Display { get; set; }
	public override bool Mutable { get => false; }

    public override void Mutate()
	{
		Debug.Assert(false, "Should not arrive here");
	}
}

internal class BooleanOption : Option
{
	public bool Value { get; set; }
	public override string Display {
		get => (Value ? "[ ✓ ] - " : "[     ] - ") + _display;
		set => _display = value;
	}
	public override bool Mutable { get => true; }

	public override void Mutate()
    {
		Value = !Value;
    }

	private string _display;
}
