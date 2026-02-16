using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference aimAction;
    [SerializeField] private InputActionReference fireAction;

    private void OnEnable()
    {
        if (moveAction) moveAction.action.Enable();
        if (aimAction) aimAction.action.Enable();
        if (fireAction)
        {
            fireAction.action.Enable();
            fireAction.action.performed += OnFire;
        }
    }

    private void OnDisable()
    {
        if (fireAction)
        {
            fireAction.action.performed -= OnFire;
            fireAction.action.Disable();
        }
        if (aimAction) aimAction.action.Disable();
        if (moveAction) moveAction.action.Disable();
    }

    private void Update()
    {
        if (moveAction)
        {
            Vector2 move = moveAction.action.ReadValue<Vector2>();
            GameEvents.OnMoveInput?.Invoke(move);
        }

        if (aimAction)
        {
            Vector2 aimScreen = aimAction.action.ReadValue<Vector2>();
            GameEvents.OnAimScreenInput?.Invoke(aimScreen);
        }
    }

    private void OnFire(InputAction.CallbackContext ctx)
    {
        GameEvents.OnFirePressed?.Invoke();
    }
}
