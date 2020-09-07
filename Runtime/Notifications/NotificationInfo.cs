using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwoSimpleDevs.Project.Core
{
  [Serializable]
  public class NotificationInfo
  {
    [SerializeField]
    private Dictionary<string, object> _data;

    public static NotificationInfo New
    {
      get { return new NotificationInfo(); }
    }
    
    public NotificationInfo()
    {
      _data = new Dictionary<string, object>();
    }

    public void Set(string key, object value)
    {
      _data.Add(key, value);
    }

    public object Get(string key)
    {
      return _data[key];
    }
  }
}