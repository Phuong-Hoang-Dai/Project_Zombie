using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance {  get; private set; }
    protected virtual void Awake()
    {
        if (Instance != null) Debug.LogWarning("Should have only 1 instance of a singleton class at a time.");
        Instance = this as T;
    }
}
