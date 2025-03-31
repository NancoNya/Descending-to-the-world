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
                this.gameObject.SetActive(false);
            }
        }
    }
}
