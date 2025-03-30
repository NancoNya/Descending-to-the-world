using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInitialSO", menuName = "ScriptableObject/LevelInitialSO")]
public class LevelInitialSO : ScriptableObject
{
    /// <summary>
    /// 人物进入小场景的初始位置
    /// </summary>
    public Vector3[] playerPositions;

    ///// <summary>
    ///// 每个大关卡的小关卡数量
    ///// </summary>
    //public  int[] smallLevelCountsPerBigLevel;
}
