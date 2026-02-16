using UnityEngine;

public class DamageableTarget : MonoBehaviour
{
    [SerializeField] private TargetType targetType;
    [SerializeField] private int health = 3;

    private void OnEnable()
    {
        GameEvents.OnDamageTarget += OnDamageRequest;
    }

    private void OnDisable()
    {
        GameEvents.OnDamageTarget -= OnDamageRequest;
    }

    private void OnDamageRequest(DamageRequest req)
    {
        if (req.targetType != targetType) return;

        health -= req.damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
