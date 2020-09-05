using UnityEngine;

namespace TwoSimpleDevs.Project.Core
{
  public class Log
  {
    public static LogLevel Level { get; set; }

    public static void Debug(string message)
    {
      if (LogLevel.Debug <= Level)
      {
        Debug.Log(message);
      }
    }

    public static void Info(string message)
    {
      if (LogLevel.Info <= Level)
      {
        Debug.Log(message);
      }
    }

    public static void Warning(string message)
    {
      if (LogLevel.Warning <= Level)
      {
        Debug.LogWarning(message);
      }
    }

    public static void Error(string message)
    {
      if (LogLevel.Error <= Level)
      {
        Debug.LogError(message);
      }
    }
  }
}
