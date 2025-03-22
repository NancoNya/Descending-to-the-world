using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassScript : MonoBehaviour
{
    /// <summary>
    /// ˾����������ײ
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllerScript player = other.gameObject.GetComponent<PlayerControllerScript>();
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
