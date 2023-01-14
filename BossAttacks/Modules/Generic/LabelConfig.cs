using System;

namespace BossAttacks.Modules.Generic;

internal class LabelConfig : ModuleConfig
{
    public override Type ModuleType => typeof(Label);

    /**
     * Text to display as option.
     */
    public string Display { get; set; }
}
