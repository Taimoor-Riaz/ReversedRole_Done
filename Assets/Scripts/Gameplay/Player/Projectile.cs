using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int baseDamage = 1;

    private Rigidbody2D _rb;
    private float _timer;

    private void Awake() => _rb = GetComponent<Rigidbody2D>();

    public void Launch(Vector2 velocity)
    {
        _rb.linearVelocity = velocity;
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= lifeTime) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<TargetTag>(out var tag))
        {
            GameEvents.OnHit?.Invoke(new HitEvent
            {
                targetType = tag.TargetType,
                hitPoint = transform.position,
                baseDamage = baseDamage
            });
        }

        Destroy(gameObject);
    }
}
