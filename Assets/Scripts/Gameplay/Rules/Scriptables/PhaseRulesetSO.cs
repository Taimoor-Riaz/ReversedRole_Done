using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Phase Ruleset")]
public class PhaseRulesetSO : ScriptableObject
{
    public string phaseName = "Phase";

    [Header("Optional Modifiers (leave default if not changing)")]
    public bool overrideMoveMultiplier;
    public Vector2 moveMultiplier = Vector2.one;

    public bool overrideAimMultiplier;
    public float aimMultiplier = 1f;

    [Header("Active Rule Cards for this phase")]
    public List<RuleCard> rules = new();
    [Header("Contact Modes")]
    public GameRuleState.ContactMode zombieContactMode = GameRuleState.ContactMode.DamagePlayer;
    public GameRuleState.ContactMode princessContactMode = GameRuleState.ContactMode.None;
}
