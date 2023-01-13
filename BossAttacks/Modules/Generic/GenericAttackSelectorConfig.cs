using System;
using System.Collections.Generic;

namespace BossAttacks.Modules.Generic;

internal class GenericAttackSelectorConfig : SingleStateModuleConfig
{
    public override Type ModuleType => typeof(GenericAttackSelector);
    public HashSet<string> IgnoreEvents { get; set; }
    public Dictionary<string, string> MapEvents { get; set; }

    public bool IgnoreEvent(string eventName)
    {
        return IgnoreEvents != null && IgnoreEvents.Contains(eventName);
    }

    public string MapEvent(string eventName)
    {
        if (MapEvents == null)
        {
            return eventName;
        }
        return MapEvents.ContainsKey(eventName) ? MapEvents[eventName] : eventName;
    }
}
