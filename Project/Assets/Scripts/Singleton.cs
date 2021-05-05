using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<T>();
                if (!_instance)
                {
                    GameObject newObj = new GameObject($"{typeof(T).Name}");
                    _instance = newObj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    private static T _instance;

    protected virtual void Awake()
    {
        if (!_instance)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(this);
        }
    }
}
