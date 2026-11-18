using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text missileText;
    [SerializeField] private TMP_Text powerText;
    [SerializeField] private Image[] lifeIcons;

    private void OnEnable()
    {
        GameEvents.RoundStarted += ShowRound;
        GameEvents.ScoreChanged += ShowScore;
        GameEvents.LivesChanged += ShowLives;
        GameEvents.MissilesChanged += ShowMissiles;
        GameEvents.PowerChanged += ShowPower;
    }

    private void OnDisable()
    {
        GameEvents.RoundStarted -= ShowRound;
        GameEvents.ScoreChanged -= ShowScore;
        GameEvents.LivesChanged -= ShowLives;
        GameEvents.MissilesChanged -= ShowMissiles;
        GameEvents.PowerChanged -= ShowPower;
    }

    private void ShowRound(int level, int round, int total)
    {
        roundText.text = "Level " + level + " - Round " + round;
    }

    private void ShowScore(int score)
    {
        scoreText.text = "Score " + score;
    }

    private void ShowLives(int lives)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = i < lives;
        }
    }

    private void ShowMissiles(int missiles)
    {
        missileText.text = "Missiles " + missiles;
    }

    private void ShowPower(int power)
    {
        powerText.text = "Power " + power;
    }
}
