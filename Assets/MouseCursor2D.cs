using UnityEngine;

public class MouseCursor2D : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Camera worldCamera;
    [SerializeField] private SpriteRenderer cursorSpriteRenderer;
    [SerializeField] private Sprite cursorSprite;
    [SerializeField] private Vector3 worldOffset = Vector3.zero;

    [Header("Cursor Settings")]
    [SerializeField] private bool hideSystemCursor = true;

    private float _camToPlane;

    private void Awake()
    {
        if (worldCamera == null) worldCamera = Camera.main;

        // plane is z = 0 (top-down 2D). camera usually at z = -10
        if (worldCamera != null)
            _camToPlane = -worldCamera.transform.position.z;

        if (cursorSpriteRenderer != null && cursorSprite != null)
            cursorSpriteRenderer.sprite = cursorSprite;
    }

    private void OnEnable()
    {
        if (hideSystemCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None; // IMPORTANT: don’t lock it
        }
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (worldCamera == null || cursorSpriteRenderer == null) return;

        Vector3 mouseScreenPos = Input.mousePosition;

        // ✅ must set Z distance from camera to z=0 plane
        mouseScreenPos.z = _camToPlane;

        Vector3 mouseWorldPos = worldCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        transform.position = mouseWorldPos + worldOffset;
    }

    public void SetCursorSprite(Sprite newSprite)
    {
        cursorSprite = newSprite;
        if (cursorSpriteRenderer != null)
            cursorSpriteRenderer.sprite = cursorSprite;
    }
}
