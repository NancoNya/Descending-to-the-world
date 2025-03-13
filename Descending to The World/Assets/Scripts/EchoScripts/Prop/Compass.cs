using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    private Animator playerAnimator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("IsWalking", false);
                playerAnimator.SetBool("IsHoldingCompass", true);
            }
        }
    }
}
