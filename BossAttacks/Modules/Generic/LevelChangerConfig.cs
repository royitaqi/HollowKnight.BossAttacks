using System;

namespace BossAttacks.Modules.Generic;

internal class LevelChangerConfig : ModuleConfig
{
    public override Type ModuleType => typeof(LevelChanger);

    /**
     * Text to display as option.
     */
    public string Display { get; set; }

    /**
     * What level to change into when interacted.
     */
    public int TargetL { get; set; }

    public enum Modes
    {
        OneTime, // Only one interaction allowed. Jumps to target L.
        OneDirection, // Multiple interactions allowed. Always jumps to target L.
        Bidirection, // Multiple interactions allowed. Jumps between target L and original L (noted every time it goes to the target L).
    }
    public Modes Mode { get; set; }
}
