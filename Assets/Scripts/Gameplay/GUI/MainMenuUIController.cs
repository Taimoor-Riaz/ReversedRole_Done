using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject rulesPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject quitPanel;

    [Header("Play Settings")]
    [Tooltip("If set, loads this scene name. Otherwise loads buildIndex + 1.")]
    [SerializeField] private string gameplaySceneName = "";
    [SerializeField] private int gameplaySceneBuildIndex = -1;

    private void Start()
    {
        CloseAllPanels();
    }

    // ---------------- Buttons (hook these in Inspector) ----------------

    public void OnPlayClicked()
    {
        CloseAllPanels();

        if (!string.IsNullOrWhiteSpace(gameplaySceneName))
        {
            SceneManager.LoadScene(gameplaySceneName);
            return;
        }

        if (gameplaySceneBuildIndex >= 0)
        {
            SceneManager.LoadScene(gameplaySceneBuildIndex);
            return;
        }

        // fallback: next scene in build settings
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(next);
    }

    public void OnRulesClicked()
    {
        OpenPanel(rulesPanel);
    }

    public void OnInfoClicked()
    {
        OpenPanel(infoPanel);
    }

    public void OnQuitClicked()
    {
        OpenPanel(quitPanel); // show confirm panel
    }

    // X buttons for panels
    public void OnClosePanelClicked()
    {
        CloseAllPanels();
    }

    // Quit confirm buttons (optional)
    public void OnQuitYesClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnQuitNoClicked()
    {
        CloseAllPanels();
    }

    // ---------------- Internal helpers ----------------

    private void OpenPanel(GameObject panel)
    {
        CloseAllPanels();
        if (panel) panel.SetActive(true);
    }

    private void CloseAllPanels()
    {
        if (rulesPanel) rulesPanel.SetActive(false);
        if (infoPanel) infoPanel.SetActive(false);
        if (quitPanel) quitPanel.SetActive(false);
    }
}
