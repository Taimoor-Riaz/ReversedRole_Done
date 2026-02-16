using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Poison Hurts")]
public class Rule_PoisonHurts : RuleCard
{
    public override void OnPickup(PickupEvent e, RuleContext ctx)
    {
        if (e.pickupType != PickupType.Poison) return;
        ctx.healthDeltas.Add(-e.baseValue);
    }
}
