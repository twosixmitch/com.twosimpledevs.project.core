using UnityEngine;
using System;

namespace TwoSimpleDevs.Project.Core
{
  public class UserService : ServiceSingletonBase<UserService>
  {
    public override string DataPath
    {
      get {  return Application.persistentDataPath + "/user_service.dat"; }
    }

    public static string UserId
    {
      get { return Instance._state.Id; }
    }

    private UserState _state;

    public override void InitFromData(object data)
    {
      if (data == null)
      {
        return;
      }

      _state = (UserState)data;

      if (string.IsNullOrEmpty(_state.Id))
      {
        _state.Id = Guid.NewGuid().ToString().Replace("-", string.Empty);
      }
    }

    public override object GetDataToSave()
    {
      return _state;
    }
  }
}