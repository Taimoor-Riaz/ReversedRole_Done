using UnityEngine;

[CreateAssetMenu(menuName = "Rules/PowerUp Heals")]
public class Rule_PowerUpHeals : RuleCard
{
    public override void OnPickup(PickupEvent e, RuleContext ctx)
    {
        if (e.pickupType != PickupType.PowerUp) return;
        ctx.healthDeltas.Add(+e.baseValue);
    }
}
