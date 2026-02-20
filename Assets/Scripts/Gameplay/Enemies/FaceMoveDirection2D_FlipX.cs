using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FaceMoveDirection2D_FlipX : MonoBehaviour
{
    [SerializeField] private Transform visualRoot; // assign zombie sprite/rig root
    [SerializeField] private float minSpeedToFlip = 0.05f;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (!visualRoot) visualRoot = transform;
    }

    private void LateUpdate()
    {
        Vector2 v = _rb.linearVelocity;
        if (v.sqrMagnitude < minSpeedToFlip * minSpeedToFlip) return;

        bool faceRight = v.x >= 0f;

        Vector3 s = visualRoot.localScale;
        s.x = Mathf.Abs(s.x) * (faceRight ? 1f : -1f);
        visualRoot.localScale = s;
    }
}
