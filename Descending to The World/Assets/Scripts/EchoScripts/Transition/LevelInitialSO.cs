using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInitialSO", menuName = "Level Data/Initial Positions")]
public class LevelInitialSO : ScriptableObject
{
    /// <summary>
    /// �������С�����ĳ�ʼλ��
    /// </summary>
    public Vector3[] playerPositions;
}
