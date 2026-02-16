using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera worldCamera;
    [SerializeField] private Transform firePoint;            // empty child on player
    [SerializeField] private Projectile projectilePrefab;

    [Header("Tuning")]
    [SerializeField] private float projectileSpeed = 14f;
    [SerializeField] private float fireCooldown = 0.15f;

    private Vector2 _aimScreen;
    private float _aimMultiplier = 1f; // 1 = normal, -1 = reversed
    private float _cooldown;

    private void Awake()
    {
        if (!worldCamera) worldCamera = Camera.main;
    }

    private void OnEnable()
    {
        GameEvents.OnAimScreenInput += OnAimScreen;
        GameEvents.OnFirePressed += OnFire;
        GameEvents.OnAimMultiplierChanged += OnAimMultiplier;
    }

    private void OnDisable()
    {
        GameEvents.OnAimScreenInput -= OnAimScreen;
        GameEvents.OnFirePressed -= OnFire;
        GameEvents.OnAimMultiplierChanged -= OnAimMultiplier;
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;

        // Optional: rotate firePoint toward aim direction (nice feel)
        Vector2 dir = GetAimDirectionWorld();
        if (dir.sqrMagnitude > 0.0001f && firePoint)
        {
            float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, ang);
        }
    }

    private void OnAimScreen(Vector2 screenPos) => _aimScreen = screenPos;

    private void OnAimMultiplier(float mult)
    {
        _aimMultiplier = (Mathf.Abs(mult) < 0.001f) ? 1f : Mathf.Sign(mult);
    }

    private void OnFire()
    {
        if (_cooldown > 0f) return;
        if (!projectilePrefab || !firePoint) return;

        Vector2 dir = GetAimDirectionWorld();
        if (dir.sqrMagnitude < 0.0001f) return;

        _cooldown = fireCooldown;

        Projectile p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        p.Launch(dir.normalized * projectileSpeed);
    }

    private Vector2 GetAimDirectionWorld()
    {
        if (!worldCamera) return Vector2.right;

        Vector3 mouseWorld = worldCamera.ScreenToWorldPoint(new Vector3(_aimScreen.x, _aimScreen.y, 0f));
        Vector2 from = firePoint ? (Vector2)firePoint.position : (Vector2)transform.position;
        Vector2 dir = (Vector2)mouseWorld - from;

        // Reverse aim by flipping the direction
        dir *= _aimMultiplier;

        return dir;
    }
}
