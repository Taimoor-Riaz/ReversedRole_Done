using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int startHealth = 10;

    public int Health { get; private set; }
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        Health = Mathf.Clamp(startHealth, 0, maxHealth);
        GameEvents.OnHealthChanged?.Invoke(Health, maxHealth);
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerHealthDelta += OnHealthDelta;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerHealthDelta -= OnHealthDelta;
    }

    private void OnHealthDelta(int delta)
    {
        Health = Mathf.Clamp(Health + delta, 0, maxHealth);
        GameEvents.OnHealthChanged?.Invoke(Health, maxHealth);

        if (Health <= 0)
            GameEvents.OnGameLose?.Invoke();
    }
}
