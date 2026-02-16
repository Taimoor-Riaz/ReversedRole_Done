using UnityEngine;

public class SimpleCameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 offset = Vector2.zero;

    [Tooltip("Lower = snappier, Higher = smoother")]
    [SerializeField] private float smoothTime = 0.15f;

    private Vector3 _velocity;

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 desired = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desired,
            ref _velocity,
            smoothTime
        );
    }

    public void SetTarget(Transform t) => target = t;
}
