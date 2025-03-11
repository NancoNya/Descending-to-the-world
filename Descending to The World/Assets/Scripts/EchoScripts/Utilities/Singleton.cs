using UnityEngine;

//����ʵ�����Ͳ��������ﵽ�������õ�Ŀ��
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //T��ռλ�������ã��൱��ģ�塣ֻ��ʹ��Singleton����������ʱ�򣬲��ܸ����Լ�����Ҫȷ��T������
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            //��ʽת��ԭ��
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(gameObject);
            }
        }
    }
}
