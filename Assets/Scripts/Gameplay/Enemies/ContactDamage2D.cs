using UnityEngine;

public class ContactDamage2D : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private int damage = 1;
    [SerializeField] private int heal = 1;

    [Header("Destroy On Touch")]
    [SerializeField] private bool destroySelfOnTouch = true;

    [Header("Death FX")]
    [SerializeField] private GameObject deadEffect;
    [SerializeField] private float destroyDelay = 0.6f;

    [Header("Type (auto from TargetTag)")]
    [SerializeField] private TargetTag targetTag; // auto-detect if empty

    private void Awake()
    {
        if (!targetTag) targetTag = GetComponentInParent<TargetTag>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // ✅ pick contact mode based on what THIS object is
        var mode = GetModeForThisTarget();

        switch (mode)
        {
            case GameRuleState.ContactMode.DamagePlayer:
                GameEvents.OnPlayerHealthDelta?.Invoke(-damage);
                break;

            case GameRuleState.ContactMode.HealPlayer:
                GameEvents.OnPlayerHealthDelta?.Invoke(+heal);
                break;

            case GameRuleState.ContactMode.None:
            default:
                break;
        }

        if (destroySelfOnTouch)
        {
            if (deadEffect)
                Instantiate(deadEffect, transform.position, Quaternion.identity);

            Destroy(gameObject, destroyDelay);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // ✅ pick contact mode based on what THIS object is
        var mode = GetModeForThisTarget();

        switch (mode)
        {
            case GameRuleState.ContactMode.DamagePlayer:
                GameEvents.OnPlayerHealthDelta?.Invoke(-damage);
                break;

            case GameRuleState.ContactMode.HealPlayer:
                GameEvents.OnPlayerHealthDelta?.Invoke(+heal);
                break;

            case GameRuleState.ContactMode.None:
            default:
                break;
        }

        if (destroySelfOnTouch)
        {
            if (deadEffect)
                Instantiate(deadEffect, transform.position, Quaternion.identity);

            Destroy(gameObject, destroyDelay);
        }
    }

    private GameRuleState.ContactMode GetModeForThisTarget()
    {
        // If TargetTag missing, fallback to Zombie mode (keeps old behavior)
        if (!targetTag)
            return (GameRuleState.ContactMode)GameRuleState.ZombieContact;

        switch (targetTag.TargetType)
        {
            case TargetType.Zombie:
                return (GameRuleState.ContactMode)GameRuleState.ZombieContact;

            case TargetType.Princess:
                return GameRuleState.PrincessContact;

            default:
                return GameRuleState.ContactMode.None;
        }
    }
}
