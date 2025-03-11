using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MilkyWayTrigger : MonoBehaviour
{
    public GameObject Moon;
    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(Moon.activeSelf) gameObject.SetActive(true);
        else gameObject.SetActive(false);
        
    }
}
