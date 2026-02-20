using UnityEngine;

public class DamageableTarget : MonoBehaviour
{
    [SerializeField] private int health = 3;

    [Header("Death")]
    [SerializeField] private GameObject deadEffect;
    [SerializeField] private float destroyDelay = 0.6f; // set in Inspector

 

    private void OnEnable() => GameEvents.OnDamageTarget += OnDamageRequest;
    private void OnDisable() => GameEvents.OnDamageTarget -= OnDamageRequest;

    private void OnDamageRequest(DamageRequest req)
    {
        if (req.target != this) return;

        health -= req.damage;
        if (health > 0) return;

      

        if (deadEffect)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        Destroy(gameObject, destroyDelay);
    }
}
