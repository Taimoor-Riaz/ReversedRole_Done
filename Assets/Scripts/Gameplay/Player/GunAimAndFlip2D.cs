using UnityEngine;

public class GunAimAndFlip2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform visualRoot;          // flip this (character visuals/bones)
    [SerializeField] private Transform gunTransform;        // the Gun object that rotates
    [SerializeField] private SpriteRenderer gunSprite;      // Gun sprite renderer
    [SerializeField] private Camera worldCamera;

    [Header("Pivot Side Settings")]
    [Tooltip("Local position of GunPivot when aiming RIGHT (set in Inspector).")]
    [SerializeField] private Vector3 rightLocalPos = new Vector3(0.4f, 0f, 0f);

    [Tooltip("Local position of GunPivot when aiming LEFT (auto mirrored from rightLocalPos).")]
    [SerializeField] private bool autoMirrorLeft = true;
    [SerializeField] private Vector3 leftLocalPos = new Vector3(-0.4f, 0f, 0f);

    [Header("Aim")]
    [SerializeField] private float gunRotationOffset = 0f;  // 0 if gun points right, -90 if up
    [SerializeField] private float aimSmoothing = 0f;       // 0 = instant

    private Vector2 _aimScreen;
    private float _zVel;

    private void Awake()
    {
        if (!worldCamera) worldCamera = Camera.main;
        if (!gunTransform) gunTransform = transform; // fallback
        if (!gunSprite && gunTransform) gunSprite = gunTransform.GetComponentInChildren<SpriteRenderer>();

        if (autoMirrorLeft)
            leftLocalPos = new Vector3(-rightLocalPos.x, rightLocalPos.y, rightLocalPos.z);
    }

    private void OnEnable() => GameEvents.OnAimScreenInput += OnAimScreen;
    private void OnDisable() => GameEvents.OnAimScreenInput -= OnAimScreen;
    private void OnAimScreen(Vector2 s) => _aimScreen = s;

    private void LateUpdate()
    {
        if (!worldCamera || !gunTransform) return;

        Vector3 mw = worldCamera.ScreenToWorldPoint(new Vector3(_aimScreen.x, _aimScreen.y, 0f));
        Vector2 mouseWorld = new Vector2(mw.x, mw.y);

        // Decide facing based on mouse x relative to player/visual
        Vector2 faceOrigin = visualRoot ? (Vector2)visualRoot.position : (Vector2)transform.position;
        bool faceRight = (mouseWorld.x - faceOrigin.x) >= 0f;

        // 1) Flip character visuals
        if (visualRoot)
        {
            Vector3 vs = visualRoot.localScale;
            vs.x = Mathf.Abs(vs.x) * (faceRight ? 1f : -1f);
            visualRoot.localScale = vs;
        }

        // 2) Move gun pivot to correct side
        transform.localPosition = faceRight ? rightLocalPos : leftLocalPos;

        // 3) Rotate gun towards mouse (use gun pivot position as origin)
        Vector2 dir = mouseWorld - (Vector2)gunTransform.position;
        if (dir.sqrMagnitude < 0.0001f) return;

        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + gunRotationOffset;

        // 4) Flip gun sprite vertically when aiming left (keeps gun looking correct)
        if (gunSprite) gunSprite.flipY = !faceRight;

        if (aimSmoothing > 0f)
        {
            float current = gunTransform.eulerAngles.z;
            float smooth = Mathf.SmoothDampAngle(current, ang, ref _zVel, aimSmoothing);
            gunTransform.rotation = Quaternion.Euler(0f, 0f, smooth);
        }
        else
        {
            gunTransform.rotation = Quaternion.Euler(0f, 0f, ang);
        }
    }
}