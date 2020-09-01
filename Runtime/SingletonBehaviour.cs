﻿using UnityEngine;
 
namespace TwoSimpleDevs.Project.Core
{
  public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
  {
    private static T _instance;
    private static object _lock = new object();
    private static bool applicationIsQuitting = false;
  
    public static T Instance
    {
      get
      {
        if (applicationIsQuitting) 
        {
          Log.Warning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
          return null;
        }
  
        lock(_lock)
        {
          if (_instance == null)
          {
            _instance = (T)FindObjectOfType(typeof(T));
          }
  
          return _instance;
        }
      }
    }

    public virtual void Awake() 
    {
      DontDestroyOnLoad(this.gameObject);
    }

    public void OnDestroy()
    {
      applicationIsQuitting = true;
    }
  }
}
