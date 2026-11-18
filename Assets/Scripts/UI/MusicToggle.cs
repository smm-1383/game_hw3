using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text label;

    private void Start()
    {
        button.onClick.AddListener(Toggle);
        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void Toggle()
    {
        AudioManager.Instance.MusicOn = !AudioManager.Instance.MusicOn;
        Refresh();
    }

    private void Refresh()
    {
        if (AudioManager.Instance == null)
        {
            return;
        }
        label.text = AudioManager.Instance.MusicOn ? "Music: On" : "Music: Off";
    }
}
