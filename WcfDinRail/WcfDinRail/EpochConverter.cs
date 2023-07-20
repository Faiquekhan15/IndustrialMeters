using System;

namespace WcfDinRail
{
  public class EpochConverter
  {
    public long ToUnixTime(DateTime date)
    {
      DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      return Convert.ToInt64((date - dateTime).TotalSeconds);
    }
  }
}
