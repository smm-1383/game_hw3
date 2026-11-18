using System.Collections;
using TMPro;
using UnityEngine;

public class LevelBanner : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private float showTime = 1.5f;

    private void OnEnable()
    {
        GameEvents.RoundStarted += ShowRound;
        GameEvents.LevelCompleted += ShowLevelComplete;
    }

    private void OnDisable()
    {
        GameEvents.RoundStarted -= ShowRound;
        GameEvents.LevelCompleted -= ShowLevelComplete;
    }

    private void Start()
    {
        label.gameObject.SetActive(false);
    }

    private void ShowRound(int level, int round, int total)
    {
        Show("Round " + round);
    }

    private void ShowLevelComplete(int level)
    {
        Show("Level " + level + " complete");
    }

    private void Show(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        label.text = message;
        label.gameObject.SetActive(true);
        yield return new WaitForSeconds(showTime);
        label.gameObject.SetActive(false);
    }
}
