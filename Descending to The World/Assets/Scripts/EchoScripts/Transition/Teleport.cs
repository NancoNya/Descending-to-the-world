using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public string sceneFrom;
    public string sceneToGo;
    public Vector3 targetPosition; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            TransitionManager.Instance.Transition(sceneFrom, sceneToGo, collision.gameObject.transform, targetPosition);
        }
    }
}
