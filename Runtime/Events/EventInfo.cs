using System;
using System.Collections.Generic;
using UnityEngine;

namespace TSDevs
{
  [Serializable]
  public class EventInfo
  {
    [SerializeField]
    private Dictionary<string, object> _data;

    public static EventInfo New
    {
      get
      { 
        return new EventInfo()
        {
          _data = new Dictionary<string, object>()
        };
      }
    }

    public EventInfo Set(string key, object value)
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