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
      get
      { 
        return new NotificationInfo()
        {
          _data = new Dictionary<string, object>()
        };
      }
    }

    public NotificationInfo Set(string key, object value)
    {
      _data.Add(key, value);
      return this;
    }

    public object Get(string key)
    {
      return _data[key];
    }
  }
}