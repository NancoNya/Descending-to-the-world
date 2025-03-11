using UnityEngine;

//泛型实现类型参数化，达到代码重用的目的
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //T起到占位符的作用，相当于模板。只有使用Singleton这个泛型类的时候，才能根据自己的需要确定T的类型
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
            //里式转换原则
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
