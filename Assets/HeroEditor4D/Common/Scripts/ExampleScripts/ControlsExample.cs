using UnityEngine;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;

namespace Assets.HeroEditor4D.Common.Scripts.ExampleScripts
{
    public class ControlsExample : MonoBehaviour
    {
        [Header("References")]
        public Character4D Character;
        public FirearmFxExample FirearmFx;

        [Header("Settings")]
        public bool InitDirection;
        public int MovementSpeed = 3;

        private bool _moving;
        private Vector2 _lastNonZeroDir = Vector2.down;

        // ✅ controlled by rules
        private Vector2 _moveMultiplier = Vector2.one;

        private void OnEnable()
        {
            GameEvents.OnMoveMultiplierChanged += SetMoveMultiplier;
        }

        private void OnDisable()
        {
            GameEvents.OnMoveMultiplierChanged -= SetMoveMultiplier;
        }

        private void SetMoveMultiplier(Vector2 m)
        {
            // Safety (avoid zero multipliers)
            if (m == Vector2.zero) m = Vector2.one;
            _moveMultiplier = m;
        }

        private void Start()
        {
            if (!Character) return;

            Character.AnimationManager.SetState(CharacterState.Idle);

            if (InitDirection)
            {
                _lastNonZeroDir = Vector2.down;
                Character.SetDirection(_lastNonZeroDir);
            }
        }

        private void Update()
        {
            if (!Character) return;

            var input = ReadMoveInput();

            // ✅ apply reversal rules
            input = ApplyMoveRules(input);

            UpdateDirection(input);
            UpdateMovement(input);
        }

        private Vector2 ReadMoveInput()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            return new Vector2(x, y);
        }

        private Vector2 ApplyMoveRules(Vector2 input)
        {
            // Reverse left/right with x=-1, reverse up/down with y=-1
            return new Vector2(input.x * _moveMultiplier.x, input.y * _moveMultiplier.y);
        }

        private void UpdateDirection(Vector2 input)
        {
            if (input == Vector2.zero) return;

            _lastNonZeroDir = input.normalized;

            // HeroEditor4D expects 4-dir
            Character.SetDirection(ToCardinal(_lastNonZeroDir));
        }

        private void UpdateMovement(Vector2 input)
        {
            if (MovementSpeed <= 0) return;

            if (input == Vector2.zero)
            {
                if (_moving)
                {
                    Character.AnimationManager.SetState(CharacterState.Idle);
                    _moving = false;
                }
                return;
            }

            if (!_moving)
            {
                Character.AnimationManager.SetState(CharacterState.Run);
                _moving = true;
            }

            // Normalize to avoid diagonal speed boost
            var moveDir = input.normalized;
            Character.transform.position += (Vector3)(moveDir * MovementSpeed * Time.deltaTime);
        }

        private Vector2 ToCardinal(Vector2 dir)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                return dir.x >= 0 ? Vector2.right : Vector2.left;

            return dir.y >= 0 ? Vector2.up : Vector2.down;
        }
    }
}
