using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private PickupType pickupType;
    [SerializeField] private int baseValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If player touches
        if (!other.CompareTag("Player")) return;

        GameEvents.OnPickup?.Invoke(new PickupEvent
        {
            pickupType = pickupType,
            baseValue = baseValue
        });

        Destroy(gameObject);
    }
}
