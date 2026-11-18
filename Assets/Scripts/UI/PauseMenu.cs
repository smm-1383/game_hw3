using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    private void OnEnable()
    {
        GameEvents.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        GameEvents.StateChanged -= OnStateChanged;
    }

    private void Start()
    {
        panel.SetActive(false);
        resumeButton.onClick.AddListener(Resume);
        menuButton.onClick.AddListener(GoToMenu);
    }

    private void OnStateChanged(GameState state)
    {
        panel.SetActive(state == GameState.Paused);
    }

    private void Resume()
    {
        GameManager.Instance.Resume();
    }

    private void GoToMenu()
    {
        GameManager.Instance.GoToMenu();
    }
}
