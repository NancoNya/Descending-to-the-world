using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
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
    //private static T instance;
    //public static T Instance
    //{
    //    get
    //    {
    //        return instance;
    //    }
    //}

    //protected virtual void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = FindObjectOfType<T>();
    //        if (instance == null)
    //        {
    //            GameObject singletonObject = new GameObject(typeof(T).Name);
    //            instance = singletonObject.AddComponent<T>();
    //        }
    //        DontDestroyOnLoad(instance.gameObject);
    //    }
    //    else
    //    {
    //        if (this != instance)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}
}
