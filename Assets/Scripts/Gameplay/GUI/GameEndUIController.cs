using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndUIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    [Header("Player Die Animation (Lose only)")]
    [SerializeField] private Animator playerAnimator;      // drag Player Visual Animator
    [SerializeField] private string dieTrigger = "Die";    // animator trigger name
    [SerializeField] private float showPanelDelay = 0.4f;  // unscaled seconds

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool _ended;

    private void Start()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnGameLose += OnLose;
        GameEvents.OnGameWin += OnWin;
    }

    private void OnDisable()
    {
        GameEvents.OnGameLose -= OnLose;
        GameEvents.OnGameWin -= OnWin;
    }

    private void OnLose()
    {
        if (_ended) return;
        _ended = true;

        // Freeze gameplay
        Time.timeScale = 0f;

        // Play die animation even when timescale is 0
        if (playerAnimator)
        {
            playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            playerAnimator.ResetTrigger(dieTrigger);
            playerAnimator.SetTrigger(dieTrigger);
        }

        StartCoroutine(ShowPanelRoutine(isWin: false));
    }

    private void OnWin()
    {
        if (_ended) return;
        _ended = true;

        // Freeze gameplay
        Time.timeScale = 0f;

        StartCoroutine(ShowPanelRoutine(isWin: true));
    }

    private System.Collections.IEnumerator ShowPanelRoutine(bool isWin)
    {
        // wait using unscaled time (since timescale = 0)
        float t = 0f;
        while (t < showPanelDelay)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        if (isWin)
        {
            if (winPanel) winPanel.SetActive(true);
        }
        else
        {
            if (gameOverPanel) gameOverPanel.SetActive(true);
        }
    
    }

    // ---------------- UI Buttons ----------------

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

   
}
