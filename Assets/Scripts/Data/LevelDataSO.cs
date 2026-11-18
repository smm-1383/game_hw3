using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ChickenShooter/Level Data")]
public class LevelDataSO : ScriptableObject
{
    public int levelNumber;
    public string sceneName;
    public string displayName;
    public AudioClip music;
    public ThemeData theme;
    public LevelDataSO nextLevel;
    public RoundEntry[] rounds;
}
