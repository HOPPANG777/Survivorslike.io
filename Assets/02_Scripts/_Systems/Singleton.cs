using UnityEngine;


//==============================================================================
// Singleton
//==============================================================================
// 제네릭 문법을 이용해서 만든 MonoBehaviour 싱글톤 
// 싱글톤이 필요한 객체들은 해당 클래스를 상속받으면 손쉽게 
// 싱글톤으로 만들 수 있다.
//==============================================================================

// 오브젝트에 해당 컴포넌트를 복수개 붙일 수 없게 하는 어트리뷰트
[DisallowMultipleComponent]  // 중복 컴포넌트 추가 방지
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                Debug.Log("singleton Manager has been created " + _instance.name);
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
            Debug.Log("singleton Manager has been created " + _instance.name);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log(_instance.name + " 파괴됨");
    }

}

