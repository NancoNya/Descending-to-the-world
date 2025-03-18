using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public Vector3[] smallLevelStartPositions;
    public GameObject[] smallLevelPlatforms;
}