using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        continueButton.interactable = SaveSystem.Exists();
        newGameButton.onClick.AddListener(NewGame);
        continueButton.onClick.AddListener(Continue);
        quitButton.onClick.AddListener(Quit);
    }

    private void NewGame()
    {
        GameManager.Instance.NewGame();
    }

    private void Continue()
    {
        GameManager.Instance.Continue();
    }

    private void Quit()
    {
        Application.Quit();
    }
}
