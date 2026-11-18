using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    private void OnEnable()
    {
        GameEvents.GameOver += Show;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= Show;
    }

    private void Start()
    {
        panel.SetActive(false);
        retryButton.onClick.AddListener(Retry);
        menuButton.onClick.AddListener(GoToMenu);
    }

    private void Show()
    {
        panel.SetActive(true);
    }

    private void Retry()
    {
        GameManager.Instance.Retry();
    }

    private void GoToMenu()
    {
        GameManager.Instance.GoToMenu();
    }
}
