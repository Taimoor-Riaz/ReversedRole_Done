using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor2D : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5.5f;
    [SerializeField] private float acceleration = 40f;   // how fast it reaches target speed
    [SerializeField] private float deceleration = 50f;   // how fast it stops
    [SerializeField] private bool normalizeInput = true; // keeps diagonals same speed

    [Header("Optional Clamp")]
    [SerializeField] private float maxVelocity = 999f;

    private Rigidbody2D _rb;
    private Vector2 _inputMove;      // raw input
    private Vector2 _desiredVel;     // target velocity
    private Vector2 _vel;            // current rb velocity cache

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true; // prevents spin
    }

    private void OnEnable() => GameEvents.OnMoveInput += OnMoveInput;
    private void OnDisable() => GameEvents.OnMoveInput -= OnMoveInput;

    private void OnMoveInput(Vector2 move)
    {
        // move comes from input system (-1..1)
        _inputMove = move;
        if (normalizeInput && _inputMove.sqrMagnitude > 1f)
            _inputMove.Normalize();
    }

    private void FixedUpdate()
    {
        _vel = _rb.linearVelocity;

        _desiredVel = _inputMove * moveSpeed;

        // Choose accel or decel
        float rate = (_inputMove.sqrMagnitude > 0.0001f) ? acceleration : deceleration;

        // Smooth velocity change
        _vel = Vector2.MoveTowards(_vel, _desiredVel, rate * Time.fixedDeltaTime);

        // Optional clamp
        if (_vel.magnitude > maxVelocity)
            _vel = _vel.normalized * maxVelocity;

        _rb.linearVelocity = _vel;
    }

    public Vector2 CurrentVelocity => _rb ? _rb.linearVelocity : Vector2.zero;
}
