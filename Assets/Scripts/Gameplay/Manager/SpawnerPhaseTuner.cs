using System.Collections.Generic;
using UnityEngine;

public class SpawnerPhaseTuner : MonoBehaviour
{
    [SerializeField] private WaveSpawner2D spawner;

    [Header("Default (if phase not found)")]
    [SerializeField] private bool defaultActive = true;
    [SerializeField]
    private WaveSpawner2D.SpawnerSettings defaultSettings =
        new WaveSpawner2D.SpawnerSettings { interval = 1.2f, maxAlive = 8, minRadius = 6f, maxRadius = 10f };

    [System.Serializable]
    public class PhaseEntry
    {
        public int phaseIndex;
        public bool active = true;
        public WaveSpawner2D.SpawnerSettings settings =
            new WaveSpawner2D.SpawnerSettings { interval = 1.2f, maxAlive = 8, minRadius = 6f, maxRadius = 10f };
    }

    [Header("Per Phase Settings")]
    [SerializeField] private List<PhaseEntry> phases = new();

    private void Awake()
    {
        if (!spawner) spawner = GetComponent<WaveSpawner2D>();
    }

    private void OnEnable()
    {
        GameEvents.OnPhaseChanged += OnPhaseChanged;
        ApplyDefault(); // so it starts correctly in editor play
    }

    private void OnDisable()
    {
        GameEvents.OnPhaseChanged -= OnPhaseChanged;
    }

    private void ApplyDefault()
    {
        if (!spawner) return;
        spawner.SetActive(defaultActive);
        spawner.ApplySettings(defaultSettings);
    }

    private void OnPhaseChanged(int index, string phaseName)
    {
        if (!spawner) return;

        // Find exact phase entry
        for (int i = 0; i < phases.Count; i++)
        {
            if (phases[i].phaseIndex != index) continue;

            spawner.SetActive(phases[i].active);
            spawner.ApplySettings(phases[i].settings);
            return;
        }

        // Not found => default
        ApplyDefault();
    }
}
