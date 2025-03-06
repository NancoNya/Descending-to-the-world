using UnityEngine;

public class Drag2DSprite : MonoBehaviour
{
    [SerializeField] private bool isSelected;
        private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            isSelected = true;
        if (Input.GetMouseButtonUp(0))
            isSelected = false;
    }
    public bool isValidFloor;
    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * 0.07f;
    }

    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * 0.07f;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 检测是否碰撞到tag为normalFloor的对象
        if (collision.gameObject.CompareTag("NormalFloor"))
        {
            isValidFloor = true;
            gameObject.SetActive(false);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // 当离开碰撞时，重置状态
        if (collision.gameObject.CompareTag("NormalFloor"))
        {
            isValidFloor = false;
            gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isValidFloor=false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.mousePosition.x+","+Input.mousePosition.y);
        if (isSelected)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(cursorPos.x, cursorPos.y);
        }
    }
}
