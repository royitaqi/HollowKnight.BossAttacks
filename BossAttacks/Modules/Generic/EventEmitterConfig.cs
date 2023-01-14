using System;

namespace BossAttacks.Modules.Generic;

internal class EventEmitterConfig : SingleStateModuleConfig
{
    public override Type ModuleType => typeof(EventEmitter);
    public string EventName { get; set; }
    public Type ActionType { get; set; }
    public int IndexDelta { get; set; }
}
