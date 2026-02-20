using UnityEngine;

public class PlayerMoveFX : MonoBehaviour
{
    [Header("Dust Prefab")]
    [SerializeField] private GameObject dustPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float dustLife = 1.0f;
    [SerializeField] private Vector2 randomOffset = new Vector2(0.05f, 0.03f);

    [Header("Dust Timing")]
    [SerializeField] private float dustSpawnInterval = 0.12f;
    [SerializeField] private float moveThreshold = 0.05f;

    [Header("Footstep Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField, Range(0f, 1f)] private float volume = 0.6f;
    [SerializeField] private float soundInterval = 0.25f; // play step sound every X sec
    [SerializeField] private float pitchMin = 0.95f;
    [SerializeField] private float pitchMax = 1.05f;

    private Vector2 _move;
    private float _dustTimer;
    private float _soundTimer;

    private void Awake()
    {
        if (!spawnPoint) spawnPoint = transform;
        if (!audioSource) audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() => GameEvents.OnMoveInput += OnMoveInput;
    private void OnDisable() => GameEvents.OnMoveInput -= OnMoveInput;

    private void OnMoveInput(Vector2 move) => _move = move;

    private void Update()
    {
        bool moving = _move.sqrMagnitude > (moveThreshold * moveThreshold);

        if (!moving)
        {
            _dustTimer = 0f;
            _soundTimer = 0f;
            return;
        }

        // ---- Dust spawn ----
        if (dustPrefab)
        {
            _dustTimer += Time.deltaTime;
            if (_dustTimer >= dustSpawnInterval)
            {
                _dustTimer = 0f;
                SpawnDust();
            }
        }

        // ---- Footstep sound ----
        if (audioSource && footstepClip)
        {
            _soundTimer += Time.deltaTime;
            if (_soundTimer >= soundInterval)
            {
                _soundTimer = 0f;
                PlayFootstep();
            }
        }
    }

    private void SpawnDust()
    {
        Vector3 basePos = spawnPoint.position;

        Vector3 off = new Vector3(
            Random.Range(-randomOffset.x, randomOffset.x),
            Random.Range(-randomOffset.y, randomOffset.y),
            0f
        );

        GameObject fx = Instantiate(dustPrefab, basePos + off, Quaternion.identity);

        // If prefab already destroys itself, set dustLife = 0
        if (dustLife > 0f)
            Destroy(fx, dustLife);
    }

    private void PlayFootstep()
    {
        float oldPitch = audioSource.pitch;
        audioSource.pitch = Random.Range(pitchMin, pitchMax);

        audioSource.PlayOneShot(footstepClip, volume);

        audioSource.pitch = oldPitch;
    }
}