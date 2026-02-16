using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Hit Princess Gives Score")]
public class Rule_HitPrincessGivesScore : RuleCard
{
    [SerializeField] private int scoreGain = 50;

    public override void OnHit(HitEvent e, RuleContext ctx)
    {
        if (e.targetType != TargetType.Princess) return;
        ctx.scoreDeltas.Add(scoreGain);
    }
}
