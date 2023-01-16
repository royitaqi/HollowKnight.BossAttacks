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

    /**
     * Should clone the Option entirely, except the hooks on the existing events.
     */
    public abstract Option Clone();
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

    public override Option Clone()
    {
        return new MonoOption { Display = Display, Interactive = Interactive };
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

    public override Option Clone()
    {
        var ret = new BooleanOption { Interactive = Interactive, Value = Value };
        ret._display = _display;
        return ret;
    }
}
