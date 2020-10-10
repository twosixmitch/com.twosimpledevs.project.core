using System;
using UnityEngine;

namespace TwoSimpleDevs.Project.Core
{
  public class SettingsService : ServiceSingletonBase<SettingsService>
  {
    public override string DataPath
    {
      get {  return Application.persistentDataPath + "/settings.dat"; }
    }

    public float SFXVolume
    {
      get { return _state.SFXVolume; }
    }

    public float MusicVolume
    {
      get { return _state.MusicVolume; }
    }

    private SettingsState _state;

    public override void InitFromData(object data)
    {
      if (data == null)
      {
        _state = new SettingsState();
        return;
      }

      _state = (SettingsState)data;
    }

    public override object GetDataToSave()
    {
      return _state;
    }
  }
}