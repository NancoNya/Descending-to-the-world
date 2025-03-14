using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    /// <summary>
    /// ˾����������ײ
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PickupCompass player = other.gameObject.GetComponent<PickupCompass>();
            if (player != null)
            {
                player.PickUpCompass();
                Destroy(gameObject);
            }
        }
    }
}
