using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragThings : MonoBehaviour
{
    private Vector2 startPos;
    [SerializeField] private Transform onFloorPosition;
    [SerializeField] private bool onFloor;//判断是否已经在地面上

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDrag()
    {
        transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    }
}
