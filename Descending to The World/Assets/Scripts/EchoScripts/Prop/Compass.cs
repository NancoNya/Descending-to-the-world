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
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.PickUpCompass();
                Destroy(gameObject);
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        PickupCompass player = other.gameObject.GetComponent<PickupCompass>();
    //        if (player != null)
    //        {
    //            player.PickUpCompass();
    //            Destroy(gameObject);
    //            Debug.Log("destroy");
    //        }
    //    }
    //}
}
