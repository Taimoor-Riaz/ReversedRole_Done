using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ZombieChase2D : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;

    private Rigidbody2D _rb;
    private Transform _target;

    private void Awake() => _rb = GetComponent<Rigidbody2D>();

    private void Start()
    {
        // Simple: find player by tag
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player) _target = player.transform;
    }

    private void FixedUpdate()
    {
        if (!_target) return;

        Vector2 dir = ((Vector2)_target.position - _rb.position);
        if (dir.sqrMagnitude > 0.0001f) dir.Normalize();

        _rb.linearVelocity = dir * speed;
    }
}
