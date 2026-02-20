using UnityEngine;

public class GunOrbitAim2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orbitCenter;     // ✅ the true center (OrbitCenter)
    [SerializeField] private Transform gun;             // Gun transform (child)
    [SerializeField] private Transform shootPoint;      // tip of barrel
    [SerializeField] private Camera worldCamera;

    [Header("Orbit")]
    [SerializeField] private float orbitRadius = 0.6f;
    [SerializeField] private float followSharpness = 40f; // higher = more snappy

    [Header("Rotation")]
    [Tooltip("0 if your gun sprite points UP by default. If it points RIGHT use 90.")]
    [SerializeField] private float gunRotationOffset = 0f;
    [SerializeField] private float rotateSharpness = 40f;

    private Vector2 _aimScreen;
    private bool _hasAim;

    private void Awake()
    {
        if (!worldCamera) worldCamera = Camera.main;

        if (!orbitCenter) orbitCenter = transform.root;     // fallback (but you should assign OrbitCenter)
        if (!gun) gun = transform.GetChild(0);
        if (!shootPoint && gun) shootPoint = gun.GetComponentInChildren<Transform>();
    }

    private void OnEnable() => GameEvents.OnAimScreenInput += OnAim;
    private void OnDisable() => GameEvents.OnAimScreenInput -= OnAim;

    private void OnAim(Vector2 s)
    {
        _aimScreen = s;
        _hasAim = true;
    }

    private void LateUpdate()
    {
        if (!worldCamera || !orbitCenter || !gun) return;

        // Keep pivot always exactly at orbit center
        transform.position = orbitCenter.position;

        Vector2 screen = _hasAim ? _aimScreen : (Vector2)Input.mousePosition;
        Vector3 mw3 = worldCamera.ScreenToWorldPoint(new Vector3(screen.x, screen.y, 0f));
        Vector2 mouseWorld = new Vector2(mw3.x, mw3.y);

        Vector2 origin = orbitCenter.position;
        Vector2 dir = mouseWorld - origin;

        if (dir.sqrMagnitude < 0.0001f) return;
        dir.Normalize();

        // 1) Orbit position (gun sits on circle around center)
        Vector2 desiredGunPos = origin + dir * orbitRadius;
        gun.position = Vector2.Lerp(gun.position, desiredGunPos, 1f - Mathf.Exp(-followSharpness * Time.deltaTime));

        // 2) Rotate gun so its UP axis points toward mouse
        // If gun points UP in sprite, use this formula:
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion desiredRot = Quaternion.Euler(0f, 0f, ang - 90f + gunRotationOffset);

        gun.rotation = Quaternion.Slerp(gun.rotation, desiredRot, 1f - Mathf.Exp(-rotateSharpness * Time.deltaTime));
    }
}
