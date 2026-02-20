using UnityEngine;

[RequireComponent(typeof(PlayerMotor2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerMotor2D _motor;

    private Vector2 _moveInput;
    private Vector2 _moveMultiplier = Vector2.one; // default normal

    private void Awake()
    {
        _motor = GetComponent<PlayerMotor2D>();
    }

    private void OnEnable()
    {
        GameEvents.OnMoveInput += HandleMoveInput;
        GameEvents.OnMoveMultiplierChanged += HandleMoveMultiplier;
    }

    private void OnDisable()
    {
        GameEvents.OnMoveInput -= HandleMoveInput;
        GameEvents.OnMoveMultiplierChanged -= HandleMoveMultiplier;
    }

    private void HandleMoveInput(Vector2 input)
    {
        _moveInput = input;
    }

    private void HandleMoveMultiplier(Vector2 mult)
    {
        // e.g. (-1,1) flips left/right
        _moveMultiplier = new Vector2(
            mult.x == 0 ? 1 : mult.x,
            mult.y == 0 ? 1 : mult.y
        );
    }

    private void Update()
    {
        // Normalize so diagonal isn’t faster
        Vector2 move = _moveInput;
        if (move.sqrMagnitude > 1f) move.Normalize();

        // Apply rule modifier
        move = Vector2.Scale(move, _moveMultiplier);

       // _motor.SetMove(move);
    }
}
