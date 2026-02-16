using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleTimelineManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RuleEngine ruleEngine;

    [Header("Timeline Steps (in order)")]
    [SerializeField] private List<TimelineStepSO> steps = new();

    [Header("Announcements")]
    [SerializeField] private bool showGameStartMessage = true;
    [SerializeField] private float gameStartDuration = 2.5f;
    [SerializeField] private string gameStartTitle = "GAME START";
    [TextArea] [SerializeField] private string gameStartBody = "Don’t trust the rules.";

    [SerializeField] private string ruleTitle = "NEW RULE";
    [SerializeField] private float ruleDuration = 2.0f;

    private int _index;
    private float _timer;

    private void Awake()
    {
        if (!ruleEngine) ruleEngine = GetComponent<RuleEngine>();
    }

    private void Start()
    {
        if (steps.Count == 0 || !steps[0] || !steps[0].phase) return;
        StartCoroutine(InitPhase0Routine());
    }

    private IEnumerator InitPhase0Routine()
    {
        // Wait 1 frame so all spawners/components subscribe to events
        yield return null;

        _index = 0;
        _timer = 0f;

        // Apply phase 0 without announcing rule yet
        ApplyStep(_index, announceRule: false);

        // Announce: Game start first, then Rule 1
        yield return StartCoroutine(AnnounceStartThenFirstRule());
    }

    private void Update()
    {
        if (steps.Count == 0) return;

        _timer += Time.deltaTime;
        if (_timer < steps[_index].duration) return;

        _timer = 0f;
        _index++;

        if (_index >= steps.Count)
        {
            enabled = false;
            return;
        }

        ApplyStep(_index, announceRule: true);
    }

    private void ApplyStep(int i, bool announceRule)
    {
        var step = steps[i];
        if (!step || !step.phase || !ruleEngine) return;

        // 1) Activate rules for this phase
        ruleEngine.SetRules(step.phase.rules);

        // 2) Broadcast phase initiated (THIS enables spawners, etc.)
        GameEvents.OnPhaseChanged?.Invoke(i, step.phase.phaseName);

        // 3) Apply modifiers only if overridden
        if (step.phase.overrideMoveMultiplier)
            GameEvents.OnMoveMultiplierChanged?.Invoke(step.phase.moveMultiplier);

        if (step.phase.overrideAimMultiplier)
            GameEvents.OnAimMultiplierChanged?.Invoke(step.phase.aimMultiplier);

        // 4) Announce rule change (optional)
        if (announceRule)
            AnnounceRule(step.bannerText, AnnouncementType.RuleFlip);
    }

    private IEnumerator AnnounceStartThenFirstRule()
    {
        if (showGameStartMessage)
        {
            GameEvents.OnAnnouncement?.Invoke(
                new Announcement(AnnouncementType.GameStart, gameStartTitle, gameStartBody, gameStartDuration)
            );

            yield return new WaitForSeconds(gameStartDuration + 0.2f);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Rule 1 announcement after intro
        AnnounceRule(steps[_index].bannerText, AnnouncementType.RuleStart);
    }

    private void AnnounceRule(string body, AnnouncementType type)
    {
        if (string.IsNullOrWhiteSpace(body)) body = "Rules updated.";

        GameEvents.OnAnnouncement?.Invoke(
            new Announcement(type, ruleTitle, body, ruleDuration)
        );
    }
}
