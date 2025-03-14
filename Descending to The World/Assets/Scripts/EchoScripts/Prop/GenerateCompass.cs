using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ڳ�����ָ��λ������˾��
/// </summary>
public class GenerateCompass : MonoBehaviour
{
    [Header("˾�Ϸ���")]
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
