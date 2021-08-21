using System;

namespace TSDevs
{
  public static partial class Utils
  {
    public static int NumberOfIntervals(DateTime oldDate, DateTime newDate, TimeIntervalType interval)
    {
      var span = newDate - oldDate;

      switch (interval)
      {
        case TimeIntervalType.Second: return span.Seconds;
        case TimeIntervalType.Minute: return span.Minutes;
        case TimeIntervalType.Hour:   return span.Hours;
        case TimeIntervalType.Day:    return span.Days;
        default: return span.Days;
      }
    }

    /// <summary>
    /// Does not take into account the time of the day.
    /// So it purely counts if it is a different day and not 24hours having passed.
    /// </summary>
    public static int NumberOfDaysBetweenDates(DateTime oldDate, DateTime newDate)
    {
      var span = newDate.Date - oldDate.Date;
      return span.Days;
    }
  }
}