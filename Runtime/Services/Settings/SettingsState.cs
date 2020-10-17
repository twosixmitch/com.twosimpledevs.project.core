using System;
using UnityEngine;

namespace TwoSimpleDevs.Project.Core
{
  [Serializable]
  public partial class SettingsState
  {
    [SerializeField]
    private float _sfxVolume;

    [SerializeField]
    private float _musicVolume;

    public float SFXVolume
    { 
      get { return _sfxVolume; } 
      set { _sfxVolume = value; } 
    }

    public float MusicVolume
    { 
      get { return _musicVolume; } 
      set { _musicVolume = value; } 
    }

    public SettingsState()
    {
      _sfxVolume = 1.0f;
      _musicVolume = 1.0f;
    }
  }
}