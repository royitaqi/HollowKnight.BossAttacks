namespace BossAttacks.Modules;


internal abstract class Option
{
	public abstract string Display { get; set; }
	public abstract bool Interactive { get; }
	public abstract void Interact();

	public delegate void InteractionHandler();
	public event InteractionHandler Interacted;

	protected void RaiseInteracted()
    {
		Interacted?.Invoke();
    }
}

internal class MonoOption : Option
{
	public override string Display { get; set; }
	public override bool Interactive { get => false; }

    public override void Interact()
	{
		RaiseInteracted();
	}
}

internal class BooleanOption : Option
{
	public bool Value { get; private set; }
	public override string Display {
		get => (Value ? "[ ✓ ] - " : "[     ] - ") + _display;
		set => _display = value;
	}
	public override bool Interactive { get => true; }

	public override void Interact()
    {
		Value = !Value;
		RaiseInteracted();
	}

	private string _display;
}
