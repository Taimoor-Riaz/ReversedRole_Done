using UnityEngine;

public class ContactDamage2D : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float cooldown = 0.7f;

    private float _timer;

    private void Update()
    {
        if (_timer > 0f) _timer -= Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_timer > 0f) return;
        if (!collision.collider.CompareTag("Player")) return;

        _timer = cooldown;
        GameEvents.OnPlayerHealthDelta?.Invoke(-damage);
    }

    // If you use triggers instead of collisions:
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_timer > 0f) return;
        if (!other.CompareTag("Player")) return;

        _timer = cooldown;
        GameEvents.OnPlayerHealthDelta?.Invoke(-damage);
    }
}
