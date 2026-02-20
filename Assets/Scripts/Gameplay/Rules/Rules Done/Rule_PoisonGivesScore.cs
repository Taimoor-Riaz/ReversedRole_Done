using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Poison Gives Score")]
public class Rule_PoisonGivesScore : RuleCard
{

    public override void OnPickup(PickupEvent e, RuleContext ctx)
    {
        if (e.pickupType != PickupType.Poison) return;
        ctx.healthDeltas.Add(+e.baseValue);
    }
}
