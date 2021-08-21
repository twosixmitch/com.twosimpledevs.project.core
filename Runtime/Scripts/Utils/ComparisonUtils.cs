using System;

namespace TSDevs
{
  public static partial class Utils
  {
    public static bool Compare<T>(T first, T second, ComparisonType comparisonType) where T : IComparable
    {
      switch (comparisonType)
      {
        case ComparisonType.Equal:                return first.CompareTo(second) == 0;
        case ComparisonType.NotEqual:             return first.CompareTo(second) != 0;
        case ComparisonType.LessThan:             return first.CompareTo(second) < 0;
        case ComparisonType.LessThanOrEqualTo:    return first.CompareTo(second) < 0 || first.CompareTo(second) == 0;
        case ComparisonType.Greater:              return first.CompareTo(second) > 0;
        case ComparisonType.GreaterThanOrEqualTo: return first.CompareTo(second) > 0 || first.CompareTo(second) == 0;
      }

      return false;
    }
  }
}