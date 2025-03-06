using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropColumnAnim : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    public float animationSpeed;
    public GameObject panel;
    public bool isStart;
    public bool isStop;

    IEnumerator ShowPanel(GameObject gameObject)
    {
        float timer = 0;
        while (timer <= 1)
        {
            gameObject.transform.localScale = Vector3.one*showCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
    }

    IEnumerator HidePanel(GameObject gameObject)
    {
        float timer = 0;
        while (timer <= 1)
        {
            gameObject.transform.localScale = Vector3.one * hideCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
    }
    void Start()
    {
        isStart = false;
        isStop = true; 
    }

    void Update()
    {
        if (isStop && !isStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(ShowPanel(panel));
                isStart = true;
                isStop = false;
            }
        }
        else if (!isStop && isStart)
        {
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(HidePanel(panel));
                isStop = true;
                isStart = false;
            }
        }
    }
}
