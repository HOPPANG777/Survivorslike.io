using UnityEngine;
using Sirenix.OdinInspector;

public class SerializedSingleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                //Debug.Log("singleton Manager has been created " + _instance.name);
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
            //Debug.Log("singleton Manager has been created " + _instance.name);
        }
        else
            Destroy(gameObject);
    }


    private void OnDestroy()
    {
        //Debug.Log(_instance.name + " ÆÄ±«µÊ");
    }

}
