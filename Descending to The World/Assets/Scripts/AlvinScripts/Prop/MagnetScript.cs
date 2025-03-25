using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScript : MonoBehaviour
{
    public GameObject gameObjectSelf;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isHoldingCompass = collision.gameObject.GetComponent<PlayerControllerScript>().holdCompass;

        if (collision.gameObject.CompareTag("Player") && isHoldingCompass)
        {
            gameObjectSelf.SetActive(false);
        }
    }
 
}
