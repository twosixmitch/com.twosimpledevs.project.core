namespace TSDevs
{
  public class Log
  {
    #if UNITY_EDITOR
    public static LogLevel Level { get; set; } = LogLevel.Debug;
    #else
    public static LogLevel Level { get; set; } = LogLevel.Warning;
    #endif

    public static void Debug(string message)
    {
      if (LogLevel.Debug <= Level)
      {
        UnityEngine.Debug.Log(message);
      }
    }

    public static void Info(string message)
    {
      if (LogLevel.Info <= Level)
      {
        UnityEngine.Debug.Log(message);
      }
    }

    public static void Warning(string message)
    {
      if (LogLevel.Warning <= Level)
      {
        UnityEngine.Debug.LogWarning(message);
      }
    }

    public static void Error(string message)
    {
      if (LogLevel.Error <= Level)
      {
        UnityEngine.Debug.LogError(message);
      }
    }
  }
}
