using UnityEngine;

public class SurvivalGoalManager : MonoBehaviour
{
    [SerializeField] private float surviveSeconds = 120f;

    private float _t;
    private bool _ended;

    private void OnEnable()
    {
        GameEvents.OnGameLose += OnEnded;
        GameEvents.OnGameWin += OnEnded;
    }

    private void OnDisable()
    {
        GameEvents.OnGameLose -= OnEnded;
        GameEvents.OnGameWin -= OnEnded;
    }

    private void Update()
    {
        if (_ended) return;

        _t += Time.deltaTime;
        if (_t >= surviveSeconds)
        {
            _ended = true;
            GameEvents.OnGameWin?.Invoke();
        }
    }

    private void OnEnded() => _ended = true;

    public float Remaining => Mathf.Max(0f, surviveSeconds - _t);
}
