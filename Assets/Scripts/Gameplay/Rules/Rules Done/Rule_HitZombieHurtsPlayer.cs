using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Hit Zombie Hurts Player")]
public class Rule_HitZombieHurtsPlayer : RuleCard
{
    [SerializeField] private int playerDamage = 1;
    [SerializeField] private int zombieKillDamage = 999; // enough to kill
    public override void OnHit(HitEvent e, RuleContext ctx)
    {
        if (e.targetType != TargetType.Zombie) return;
        ctx.healthDeltas.Add(-playerDamage);
        // ✅ kill THIS princess instance
        if (e.target != null)
        {
            ctx.damages.Add(new DamageRequest
            {
                target = e.target,
                damage = zombieKillDamage
            });
        }
    }
}
