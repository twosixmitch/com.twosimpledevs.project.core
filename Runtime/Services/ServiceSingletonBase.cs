using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace TwoSimpleDevs.Project.Core
{
  public abstract class ServiceSingletonBase<T> : SingletonBehaviour<T> where T : MonoBehaviour
  {
    public abstract string DataPath { get; }

    public abstract void InitFromData(object data);
    public abstract object GetDataToSave();

    public virtual void Load()
    {
      var path = DataPath;
      
      Log.Debug($"[Service] Attempting to Load from {path}");

      if (File.Exists(path)) 
      {
        Log.Debug($"[Service] Found existing data at {path}"); 

        try
        {
          var formatter = new BinaryFormatter();
          var file = File.Open(path, FileMode.Open);

          InitFromData(formatter.Deserialize(file));
          
          file.Close();
        }
        catch (Exception e)
        {
          Log.Error($"[Service] Failed to load due to: {e.Message}");
        }              
      }
      else
      {
        Log.Debug($"[Service] Data does not exist at {path}");
      }
    }

    public virtual void Save()
    {
      try
      {
        var path = DataPath;
        var file = File.Create(path);

        var formatter = new BinaryFormatter();

        Log.Debug($"[Service] Saving to {path}");

        formatter.Serialize(file, GetDataToSave());

        file.Close();
      }
      catch (Exception e)
      {
        Log.Error($"[Service] Failed to save due to: {e.Message}");
      }
    }
  }
}