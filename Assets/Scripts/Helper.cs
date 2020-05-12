using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Helper {
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T m_instance;

    public T Instance {
        get {
            if (!m_instance) m_instance = new GameObject(typeof(T).ToString().ToUpper()).AddComponent<T>();
            return m_instance;
        }
    }
}
