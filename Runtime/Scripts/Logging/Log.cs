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
      if (CanLog(LogLevel.Debug))
      {
        UnityEngine.Debug.Log(message);
      }
    }

    public static void Info(string message)
    {
      if (CanLog(LogLevel.Info))
      {
        UnityEngine.Debug.Log(message);
      }
    }

    public static void Warning(string message)
    {
      if (CanLog(LogLevel.Warning))
      {
        UnityEngine.Debug.LogWarning(message);
      }
    }

    public static void Error(string message)
    {
      if (CanLog(LogLevel.Error))
      {
        UnityEngine.Debug.LogError(message);
      }
    }

    private static bool CanLog(LogLevel level)
    {
      if (Level == LogLevel.None)
      {
        return false;
      }

      return Level <= level ? true : false;
    }
  }
}
