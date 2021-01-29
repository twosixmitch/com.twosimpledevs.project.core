using UnityEngine;

namespace TSDevs
{
  public class SettingsService : ServiceSingletonBase<SettingsService>
  {
    public override string DataPath
    {
      get {  return Application.persistentDataPath + "/settings_service.dat"; }
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

    public override void Initialize(object data)
    {
      if (data == null)
      {
        _state = new SettingsState();
        return;
      }

      _state = (SettingsState)data;
    }

    public override object Serialize()
    {
      return _state;
    }
  }
}