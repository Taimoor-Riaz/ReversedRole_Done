using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private PickupType pickupType;
    [SerializeField] private int baseValue = 1;
    [SerializeField] private GameObject Effect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If player touches
        if (!other.CompareTag("Player")) return;

        GameEvents.OnPickup?.Invoke(new PickupEvent
        {
            pickupType = pickupType,
            baseValue = baseValue
        });

        if (Effect)
            Instantiate(Effect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
