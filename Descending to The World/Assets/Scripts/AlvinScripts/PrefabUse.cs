using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabUse : MonoBehaviour
{
    public GameObject prefab;


    // Start is called before the first frame update
    void Start()
    {
     if(prefab != null)
        {
            GameObject instantiatedPerfab = Instantiate(prefab);
            instantiatedPerfab.transform.SetParent(transform.parent,false);
            instantiatedPerfab.transform.position = transform.position;
            instantiatedPerfab.transform.localScale = transform.localScale;

            Destroy(gameObject);
        }   
    }

}
