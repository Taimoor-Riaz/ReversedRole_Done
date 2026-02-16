using System.Collections.Generic;

public class RuleContext
{
    public readonly List<int> healthDeltas = new();
    public readonly List<int> scoreDeltas = new();
    public readonly List<DamageRequest> damages = new();

    public void Clear()
    {
        healthDeltas.Clear();
        scoreDeltas.Clear();
        damages.Clear();
    }
}
