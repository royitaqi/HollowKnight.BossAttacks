using BossAttacks.Utils;

namespace BossAttacks.Modules;


internal abstract class Option
{
    public abstract string Display { get; set; }
    public bool Interactive { get; set; } = true;

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

    public override void Interact()
    {
        if (Interactive)
        {
            RaiseInteracted();
        }
        else
        {
            ModAssert.AllBuilds(false, "An non-interactive MonoOption should not be interacted");
        }
    }
}

internal class BooleanOption : Option
{
    public bool Value { get; private set; }

    public override string Display {
        get => (Value ? "[ ✓ ] - " : "[     ] - ") + _display;
        set => _display = value;
    }
    private string _display;

    public override void Interact()
    {
        if (Interactive)
        {
            Value = !Value;
            RaiseInteracted();
        }
        else
        {
            ModAssert.AllBuilds(false, "An non-interactive BooleanOption should not be interacted");
        }
    }
}
