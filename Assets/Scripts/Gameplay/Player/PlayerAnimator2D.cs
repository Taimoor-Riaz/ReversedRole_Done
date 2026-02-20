using UnityEngine;

public class PlayerAnimator2D : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private string speedParam = "Speed";

    [Header("Flip (Mouse + Buttons)")]
    [SerializeField] private bool enableFlip = true;
    [SerializeField] private Transform visualRoot;

    [Tooltip("If mouse aim updates within this time window, face mouse. Otherwise face move.")]
    [SerializeField] private float mousePrioritySeconds = 0.15f;

    [SerializeField] private float moveDeadZone = 0.05f;

    private Vector2 _move;
    private float _speed;

    private Vector2 _aimScreen;
    private float _lastAimTime = -999f;

    private Camera _cam;
    private float _faceSign = 1f;

    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!visualRoot) visualRoot = transform;
        _cam = Camera.main;
    }

    private void OnEnable()
    {
        GameEvents.OnMoveInput += OnMoveInput;
        GameEvents.OnAimScreenInput += OnAimScreenInput;
    }

    private void OnDisable()
    {
        GameEvents.OnMoveInput -= OnMoveInput;
        GameEvents.OnAimScreenInput -= OnAimScreenInput;
    }

    private void OnMoveInput(Vector2 move)
    {
        _move = move;
        _speed = move.magnitude;
    }

    private void OnAimScreenInput(Vector2 screenPos)
    {
        _aimScreen = screenPos;
        _lastAimTime = Time.time;
    }

    private void Update()
    {
        if (animator)
            animator.SetFloat(speedParam, _speed);

        if (!enableFlip || !visualRoot) return;

        bool usedAim = false;

        // 1) Prefer mouse aim if it was updated recently
        if (_cam && (Time.time - _lastAimTime) <= mousePrioritySeconds)
        {
            Vector3 mw = _cam.ScreenToWorldPoint(new Vector3(_aimScreen.x, _aimScreen.y, 0f));
            float dx = mw.x - visualRoot.position.x;

            if (Mathf.Abs(dx) > 0.001f)
            {
                _faceSign = (dx >= 0f) ? 1f : -1f;
                usedAim = true;
            }
        }

        // 2) Otherwise face movement direction
        if (!usedAim && Mathf.Abs(_move.x) > moveDeadZone)
        {
            _faceSign = (_move.x >= 0f) ? 1f : -1f;
        }

        // Apply flip
        Vector3 s = visualRoot.localScale;
        s.x = Mathf.Abs(s.x) * _faceSign;
        visualRoot.localScale = s;
    }

    // Optional control from rules/phases
    public void SetFlipEnabled(bool on) => enableFlip = on;
}
