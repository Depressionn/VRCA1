using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper {

}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T m_instance;

    public static T Instance {
        get {
            if (!m_instance) {
                m_instance = FindObjectOfType<T>();
                if(!m_instance) m_instance = new GameObject(typeof(T).ToString().ToUpper()).AddComponent<T>();
            }
            return m_instance;
        }
    }
}
