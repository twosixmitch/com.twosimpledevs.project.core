using UnityEngine;
using System;

namespace TSDevs
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

    public override void Initialize(object data)
    {
      if (data == null)
      {
        _state = new UserState() { Id = Guid.NewGuid().ToString().Replace("-", string.Empty) };
        return;
      }

      _state = (UserState)data;
    }

    public override object Serialize()
    {
      return _state;
    }
  }
}