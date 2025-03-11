using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SundialScript : MonoBehaviour
{
    public int Sun_Moon;
    public bool isInvincible;
    public float invincibleTimer;
    public float invincibleTime = 2.0f;
    public GameObject Moon;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (isInvincible) return;
        if (!isInvincible)
        {
            //if (collision.gameObject.CompareTag("Player"))
            //{
                
                isInvincible = true;
                Sun_Moon++;
            
            //}

        }

    }
    void Start()
    {
        Sun_Moon = 0;
        isInvincible = false;
        Moon.SetActive (false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer <= 0)
            {
                isInvincible = false;
                invincibleTimer = invincibleTime;
            }
        }else invincibleTimer = invincibleTime;
        Sun_Moon %= 2;
        if (Sun_Moon == 1) Moon.SetActive(true); else Moon.SetActive(false);
    }
}
