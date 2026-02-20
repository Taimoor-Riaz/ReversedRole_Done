using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    [Header("UI - HUD")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthFill;
    [SerializeField] private TMP_Text timerText;

    [Header("Refs")]
    [SerializeField] private SurvivalGoalManager goal; // drag GameManager component here

    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;      // assign PausePanel
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private bool pauseWithEscape = true;

    private int _hp, _maxHp;
    private bool _paused;

    private void OnEnable()
    {
        GameEvents.OnHealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnHealthChanged -= OnHealthChanged;
    }

    private void Start()
    {
        SetPaused(false);
       
    }

    private void Update()
    {
        // Timer
        if (goal && timerText)
        {
            float t = goal.Remaining;
            int sec = Mathf.CeilToInt(t);
            timerText.text = $"TIME: {sec}s";
        }

        // Pause toggle
        if (pauseWithEscape && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void OnHealthChanged(int hp, int maxHp)
    {
        _hp = hp; _maxHp = maxHp;

        if (healthText)
            healthText.text = $"HP: {_hp}/{_maxHp}";

        if (healthFill)
            healthFill.fillAmount = (_maxHp <= 0) ? 0f : (float)_hp / _maxHp;
    }

    // ---------------- Pause API (hook to buttons) ----------------

    public void TogglePause()
    {
        SetPaused(!_paused);
    }

    public void Resume()
    {
        SetPaused(false);
    }

    public void GoToMainMenu()
    {
        // unpause before loading
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void SetPaused(bool on)
    {
        _paused = on;

        if (pausePanel)
        {
         
            pausePanel.SetActive(on);
        }

        Time.timeScale = on ? 0f : 1f;
    }

   
}
