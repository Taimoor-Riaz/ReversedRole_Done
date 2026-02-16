using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AnnouncementUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;

    [Header("Timing")]
    [SerializeField] private float defaultDuration = 2f;
    [SerializeField] private float gapBetweenMessages = 0.15f;

    [Header("Animation (Optional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string showTrigger = "Show";
    [SerializeField] private string hideTrigger = "Hide";

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameStartClip;
    [SerializeField] private AudioClip ruleStartClip;
    [SerializeField] private AudioClip ruleFlipClip;
    [SerializeField] private AudioClip warningClip;

    [Header("Hooks (Optional)")]
    public UnityEvent onShow;
    public UnityEvent onHide;

    private readonly Queue<Announcement> _queue = new();
    private float _timer;
    private float _gapTimer;
    private bool _isShowing;

    private void Awake()
    {
        if (!panelRoot) panelRoot = gameObject;
        panelRoot.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnAnnouncement += Enqueue;
    }

    private void OnDisable()
    {
        GameEvents.OnAnnouncement -= Enqueue;
    }

    private void Update()
    {
        // If showing, countdown current message
        if (_isShowing)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                HideNow();
                _gapTimer = gapBetweenMessages;
            }
            return;
        }

        // small gap so it doesn't look spammy
        if (_gapTimer > 0f)
        {
            _gapTimer -= Time.deltaTime;
            return;
        }

        // if not showing, show next in queue
        if (_queue.Count > 0)
        {
            ShowNow(_queue.Dequeue());
        }
    }

    public void Enqueue(Announcement a)
    {
        // Safety: if duration not set
        if (a.duration <= 0f) a.duration = defaultDuration;
        _queue.Enqueue(a);
    }

    private void ShowNow(Announcement a)
    {
        _isShowing = true;
        _timer = a.duration;

        if (titleText) titleText.text = a.title;
        if (bodyText) bodyText.text = a.body;

        panelRoot.SetActive(true);

        // Animation hook
        if (animator)
        {
            animator.ResetTrigger(hideTrigger);
            animator.SetTrigger(showTrigger);
        }

        // Audio hook
        PlayClipFor(a.type);

        onShow?.Invoke();
    }

    private void HideNow()
    {
        _isShowing = false;

        if (animator)
        {
            animator.ResetTrigger(showTrigger);
            animator.SetTrigger(hideTrigger);
            // panelRoot stays active; animator can scale/alpha it out
            // If you want panel to disable after animation, call DisablePanel() via Animation Event
        }
        else
        {
            panelRoot.SetActive(false);
        }

        onHide?.Invoke();
    }

    // Call this from an Animation Event at the end of hide animation
    public void DisablePanel()
    {
        if (panelRoot) panelRoot.SetActive(false);
    }

    private void PlayClipFor(AnnouncementType type)
    {
        if (!audioSource) return;

        AudioClip clip = type switch
        {
            AnnouncementType.GameStart => gameStartClip,
            AnnouncementType.RuleStart => ruleStartClip,
            AnnouncementType.RuleFlip => ruleFlipClip,
            AnnouncementType.Warning => warningClip,
            _ => null
        };

        if (clip) audioSource.PlayOneShot(clip);
    }
}
