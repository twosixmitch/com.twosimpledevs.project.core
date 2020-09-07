using System;
using UnityEngine;

namespace TwoSimpleDevs.Project.Core
{
  [Serializable]
  public partial class UserState
  {
    [SerializeField]
    private string _id;

    public string Id
    { 
      get { return _id; } 
      set { _id = value; } 
    }

    public UserState()
    {
    }
  }
}