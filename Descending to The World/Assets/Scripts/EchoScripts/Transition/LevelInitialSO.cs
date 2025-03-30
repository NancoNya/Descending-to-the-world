using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInitialSO", menuName = "ScriptableObject/LevelInitialSO")]
public class LevelInitialSO : ScriptableObject
{
    /// <summary>
    /// �������С�����ĳ�ʼλ��
    /// </summary>
    public Vector3[] playerPositions;

    ///// <summary>
    ///// ÿ����ؿ���С�ؿ�����
    ///// </summary>
    //public  int[] smallLevelCountsPerBigLevel;
}
