using System;

namespace TwoSimpleDevs.Project.Core
{
  public class DateProvider : SingletonBehaviour<DateProvider>
  {
    private TimeSpan _offset = TimeSpan.Zero;

    public DateTime LocalNow 
    {
      get { return DateTime.Now + _offset; }
    }

    public DateTime UtcNow 
    {
      get { return DateTime.UtcNow + _offset; }
    }

    public void IncreaseCurrentTime(TimeIntervalType interval, int amount)
    {
      switch (interval)
      {
        case TimeIntervalType.Second: _offset += TimeSpan.FromSeconds(amount); break;
        case TimeIntervalType.Minute: _offset += TimeSpan.FromMinutes(amount); break;
        case TimeIntervalType.Hour:   _offset += TimeSpan.FromHours(amount); break;
        case TimeIntervalType.Day:    _offset += TimeSpan.FromDays(amount); break;
        default: break;
      }
    }
  }
}