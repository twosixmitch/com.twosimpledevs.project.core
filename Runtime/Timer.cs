using System;

namespace TwoSimpleDevs.Project.Core
{
  public class Timer
  {
    private DateTime _startTime;
    private DateTime _endTime;
    private double _maxDuration; // Seconds

    public Timer()
    {
      _maxDuration = 0;
      _startTime = DateTime.MinValue;
      _endTime = DateTime.MinValue;
    }

    public void Set(double duration)
    {
      _maxDuration = duration;
    }

    public void Start(double duration = 0)
    {
      _maxDuration = duration;
      _startTime = DateTime.UtcNow;
    }

    public void Stop()
    {
      _endTime = DateTime.UtcNow;
    }

    public void Reset()
    {
      _startTime = DateTime.MinValue;
      _endTime = DateTime.MinValue;
      _maxDuration = 0;
    }

    public bool IsFinished()
    {
      return _maxDuration != 0 ? Seconds >= _maxDuration : false;
    }

    public double Seconds
    {
      get
      {
        TimeSpan span = TimeSpan.Zero;

        if (_endTime != DateTime.MinValue)
        {
          span = _endTime - _startTime;
        }
        else
        {
          span = DateTime.UtcNow - _startTime;
        }

        return span.TotalSeconds;
      }
    }
  }
}