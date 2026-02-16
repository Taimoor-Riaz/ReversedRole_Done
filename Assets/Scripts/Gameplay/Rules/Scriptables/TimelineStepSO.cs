using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Timeline Step")]
public class TimelineStepSO : ScriptableObject
{
    public PhaseRulesetSO phase;
    public float duration = 20f;   // seconds
    public string bannerText = "New Rule!";
}
