using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner2D : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<GameObject> prefabs = new();

    [Header("Spawn Timing")]
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float interval = 2f;

    [Header("Limits")]
    [SerializeField] private int maxAlive = 10;

    [Header("Spawn Area (around player)")]
    [SerializeField] private Transform player;
    [SerializeField] private float minRadius = 6f;
    [SerializeField] private float maxRadius = 10f;

    [Header("Enable")]
    [SerializeField] private bool isActiveOnStart = false;

    private readonly List<GameObject> _alive = new();
    private float _t;
    private bool _active;

    private void Start()
    {
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        _active = isActiveOnStart;
        _t = -startDelay;
    }

    private void Update()
    {
        CleanupDead();

        if (!_active) return;
        if (!player) return;
        if (prefabs == null || prefabs.Count == 0) return;

        _t += Time.deltaTime;
        if (_t < interval) return;
        _t = 0f;

        if (_alive.Count >= maxAlive) return;

        SpawnOne();
    }

    private void SpawnOne()
    {
        Vector2 pos = GetRandomSpawnPos();
        var prefab = prefabs[Random.Range(0, prefabs.Count)];
        if (!prefab) return;

        var go = Instantiate(prefab, pos, Quaternion.identity);
        _alive.Add(go);
    }

    private Vector2 GetRandomSpawnPos()
    {
        Vector2 center = player.position;
        float r = Random.Range(minRadius, maxRadius);
        float a = Random.Range(0f, Mathf.PI * 2f);
        return center + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * r;
    }

    private void CleanupDead()
    {
        for (int i = _alive.Count - 1; i >= 0; i--)
            if (_alive[i] == null) _alive.RemoveAt(i);
    }

    public void SetActive(bool active)
    {
        _active = active;
        if (_active)
        {
            CleanupDead();
            _t = -startDelay; // restart delay each time it becomes active
        }
    }
    public void ApplySettings(SpawnerSettings s)
    {
        interval = Mathf.Max(0.05f, s.interval);
        maxAlive = Mathf.Max(0, s.maxAlive);
        minRadius = Mathf.Max(0f, s.minRadius);
        maxRadius = Mathf.Max(minRadius, s.maxRadius);
    }

    [System.Serializable]
    public struct SpawnerSettings
    {
        public float interval;
        public int maxAlive;
        public float minRadius;
        public float maxRadius;
    }
}
