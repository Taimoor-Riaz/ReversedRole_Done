using UnityEngine;

public abstract class RuleCard : ScriptableObject
{
    public string ruleName = "New Rule";
    [TextArea] public string description;

    public virtual void OnHit(HitEvent e, RuleContext ctx) { }
    public virtual void OnPickup(PickupEvent e, RuleContext ctx) { }
}
