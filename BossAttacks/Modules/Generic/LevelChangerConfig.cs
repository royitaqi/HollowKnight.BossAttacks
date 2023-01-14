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

    /**
     * Whether or not the level can be changed back to the original by a second interaction.
     */
    public bool Reversible { get; set; }
}
