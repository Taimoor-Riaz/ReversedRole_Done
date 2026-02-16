using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int startHealth = 10;

    public int Health { get; private set; }
    public int Score { get; private set; }

    private void Awake()
    {
        Health = Mathf.Clamp(startHealth, 0, maxHealth);
        Score = 0;
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerHealthDelta += OnHealthDelta;
        GameEvents.OnScoreDelta += OnScoreDelta;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerHealthDelta -= OnHealthDelta;
        GameEvents.OnScoreDelta -= OnScoreDelta;
    }

    private void OnHealthDelta(int delta)
    {
        Health = Mathf.Clamp(Health + delta, 0, maxHealth);
        // TODO: update UI here (later via another event if you want)
        if (Health <= 0)
        {
            Debug.Log("Player Dead");
            GameEvents.OnGameLose?.Invoke();
        }
    }

    private void OnScoreDelta(int delta)
    {
        Score += delta;
    }
}
