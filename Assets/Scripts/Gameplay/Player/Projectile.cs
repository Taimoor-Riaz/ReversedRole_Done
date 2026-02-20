using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int baseDamage = 1;

    [Header("Hit VFX")]
    [SerializeField] private GameObject hitEffectPrefab; // assign prefab in Inspector
    [SerializeField] private float hitEffectLife = 1.2f;  // auto destroy VFX

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
        if (other.CompareTag("Player")) return;

        // Spawn VFX at true hit point (closest point on collider)
        SpawnHitEffect(other);

        // Find DamageableTarget on the object or its parents
        DamageableTarget dmg = other.GetComponentInParent<DamageableTarget>();
        TargetTag tag = other.GetComponentInParent<TargetTag>();

        if (dmg != null && tag != null)
        {
            Vector2 hitPoint = other.ClosestPoint(transform.position);

            GameEvents.OnHit?.Invoke(new HitEvent
            {
                targetType = tag.TargetType,
                target = dmg,
                hitPoint = hitPoint,
                baseDamage = baseDamage
            });
        }

        Destroy(gameObject);
    }

    private void SpawnHitEffect(Collider2D other)
    {
        if (!hitEffectPrefab) return;

        Vector3 p = other.ClosestPoint(transform.position);
        var fx = Instantiate(hitEffectPrefab, p, Quaternion.identity);

        if (hitEffectLife > 0f)
            Destroy(fx, hitEffectLife);
    }
}
