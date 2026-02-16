using UnityEngine;

[CreateAssetMenu(menuName = "Rules/PowerUp Hurts")]
public class Rule_PowerUpHurts : RuleCard
{
    public override void OnPickup(PickupEvent e, RuleContext ctx)
    {
        if (e.pickupType != PickupType.PowerUp) return;
        ctx.healthDeltas.Add(-e.baseValue);
    }
}
