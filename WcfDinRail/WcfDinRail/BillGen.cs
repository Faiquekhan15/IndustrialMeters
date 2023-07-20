
using System;

namespace WcfDinRail
{
  public class BillGen
  {
    public Decimal total_kwh_consumed { get; set; }

    public Decimal total_kwh_peak { get; set; }

    public Decimal total_kwh_offpeak { get; set; }

    public Decimal curr_kwh_reading_total { get; set; }

    public Decimal curr_kwh_reading_t1 { get; set; }

    public Decimal curr_kwh_reading_t2 { get; set; }

    public Decimal last_kwh_reading_total { get; set; }

    public Decimal last_kwh_reading_t1 { get; set; }

    public Decimal last_kwh_reading_t2 { get; set; }

    public string customer_info { get; set; }
  }
}
