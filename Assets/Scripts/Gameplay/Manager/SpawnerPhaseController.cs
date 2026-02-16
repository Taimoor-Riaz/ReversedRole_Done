using UnityEngine;

public class SpawnerPhaseController : MonoBehaviour
{
    [SerializeField] private WaveSpawner2D spawner;
    [SerializeField] private int enableFromPhase = 0;
    [SerializeField] private int disableAtPhase = -1; // -1 = never disable

    private void Awake()
    {
        if (!spawner) spawner = GetComponent<WaveSpawner2D>();
    }

    private void OnEnable() => GameEvents.OnPhaseChanged += OnPhaseChanged;
    private void OnDisable() => GameEvents.OnPhaseChanged -= OnPhaseChanged;

    private void OnPhaseChanged(int index, string phaseName)
    {
        if (!spawner) return;

        bool shouldEnable = index >= enableFromPhase && (disableAtPhase < 0 || index < disableAtPhase);
        spawner.SetActive(shouldEnable);
    }
}
