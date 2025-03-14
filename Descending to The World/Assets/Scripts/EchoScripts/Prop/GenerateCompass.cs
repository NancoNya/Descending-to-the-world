using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在场景中指定位置生成司南
/// </summary>
public class GenerateCompass : MonoBehaviour
{
    [Header("司南放置")]
    public GameObject compassPrefab;
    public Vector3 compassPosition;

    private void Start()
    {
        Instantiate(compassPrefab, compassPosition, Quaternion.identity);
    }

    public void RespawnCompass()
    {
        Instantiate(compassPrefab, compassPosition, Quaternion.identity);
    }
}
