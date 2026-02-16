using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody2D _rb;
    private Vector2 _desiredVelocity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetMove(Vector2 moveWorld)
    {
        // moveWorld already includes modifiers, normalization, etc.
        _desiredVelocity = moveWorld * moveSpeed;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _desiredVelocity;
    }
}
