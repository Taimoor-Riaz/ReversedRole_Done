using System.Collections.Generic;
using UnityEngine;

public class RuleEngine : MonoBehaviour
{
    [Header("Active Rules (Rule Cards)")]
    [SerializeField] private List<RuleCard> activeRules = new();

    private readonly RuleContext _ctx = new();

    private void OnEnable()
    {
        GameEvents.OnHit += HandleHit;
        GameEvents.OnPickup += HandlePickup;
    }

    private void OnDisable()
    {
        GameEvents.OnHit -= HandleHit;
        GameEvents.OnPickup -= HandlePickup;
    }

    public void SetRules(List<RuleCard> rules)
    {
        activeRules = rules;
    }

    private void HandleHit(HitEvent e)
    {
        _ctx.Clear();

        for (int i = 0; i < activeRules.Count; i++)
            activeRules[i].OnHit(e, _ctx);

        ApplyContext();
    }

    private void HandlePickup(PickupEvent e)
    {
        _ctx.Clear();

        for (int i = 0; i < activeRules.Count; i++)
            activeRules[i].OnPickup(e, _ctx);

        ApplyContext();
    }

    private void ApplyContext()
    {
        for (int i = 0; i < _ctx.healthDeltas.Count; i++)
            GameEvents.OnPlayerHealthDelta?.Invoke(_ctx.healthDeltas[i]);

        for (int i = 0; i < _ctx.scoreDeltas.Count; i++)
            GameEvents.OnScoreDelta?.Invoke(_ctx.scoreDeltas[i]);

        for (int i = 0; i < _ctx.damages.Count; i++)
            GameEvents.OnDamageTarget?.Invoke(_ctx.damages[i]);
    }
}
