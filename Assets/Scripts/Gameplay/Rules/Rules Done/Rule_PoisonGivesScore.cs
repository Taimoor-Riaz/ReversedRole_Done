using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Poison Gives Score")]
public class Rule_PoisonGivesScore : RuleCard
{
    [SerializeField] private int scorePerValue = 25;

    public override void OnPickup(PickupEvent e, RuleContext ctx)
    {
        if (e.pickupType != PickupType.Poison) return;
        ctx.scoreDeltas.Add(e.baseValue * scorePerValue);
    }
}
