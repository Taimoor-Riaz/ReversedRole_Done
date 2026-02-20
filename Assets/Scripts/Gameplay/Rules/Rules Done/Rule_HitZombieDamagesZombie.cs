using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Hit Zombie Damages Zombie")]
public class Rule_HitZombieDamagesZombie : RuleCard
{
    [SerializeField] private int healAmount = 1;
    public override void OnHit(HitEvent e, RuleContext ctx)
    {
        if (e.targetType != TargetType.Zombie) return;

        ctx.damages.Add(new DamageRequest
        {
            target = e.target,
            damage = e.baseDamage
        });
        ctx.healthDeltas.Add(+healAmount);
    }
}
