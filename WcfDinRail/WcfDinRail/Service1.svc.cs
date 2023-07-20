using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;

namespace WcfDinRail
{
    public class Service1 : IService1
    {
        private void ExecuteNonQurey(string query) => new DBGetSet()
        {
            Query = query
        }.ExecuteNonQuery();

        private DataTable ExecuteReader(string query) => new DBGetSet()
        {
            Query = query
        }.ExecuteReader();


        public string MeterData()
        {
            try
            {
                string str = "[";
                DataTable dataTable = new DataTable();
                DBGetSet dbGetSet = new DBGetSet();
                dbGetSet.Query = "SELECT SUM(total_active_energy) AS TotalConsumption, AVG(combine_power_factor) AS TotalPowerFactor, SUM(CASE WHEN DATE_FORMAT(Time_stamp, '%Y-%m-%d') = CURDATE() THEN total_active_energy ELSE 0 END) AS DailyQuantity, SUM(total_active_energy) AS MonthlyQuantity FROM dingrail.meter_data; ";

                foreach (DataRow row in dbGetSet.ExecuteReader().Rows)
                {
                    str += new JavaScriptSerializer().Serialize(new
                    {
                        TotalConsumption = row["TotalConsumption"].ToString(),
                        TotalPowerFactor = row["TotalPowerFactor"].ToString(),
                        DailyQuantity = row["DailyQuantity"].ToString(),
                        MonthlyQuantity = row["MonthlyQuantity"].ToString()
                    });
                    str += ",";
                }
                return str.TrimEnd(',') + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public string[] GetData()
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DBGetSet()
            {
                Query = "select serial from meter where (serial = '3098000020' or serial = '3098000010') and deviceType = 'Three Phase' order by serial;"
            }.ExecuteReader();
            string[] data = new string[dataTable2.Rows.Count];
            int index = 0;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                data[index] = row["serial"].ToString();
                row["serial"].ToString();
                ++index;
            }
            return data;
        }

        public string[] GetDataMeters()
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DBGetSet()
            {
                Query = "select serial from meter where deviceType = 'Three Phase' order by serial;"
            }.ExecuteReader();
            string[] dataMeters = new string[dataTable2.Rows.Count];
            int index = 0;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                dataMeters[index] = row["serial"].ToString();
                row["serial"].ToString();
                ++index;
            }
            return dataMeters;
        }

        public string GetDataMetersWD(string username)
        {
            string str1 = "[";
            DataTable dataTable = new DataTable();
            foreach (DataRow row in (InternalDataCollectionBase)new DBGetSet()
            {
                Query = "select serial,connected,customerCode from meter where deviceType = 'Three Phase' order by serial;"
            }.ExecuteReader().Rows)
            {
                string str2 = "Connected";
                if (row["connected"].ToString() == "0")
                    str2 = "Disconnected";
                str1 += new JavaScriptSerializer().Serialize((object)new
                {
                    Serial = row["serial"],
                    Status = str2,
                    RefrenceNumber = row["customerCode"]
                });
                str1 += ",";
            }
            return str1.Substring(0, str1.Length - 1) + "]";
        }

        public string getUMeters(string username)
        {
            string str1 = "[";
            DataTable dataTable = new DataTable();
            foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select serial,connected from meter where deviceType = 'Three Phase' and referenceNo = 'N/A' order by serial;").Rows)
            {
                string str2 = "Connected";
                if (row["connected"].ToString() == "0")
                    str2 = "Disconnected";
                str1 += new JavaScriptSerializer().Serialize((object)new
                {
                    Serial = row["serial"],
                    Status = str2
                });
                str1 += ",";
            }
            return str1.Substring(0, str1.Length - 1) + "]";
        }

        public int ValidateUser(string auth)
        {
            string[] strArray = auth.Split(',');
            DataTable dataTable = new DataTable();
            return new DBGetSet()
            {
                Query = ("select * from users where username='" + strArray[0] + "' and password = '" + strArray[1] + "'")
            }.ExecuteReader().Rows.Count > 0 ? 1 : 0;
        }

        public string GetFDData()
        {
            string str = "[";
            DataTable dataTable = new DataTable();
            foreach (DataRow row in (InternalDataCollectionBase)new DBGetSet()
            {
                Query = "SELECT * FROM fdevices;"
            }.ExecuteReader().Rows)
            {
                str = !(DateTime.Now.Subtract((DateTime)row["lastcomm"]) < TimeSpan.Parse("00:010:20.9896330")) ? str + new JavaScriptSerializer().Serialize((object)new
                {
                    Connected = 0,
                    DeviceID = row["deviceID"].ToString(),
                    SignalSerength = row["rssi"].ToString(),
                    Tech = row["connection"].ToString(),
                    LastCommunication = row["lastcomm"].ToString()
                }) : str + new JavaScriptSerializer().Serialize((object)new
                {
                    Connected = 1,
                    DeviceID = row["deviceID"].ToString(),
                    SignalSerength = row["rssi"].ToString(),
                    Tech = row["connection"].ToString(),
                    LastCommunication = row["lastcomm"].ToString()
                });
                str += ",";
            }
            return str.Substring(0, str.Length - 1) + "]";
        }

        public string GetBillData(ulong serial)
        {
            int num = 0;
            string str = "[";
            DataTable dataTable = new DataTable();
            DBGetSet dbGetSet = new DBGetSet();
            dbGetSet.Query = "SELECT msn,mode FROM meter1 where serial='" + (object)serial + "';";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                num = (int)row["msn"];
            DateTime dateTime = new DateTime();
            dbGetSet.Query = "SELECT Time_stamp FROM info11 where quantity=0 and msn ='" + (object)num + "' order by Time_stamp desc limit 1;";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                dateTime = (DateTime)row["Time_stamp"];
            dbGetSet.Query = "SELECT * FROM info11 where msn='" + (object)num + "' and quantity = 0 and Time_stamp = '" + dateTime.ToString("yyyy/M/d HH:mm:ss") + "';";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
            {
                str += new JavaScriptSerializer().Serialize((object)new
                {
                    Serial = serial,
                    Obis = row["OBIS"].ToString(),
                    Description = this.OBISTranslation(row["OBIS"].ToString()),
                    Value = row["Amount"].ToString(),
                    Unit = row["unit"].ToString(),
                    Time = ((DateTime)row["Time_stamp"]).ToString()
                });
                str += ",";
            }
            return str.Substring(0, str.Length - 1) + "]";
        }

        public string GetIData(ulong serial)
        {
            int num = 0;
            string str1 = "[";
            string str2 = "";
            DataTable dataTable = new DataTable();
            DBGetSet dbGetSet = new DBGetSet();
            dbGetSet.Query = "SELECT * FROM meter1 where serial='" + (object)serial + "';";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
            {
                num = (int)row["msn"];
                str2 = !(row["connected"].ToString() == "1") ? "Offline" : "Online";
            }
            DateTime dateTime = new DateTime();
            dbGetSet.Query = "SELECT Time_stamp FROM info11 where quantity=1 and msn ='" + (object)num + "' order by Time_stamp desc limit 1;";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                dateTime = (DateTime)row["Time_stamp"];
            dbGetSet.Query = "SELECT * FROM info11 where msn='" + (object)num + "' and quantity = 1 and Time_stamp = '" + dateTime.ToString("yyyy/M/d HH:mm:ss") + "';";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
            {
                str1 += new JavaScriptSerializer().Serialize((object)new
                {
                    Serial = serial,
                    Obis = row["OBIS"].ToString(),
                    Description = this.OBISTranslation(row["OBIS"].ToString()),
                    Value = row["Amount"].ToString(),
                    Unit = row["unit"].ToString(),
                    Time = ((DateTime)row["Time_stamp"]).ToString(),
                    Status = str2
                });
                str1 += ",";
            }
            return str1.Substring(0, str1.Length - 1) + "]";
        }

        protected string OBISTranslation(string Val)
        {
            switch (Val)
            {
                case "1.0.15.8.0.255":
                    return "CumActiveEnergyAbsTotal";
                case "1.0.15.8.1.255":
                    return "CumActiveEnergyAbsTariff1";
                case "1.0.15.8.2.255":
                    return "CumActiveEnergyAbsTariff2";
                case "1.0.94.92.0.255":
                    return "CumReactiveEnergyAbsTotal";
                case "1.0.94.92.1.255":
                    return "CumReactiveEnergyAbsTariff1";
                case "1.0.94.92.2.255":
                    return "CumReactiveEnergyAbsTariff2";
                case "1.0.15.6.0.255":
                    return "MaximumDemandActiveAbsTotal";
                case "1.0.15.6.1.255":
                    return "MaximumDemandActiveAbsTariff1";
                case "1.0.15.6.2.255":
                    return "MaximumDemandActiveAbsTariff2";
                case "1.0.15.2.0.255":
                    return "CumMaximumDemandActiveAbsTotal";
                case "1.0.15.2.1.255":
                    return "CumMaximumDemandActiveAbsTariff1";
                case "1.0.15.2.2.255":
                    return "CumMaximumDemandActiveAbsTariff2";
                case "1.0.0.1.2.255":
                    return "MDIResetDateandTime";
                case "1.0.0.1.0.255":
                    return "MDIREsetCount";
                case "1.0.1.7.0.255":
                    return "AggregateActivepowerImport";
                case "1.0.2.7.0.255":
                    return "AggregateActivePowerExport";
                case "1.0.3.7.0.255":
                    return "AggregateReactivePowerImport";
                case "1.0.4.7.0.255":
                    return "AggregateReactivePowerexport";
                case "1.0.32.7.0.255":
                    return "VoltageA";
                case "1.0.52.7.0.255":
                    return "VoltageB";
                case "1.0.72.7.0.255":
                    return "VoltageC";
                case "1.0.31.7.0.255":
                    return "CurrentA";
                case "1.0.51.7.0.255":
                    return "CurrentB";
                case "1.0.71.7.0.255":
                    return "CurrentC";
                case "1.0.13.7.0.255":
                    return "AveragePowerFactor";
                case "1.0.14.7.0.255":
                    return "Frequency";
                case "1.0.0.9.1.255":
                    return "Time";
                case "1.0.0.9.2.255":
                    return "Date";
                case "0.0.96.14.0.255":
                    return "CurrentTariff";
                case "1.0.0.4.2.255":
                    return "CTRatioNumerator";
                case "1.0.0.4.5.255":
                    return "CTratioDenominator";
                case "1.0.0.4.3.255":
                    return "PTRatioNumerator";
                case "1.0.0.4.6.255":
                    return "PTratioDenominator";
                case "0.0.96.1.10.255":
                    return "CustomerCode";
                default:
                    return "";
            }
        }

        public string GetLP(ulong serial)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime dateTime1 = new DateTime(now.Year, now.Month, 1);
                DateTime dateTime2 = dateTime1.AddMonths(1).AddDays(-1.0);
                string str = "[";
                DataTable dataTable = new DataTable();
                foreach (DataRow row in (InternalDataCollectionBase)new DBGetSet()
                {
                    Query = ("SELECT * FROM loadprofile1 where serial='" + (object)serial + "' and Time_stamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp limit 9999;")
                }.ExecuteReader().Rows)
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {
                        Serial = serial,
                        kWh = float.Parse(row["kWh"].ToString()),
                        kW = float.Parse(row["kW"].ToString()),
                        kVARh = float.Parse(row["kVARh"].ToString()),
                        kVAR = float.Parse(row["kVAR"].ToString()),
                        Time = ((DateTime)row["Time_stamp"]).ToString()
                    });
                    str += ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetLPApp(ulong serial, int month)
        {
            try
            {
                DateTime dateTime1 = new DateTime(DateTime.Now.Year, month, 1);
                DateTime dateTime2 = dateTime1.AddMonths(1).AddDays(-1.0);
                string str = "[";
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = new DBGetSet()
                {
                    Query = ("SELECT * FROM loadprofile1 where serial='" + (object)serial + "' and Time_stamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp limit 9999;")
                }.ExecuteReader();
                if (dataTable2.Rows.Count < 1)
                    return "Error: No Data";
                int num1 = dataTable2.Rows.Count / 10;
                int num2 = 0;
                for (int index = 0; index < dataTable2.Rows.Count; index += num1)
                {
                    DataRow row = dataTable2.Rows[index];
                    str = str + new JavaScriptSerializer().Serialize((object)new
                    {
                        Serial = serial,
                        kWh = float.Parse(row["kWh"].ToString()),
                        kW = float.Parse(row["kW"].ToString()),
                        kVARh = float.Parse(row["kVARh"].ToString()),
                        kVAR = float.Parse(row["kVAR"].ToString()),
                        Time = ((DateTime)row["Time_stamp"]).ToString()
                    }) + ",";
                    num2 = index;
                }
                if (num2 != dataTable2.Rows.Count - 1)
                {
                    DataRow row = dataTable2.Rows[dataTable2.Rows.Count - 1];
                    str = str + new JavaScriptSerializer().Serialize((object)new
                    {
                        Serial = serial,
                        kWh = float.Parse(row["kWh"].ToString()),
                        kW = float.Parse(row["kW"].ToString()),
                        kVARh = float.Parse(row["kVARh"].ToString()),
                        kVAR = float.Parse(row["kVAR"].ToString()),
                        Time = ((DateTime)row["Time_stamp"]).ToString()
                    }) + ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetLPMeter(ulong serial, int month, int year)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime dateTime1 = new DateTime(year, month, 1);
                DateTime dateTime2 = dateTime1.AddMonths(1).AddDays(-1.0);
                string str = "[";
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = new DBGetSet()
                {
                    Query = ("SELECT * FROM loadprofile1 where serial='" + (object)serial + "' and Time_stamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp limit 9999;")
                }.ExecuteReader();
                if (dataTable2.Rows.Count < 1)
                    return "Error: No Data";
                int num1 = 1;
                int num2 = 0;
                EpochConverter epochConverter = new EpochConverter();
                for (int index = 0; index < dataTable2.Rows.Count; index += num1)
                {
                    DataRow row = dataTable2.Rows[index];
                    str = str + new JavaScriptSerializer().Serialize((object)new
                    {
                        Serial = serial,
                        kWh = float.Parse(row["kWh"].ToString()),
                        kW = float.Parse(row["kW"].ToString()),
                        kVARh = float.Parse(row["kVARh"].ToString()),
                        kVAR = float.Parse(row["kVAR"].ToString()),
                        Time = ((DateTime)row["Time_stamp"]).ToString(),
                        Date = epochConverter.ToUnixTime((DateTime)row["Time_stamp"])
                    }) + ",";
                    num2 = index;
                }
                if (num2 != dataTable2.Rows.Count - 1)
                {
                    DataRow row = dataTable2.Rows[dataTable2.Rows.Count - 1];
                    str = str + new JavaScriptSerializer().Serialize((object)new
                    {
                        Serial = serial,
                        kWh = float.Parse(row["kWh"].ToString()),
                        kW = float.Parse(row["kW"].ToString()),
                        kVARh = float.Parse(row["kVARh"].ToString()),
                        kVAR = float.Parse(row["kVAR"].ToString()),
                        Time = ((DateTime)row["Time_stamp"]).ToString()
                    }) + ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetLPMeter1(ulong serial, int month, int year)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime dateTime1 = new DateTime(year, month, 1);
                DateTime dateTime2 = dateTime1.AddMonths(1);
                string str = "[";
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = new DBGetSet()
                {
                    Query = ("SELECT * FROM loadprofile1 where serial='" + (object)serial + "' and Time_stamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp limit 9999;")
                }.ExecuteReader();
                if (dataTable2.Rows.Count < 1)
                    return "Error: No Data";
                int num1 = 1;
                int num2 = 0;
                for (int index = 0; index < dataTable2.Rows.Count; index += num1)
                {
                    DataRow row = dataTable2.Rows[index];
                    str = str + new JavaScriptSerializer().Serialize((object)new
                    {
                        Serial = serial,
                        kWh = float.Parse(row["kWh"].ToString()),
                        kW = float.Parse(row["kW"].ToString()),
                        kVARh = float.Parse(row["kVARh"].ToString()),
                        kVAR = float.Parse(row["kVAR"].ToString()),
                        Time = Convert.ToUInt64(((DateTime)row["Time_stamp"]).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds)
                    }) + ",";
                    num2 = index;
                }
                if (num2 != dataTable2.Rows.Count - 1)
                {
                    DataRow row = dataTable2.Rows[dataTable2.Rows.Count - 1];
                    str = str + new JavaScriptSerializer().Serialize((object)new
                    {
                        Serial = serial,
                        kWh = float.Parse(row["kWh"].ToString()),
                        kW = float.Parse(row["kW"].ToString()),
                        kVARh = float.Parse(row["kVARh"].ToString()),
                        kVAR = float.Parse(row["kVAR"].ToString()),
                        Time = Convert.ToUInt64(((DateTime)row["Time_stamp"]).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds)
                    }) + ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string[] GetProperties(ulong Serial)
        {
            DataTable dataTable1 = new DataTable();
            DBGetSet dbGetSet = new DBGetSet();
            dbGetSet.Query = "select * from meter where serial = " + (object)Serial + ";";
            DataTable dataTable2 = dbGetSet.ExecuteReader();
            string[] properties = new string[11];
            string str = "0";
            for (int index = 0; index < properties.Length; ++index)
                properties[index] = "Data N/A";
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                str = row["msn"].ToString();
                properties[9] = !(row["connected"].ToString() == "1") ? "Device Disconnected" : "Device Connected";
                properties[10] = !(row["relayStatus"].ToString() == "1") ? "Relay Disconnected" : "Relay Connected";
                properties[2] = "Last Reading:" + DateTime.Parse(row["lastRead"].ToString()).ToString("yyyy-M-dd HH:mm:ss");
                properties[3] = "Last Communication:" + DateTime.Parse(row["times"].ToString()).ToString("yyyy-M-dd HH:mm:ss");
                properties[4] = "Customer Code:" + row["customerCode"].ToString();
                properties[5] = "Latitude:" + row["lat"].ToString();
                properties[6] = "Longitude:" + row["longituge"].ToString();
                properties[7] = "Carrier Technology:" + row["tech"].ToString();
                properties[8] = "Device Type:" + row["deviceType"].ToString();
            }
            dbGetSet.Query = "select * from info1 where msn = " + str + " and OBIS = '1.0.15.8.0.255' order by Time_stamp desc limit 1;";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                properties[0] = "kWh Total:" + row["Amount"].ToString();
            dbGetSet.Query = "select * from info1 where msn = " + str + " and OBIS = '1.0.94.92.0.255' order by Time_stamp desc limit 1;";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                properties[1] = "kVARh Total:" + row["Amount"].ToString();
            return properties;
        }

        public string GetVoltagesofUnits()
        {
            string str = "[";
            DataTable dataTable = new DataTable();
            foreach (DataRow row in (InternalDataCollectionBase)new DBGetSet()
            {
                Query = ("SELECT meter.subutility,meter.subsubsubutility, meter_data.a_phase_and_n_voltage, meter_data.b_phase_and_n_voltage, meter_data.c_phase_and_n_voltage, meter_data.Time_stamp FROM dingrail.meter JOIN dingrail.meter_data ON meter.subsubsubutility= meter_data.meter_no;")
            }.ExecuteReader().Rows)
            {
                str += new JavaScriptSerializer().Serialize((object)new
                {
                    Unit= row["subutility"].ToString(),
                    Meter_no = row["subsubsubutility"].ToString(),
                    Voltage_phase1= row["a_phase_and_n_voltage"].ToString(),
                    Voltage_phase2 = row["b_phase_and_n_voltage"].ToString(),
                    Voltage_phase3 = row["c_phase_and_n_voltage"].ToString(),
                    Time_Stamp = row["Time_stamp"].ToString()
                });
                str += ",";
            }
            return str.Substring(0, str.Length - 1) + "]";

        }


        public string GetCurrent()
        {
            string str = "[";
            DataTable dataTable = new DataTable();
            foreach (DataRow row in (InternalDataCollectionBase)new DBGetSet()
            {
                Query = ("SELECT meter.subutility,meter.subsubsubutility, meter_data.current_a, meter_data.current_b, meter_data.current_c, meter_data.Time_stamp FROM dingrail.meter JOIN dingrail.meter_data ON meter.subsubsubutility= meter_data.meter_no;")
            }.ExecuteReader().Rows)
            {
                str += new JavaScriptSerializer().Serialize((object)new
                {
                    Unit = row["subutility"].ToString(),
                    Meter_no = row["subsubsubutility"].ToString(),
                    Current_phase_A = row["current_a"].ToString(),
                    Current_phase_B = row["current_b"].ToString(),
                    Current_phase_C = row["current_c"].ToString(),
                    Time_Stamp = row["Time_stamp"].ToString()
                });
                str += ",";
            }
            return str.Substring(0, str.Length - 1) + "]";

        }

        public string GetMuteData()
        {
            string str = "[";
            DataTable dataTable = new DataTable();
            foreach (DataRow row in (InternalDataCollectionBase)new DBGetSet()
            {
                Query = ("SELECT subsubsubutility,lastRead FROM dingrail.meter where lastRead < '" + DateTime.Now.AddDays(-1).ToString("yyyy/M/d HH:mm:ss") + "' order by subsubsubutility;")
            }.ExecuteReader().Rows)
            {
                str += new JavaScriptSerializer().Serialize((object)new
                {
                    Serial = row["subsubsubutility"].ToString(),
                    LastRead = row["lastRead"].ToString()
                });
                str += ",";
            }
            return str.Substring(0, str.Length - 1) + "]";
        }

        public string powerfactor()
        {
            try
            {
                string str = "[";
                DataTable dataTable = new DataTable();
                DBGetSet dbGetSet = new DBGetSet();
                dbGetSet.Query = "SELECT  meter_no, Time_stamp, combine_power_factor FROM dingrail.meter_data;";

                foreach (DataRow row in dbGetSet.ExecuteReader().Rows)
                {
                    
                    str += new JavaScriptSerializer().Serialize(new
                    {
                        Meter_no = row["meter_no"].ToString(),
                        Time_Stamp = row["Time_stamp"].ToString(),
                        pf = row["combine_power_factor"].ToString()
                    });
                    str += ",";
                }

                return str.TrimEnd(',') + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string year_consumption()
        {
            try
            {
                string str = "[";
                DataTable dataTable = new DataTable();
                DBGetSet dbGetSet = new DBGetSet();
                dbGetSet.Query = "SELECT meter.subutility AS department,YEAR(t1.Time_stamp) AS year, ABS(MAX(t1.total_active_energy) - MIN(t3.total_active_energy)) AS consumption FROM ( SELECT meter_no,MAX(Time_stamp) AS max_time, MIN(Time_stamp) AS min_time FROM dingrail.meter_data GROUP BY meter_no HAVING COUNT(DISTINCT YEAR(Time_stamp)) = 2) AS t JOIN dingrail.meter_data AS t1 ON t.meter_no = t1.meter_no AND t.max_time = t1.Time_stamp JOIN dingrail.meter_data AS t3 ON t.meter_no = t3.meter_no AND t.min_time = t3.Time_stamp JOIN dingrail.meter ON t1.meter_no = meter.subsubsubutility GROUP BY department, year ORDER BY department, year;";

                foreach (DataRow row in dbGetSet.ExecuteReader().Rows)
                {
                    str += new JavaScriptSerializer().Serialize(new
                    {
                        department = row["department"].ToString(),
                        kWh = row["consumption"].ToString(),
                        date = row["year"].ToString()
                    });
                    str += ",";
                }
                return str.TrimEnd(',') + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string tedata()
        {
            try
            {
                string str = "[";
                DataTable dataTable = new DataTable();
                DBGetSet dbGetSet = new DBGetSet();
                dbGetSet.Query = "SELECT meter.subutility AS department,t1.Time_stamp AS month,ABS(MAX(t1.Total_Active_energy) - MIN(t3.Total_Active_energy)) AS consumption FROM(SELECT meter_no, MAX(Time_stamp) AS max_time, MIN(Time_stamp) AS min_time FROM dingrail.meter_data GROUP BY meter_no HAVING COUNT(DISTINCT MONTH(Time_stamp)) >2) AS t JOIN dingrail.meter_data AS t1 ON t.meter_no = t1.meter_no AND t.max_time = t1.Time_stamp JOIN dingrail.meter_data AS t3 ON t.meter_no = t3.meter_no AND t.min_time = t3.Time_stamp JOIN dingrail.meter ON t1.meter_no = meter.subsubsubutility GROUP BY department,month ORDER BY department,month;";

                foreach (DataRow row in dbGetSet.ExecuteReader().Rows)
                {
                    str += new JavaScriptSerializer().Serialize(new
                    {
                        department = row["department"].ToString(),
                        date = row["month"].ToString(),
                        kWh = row["consumption"].ToString()
                       
                    });
                    str += ",";
                }
                return str.TrimEnd(',') + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public string tedata2(int serial)
        {
            try
            {
                string str = "[";
                DataTable dataTable = new DataTable();
                DBGetSet dbGetSet = new DBGetSet();
                DateTime now = DateTime.Now;
                DateTime dateTime = new DateTime(now.Year, now.Month, 1);
                dbGetSet.Query = "SELECT * from dingrail.systempower WHERE meter_serial = " + serial + " AND Time_stamp > '" + dateTime.ToString("yyyy/M/d HH:mm:ss") + "';";

                foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {

                        Energy = float.Parse(row["rpower"].ToString()).ToString("F2"),
                        Time = DateTime.Parse(row["Time_stamp"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    });
                    str += ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string tedata3(int serial)
        {
            try
            {
                string str = "[";
                DataTable dataTable = new DataTable();
                DBGetSet dbGetSet = new DBGetSet();
                DateTime now = DateTime.Now;
                DateTime dateTime = new DateTime(now.Year, now.Month, 1);
                dbGetSet.Query = "SELECT * from dingrail.systempower WHERE meter_serial = " + serial + " AND Time_stamp > '" + dateTime.ToString("yyyy/M/d HH:mm:ss") + "';";

                foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {

                        Energy = float.Parse(row["power"].ToString()).ToString("F2"),
                        Time = DateTime.Parse(row["Time_stamp"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    });
                    str += ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string tedata4(int serial)
        {
            try
            {
                string str = "[";
                DataTable dataTable = new DataTable();
                DBGetSet dbGetSet = new DBGetSet();
                DateTime now = DateTime.Now;
                DateTime dateTime = new DateTime(now.Year, now.Month, 1);
                dbGetSet.Query = "SELECT a_phase_and_n_voltage,b_phase_and_n_voltage,c_phase_and_n_voltage, Time_stamp FROM dingrail.meter_data where meter_no = " + serial + " AND Time_stamp > '" + dateTime.ToString("yyyy/M/d HH:mm:ss") + "';";

                foreach (DataRow row in dbGetSet.ExecuteReader().Rows)
                {
                    str += new JavaScriptSerializer().Serialize(new
                    {
                        V1 = float.Parse(row["a_phase_and_n_voltage"].ToString()).ToString("F2"),
                        V2 = float.Parse(row["b_phase_and_n_voltage"].ToString()).ToString("F2"),
                        V3 = float.Parse(row["c_phase_and_n_voltage"].ToString()).ToString("F2"),
                        Time = DateTime.Parse(row["Time_stamp"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    });
                    str += ",";
                }
                return str.TrimEnd(',') + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string currentreading(int serial)
        {
            try
            {
                string str = "[";
                DataTable dataTable = new DataTable();
                DBGetSet dbGetSet = new DBGetSet();
                DateTime now = DateTime.Now;
                DateTime dateTime = new DateTime(now.Year, now.Month, 1);
                dbGetSet.Query = "SELECT current_a,current_b,current_c, Time_stamp FROM dingrail.meter_data where meter_no = " + serial + " AND Time_stamp > '" + dateTime.ToString("yyyy/M/d HH:mm:ss") + "';";

                foreach (DataRow row in dbGetSet.ExecuteReader().Rows)
                {
                    str += new JavaScriptSerializer().Serialize(new
                    {
                        current_a = float.Parse(row["current_a"].ToString()).ToString("F2"),
                        current_b = float.Parse(row["current_b"].ToString()).ToString("F2"),
                        current_c = float.Parse(row["current_c"].ToString()).ToString("F2"),
                        Time = DateTime.Parse(row["Time_stamp"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    });
                    str += ",";
                }
                return str.TrimEnd(',') + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string getpf(int serial)
        {
            try
            {
                string str = "[";
                DataTable dataTable = new DataTable();
                DBGetSet dbGetSet = new DBGetSet();
                DateTime now = DateTime.Now;
                DateTime dateTime = new DateTime(now.Year, now.Month, 1);
                dbGetSet.Query = "SELECT combine_power_factor, Time_stamp FROM dingrail.meter_data where meter_no = " + serial + " AND Time_stamp > '" + dateTime.ToString("yyyy/M/d HH:mm:ss") + "';";

                foreach (DataRow row in dbGetSet.ExecuteReader().Rows)
                {
                    str += new JavaScriptSerializer().Serialize(new
                    {
                        average_pf = float.Parse(row["combine_power_factor"].ToString()).ToString("F2"),
                        Time = DateTime.Parse(row["Time_stamp"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                    });
                    str += ",";
                }
                return str.TrimEnd(',') + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string getGroups()
        {
            try
            {
                string str = "[";
                DataTable dataTable = new DataTable();
                DBGetSet dbGetSet = new DBGetSet();
                DateTime now = DateTime.Now;
                DateTime dateTime = new DateTime(now.Year, now.Month, 1);
                dbGetSet.Query = "SELECT * from devicegroups;";
                foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {
                        id = row["id"].ToString(),
                        category = row["category"].ToString(),
                        parentNode = row["parentNode"].ToString()
                    });
                    str += ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        
        public string GPApp(ulong Serial)
        {
            DataTable dataTable1 = new DataTable();
            DBGetSet dbGetSet = new DBGetSet();
            dbGetSet.Query = "select * from meter where serial = " + (object)Serial + ";";
            DataTable dataTable2 = dbGetSet.ExecuteReader();
            string[] strArray = new string[11];
            string str1 = "0";
            for (int index = 0; index < strArray.Length; ++index)
                strArray[index] = "Data N/A";
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                str1 = row["msn"].ToString();
                strArray[9] = !(row["connected"].ToString() == "1") ? "Device Disconnected" : "Device Connected";
                strArray[10] = !(row["relayStatus"].ToString() == "1") ? "Relay Disconnected" : "Relay Connected";
                strArray[2] = DateTime.Parse(row["lastRead"].ToString()).ToString("yyyy-M-dd HH:mm:ss");
                strArray[3] = DateTime.Parse(row["times"].ToString()).ToString("yyyy-M-dd HH:mm:ss");
                strArray[4] = row["customerCode"].ToString();
                strArray[5] = row["lat"].ToString();
                strArray[6] = row["longituge"].ToString();
                strArray[7] = row["tech"].ToString();
                strArray[8] = row["deviceType"].ToString();
            }
            dbGetSet.Query = "select * from info1 where msn = " + str1 + " and OBIS = '1.0.15.8.0.255' order by Time_stamp desc limit 1;";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                strArray[0] = row["Amount"].ToString();
            dbGetSet.Query = "select * from info1 where msn = " + str1 + " and OBIS = '1.0.94.92.0.255' order by Time_stamp desc limit 1;";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                strArray[1] = row["Amount"].ToString();
            string str2 = "";
            string str3 = "10";
            dbGetSet.Query = "select * from info1 where msn = " + str1 + " and OBIS = '1.0.15.6.0.255' order by Time_stamp desc limit 1;";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                str2 = row["Amount"].ToString();
            string str4 = "";
            string str5 = "";
            dbGetSet.Query = "select * from info1 where msn = " + str1 + " and OBIS = '1.0.15.8.1.255' order by Time_stamp desc limit 1;";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                str4 = row["Amount"].ToString();
            dbGetSet.Query = "select * from info1 where msn = " + str1 + " and OBIS = '1.0.15.8.2.255' order by Time_stamp desc limit 1;";
            foreach (DataRow row in (InternalDataCollectionBase)dbGetSet.ExecuteReader().Rows)
                str5 = row["Amount"].ToString();
            return new JavaScriptSerializer().Serialize((object)new
            {
                kWh = strArray[0],
                kVARh = strArray[1],
                Lat = strArray[5],
                Long = strArray[6],
                Carrier = strArray[7],
                Type = strArray[8],
                RelayStatus = strArray[10],
                LastReading = strArray[2],
                LastCommunication = strArray[3],
                MDI = str2,
                SanctionedLoad = str3,
                Tariff1 = str4,
                Tariff2 = str5
            });
        }

        protected string eventCodeTranslation(string code)
        {
            switch (code)
            {
                case "101":
                    return "MDI Reset";
                case "131":
                    return "MDI Reset End";
                case "102":
                    return "Parameterization";
                case "132":
                    return "Parameterization End";
                case "111":
                    return "Power fail start";
                case "112":
                    return "Power fail end";
                case "113":
                    return "Phase failure";
                case "143":
                    return "Phase failure End";
                case "114":
                    return "Over Volt";
                case "144":
                    return "Over Volt End";
                case "115":
                    return "Under Volt";
                case "145":
                    return "Under Volt End";
                case "116":
                    return "Demand Over Load";
                case "146":
                    return "Demand Over Load End";
                case "117":
                    return "Reverse Energy";
                case "147":
                    return "Reverse Energy End";
                case "118":
                    return "Reverse Polarity";
                case "148":
                    return "Reverse Polarity End";
                case "121":
                    return "CT Bypass";
                case "151":
                    return "CT Bypass End";
                case "44":
                    return "Cover Opened";
                case "45":
                    return "Cover Opened End";
                case "54":
                    return "Relay Disconnected";
                case "55":
                    return "Relay Reconnected";
                case "119":
                    return "Reactive Negative Energy";
                case "149":
                    return "Reactive Negative Energy Recovery";
                default:
                    return "";
            }
        }

        public string GetCircles(string parentNode, string username)
        {
            try
            {
                string str1 = "[";
                string str2 = "";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("Select typ from users where username = '" + username + "';").Rows)
                    str2 = row["typ"].ToString();
                if (str2 == "Admin")
                {
                    foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("Select * from customergroups where category = 'Circle';").Rows)
                    {
                        int count = this.ExecuteReader("select referenceNo from consumers where circle = '" + row["areaID"].ToString() + "' and !((circle = '' or circle = 'Unsorted') or (divi = '' or divi = 'Unsorted') or (subdivi = '' or subdivi = 'Unsorted'));").Rows.Count;
                        str1 += new JavaScriptSerializer().Serialize((object)new
                        {
                            areaID = row["areaID"].ToString(),
                            count = count
                        });
                        str1 += ",";
                    }
                    DataTable dataTable = this.ExecuteReader("select referenceNo from consumers where ((circle = '' or circle = 'Unsorted') or (divi = '' or divi = 'Unsorted') or (subdivi = '' or subdivi = 'Unsorted'));");
                    if (dataTable.Rows.Count > 0)
                        str1 = str1 + new JavaScriptSerializer().Serialize((object)new
                        {
                            areaID = "Unsorted",
                            count = dataTable.Rows.Count
                        }) + ",";
                }
                else
                {
                    DataTable dataTable1 = this.ExecuteReader("select * from portalaccess where username = '" + username + "';");
                    Circles[] circlesArray = new Circles[100];
                    for (int index = 0; index < 100; ++index)
                        circlesArray[index] = new Circles();
                    int index1 = 0;
                    foreach (DataRow row in (InternalDataCollectionBase)dataTable1.Rows)
                    {
                        DataTable dataTable2 = this.ExecuteReader("select * from consumers where subdivi = '" + row["areaID"].ToString() + "';");
                        if (dataTable2.Rows.Count > 0)
                        {
                            bool flag = false;
                            for (int index2 = 0; index2 < index1; ++index2)
                            {
                                if (circlesArray[index2].Circle == dataTable2.Rows[0]["circle"].ToString())
                                {
                                    ++circlesArray[index2].Count;
                                    flag = true;
                                }
                            }
                            if (!flag)
                            {
                                circlesArray[index1].Circle = dataTable2.Rows[0]["circle"].ToString();
                                circlesArray[index1].Count = dataTable2.Rows.Count;
                                ++index1;
                            }
                        }
                    }
                    for (int index3 = 0; index3 < index1; ++index3)
                        str1 = str1 + new JavaScriptSerializer().Serialize((object)new
                        {
                            areaID = circlesArray[index3].Circle,
                            count = circlesArray[index3].Count
                        }) + ",";
                }
                return str1.Substring(0, str1.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetDivisions(string parentNode, string username)
        {
            try
            {
                string str1 = "[";
                string str2 = "";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("Select typ from users where username = '" + username + "';").Rows)
                    str2 = row["typ"].ToString();
                if (str2 == "Admin")
                {
                    if (parentNode == "Unsorted")
                    {
                        DataTable dataTable = this.ExecuteReader("select referenceNo from consumers where ((circle = '' or circle = 'Unsorted') or (divi = '' or divi = 'Unsorted') or (subdivi = '' or subdivi = 'Unsorted'));");
                        return str1 + new JavaScriptSerializer().Serialize((object)new
                        {
                            areaID = "Unsorted",
                            count = dataTable.Rows.Count
                        }) + "]";
                    }
                    foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("Select areaID from customergroups where parentNode = '" + parentNode + "' and category = 'Sub Division';").Rows)
                    {
                        int count = this.ExecuteReader("select referenceNo from consumers where subdivi = '" + row["areaID"].ToString() + "' and divi = '" + parentNode + "' and !((circle = '' or circle = 'Unsorted') or (divi = '' or divi = 'Unsorted') or (subdivi = '' or subdivi = 'Unsorted'));").Rows.Count;
                        str1 += new JavaScriptSerializer().Serialize((object)new
                        {
                            areaID = row["areaID"].ToString(),
                            count = count
                        });
                        str1 += ",";
                    }
                }
                else
                {
                    DataTable dataTable1 = this.ExecuteReader("select * from portalaccess where username = '" + username + "';");
                    Circles[] circlesArray = new Circles[100];
                    for (int index = 0; index < 100; ++index)
                        circlesArray[index] = new Circles();
                    int index1 = 0;
                    foreach (DataRow row in (InternalDataCollectionBase)dataTable1.Rows)
                    {
                        DataTable dataTable2 = this.ExecuteReader("select * from consumers where circle = '" + parentNode + "' and subdivi = '" + row["areaID"].ToString() + "';");
                        if (dataTable2.Rows.Count > 0)
                        {
                            bool flag = false;
                            for (int index2 = 0; index2 < index1; ++index2)
                            {
                                if (circlesArray[index2].Circle == dataTable2.Rows[0]["divi"].ToString())
                                {
                                    ++circlesArray[index2].Count;
                                    flag = true;
                                }
                            }
                            if (!flag)
                            {
                                circlesArray[index1].Circle = dataTable2.Rows[0]["divi"].ToString();
                                circlesArray[index1].Count = dataTable2.Rows.Count;
                                ++index1;
                            }
                        }
                    }
                    for (int index3 = 0; index3 < index1; ++index3)
                        str1 = str1 + new JavaScriptSerializer().Serialize((object)new
                        {
                            areaID = circlesArray[index3].Circle,
                            count = circlesArray[index3].Count
                        }) + ",";
                }
                return str1.Substring(0, str1.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetSubDivisions(string parentNode, string username)
        {
            try
            {
                string str1 = "[";
                string str2 = "";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("Select typ from users where username = '" + username + "';").Rows)
                    str2 = row["typ"].ToString();
                if (str2 == "Admin")
                {
                    if (parentNode == "Unsorted")
                    {
                        DataTable dataTable = this.ExecuteReader("select referenceNo from consumers where ((circle = '' or circle = 'Unsorted') or (divi = '' or divi = 'Unsorted') or (subdivi = '' or subdivi = 'Unsorted'));");
                        return str1 + new JavaScriptSerializer().Serialize((object)new
                        {
                            areaID = "Unsorted",
                            count = dataTable.Rows.Count
                        }) + "]";
                    }
                    foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("Select areaID from customergroups where parentNode = '" + parentNode + "' and category = 'Sub Division';").Rows)
                    {
                        int count = this.ExecuteReader("select referenceNo from consumers where subdivi = '" + row["areaID"].ToString() + "' and divi = '" + parentNode + "' and !((circle = '' or circle = 'Unsorted') or (divi = '' or divi = 'Unsorted') or (subdivi = '' or subdivi = 'Unsorted'));").Rows.Count;
                        str1 += new JavaScriptSerializer().Serialize((object)new
                        {
                            areaID = row["areaID"].ToString(),
                            count = count
                        });
                        str1 += ",";
                    }
                }
                else
                {
                    DataTable dataTable1 = this.ExecuteReader("select * from portalaccess where username = '" + username + "';");
                    Circles[] circlesArray = new Circles[100];
                    for (int index = 0; index < 100; ++index)
                        circlesArray[index] = new Circles();
                    int index1 = 0;
                    foreach (DataRow row in (InternalDataCollectionBase)dataTable1.Rows)
                    {
                        DataTable dataTable2 = this.ExecuteReader("select * from consumers where divi = '" + parentNode + "' and subdivi = '" + row["areaID"].ToString() + "';");
                        if (dataTable2.Rows.Count > 0)
                        {
                            bool flag = false;
                            for (int index2 = 0; index2 < index1; ++index2)
                            {
                                if (circlesArray[index2].Circle == dataTable2.Rows[0]["subdivi"].ToString())
                                {
                                    ++circlesArray[index2].Count;
                                    flag = true;
                                }
                            }
                            if (!flag)
                            {
                                circlesArray[index1].Circle = dataTable2.Rows[0]["subdivi"].ToString();
                                circlesArray[index1].Count = dataTable2.Rows.Count;
                                ++index1;
                            }
                        }
                    }
                    for (int index3 = 0; index3 < index1; ++index3)
                        str1 = str1 + new JavaScriptSerializer().Serialize((object)new
                        {
                            areaID = circlesArray[index3].Circle,
                            count = circlesArray[index3].Count
                        }) + ",";
                }
                return str1.Substring(0, str1.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetCustomers(string parentNode)
        {
            try
            {
                string str1 = "[";
                DataTable dataTable = new DataTable();
                int num1 = 11;
                string str2 = "";
                float num2 = 4f;
                float num3 = 100f;
                if (parentNode == "Unsorted")
                {
                    foreach (DataRow row1 in (InternalDataCollectionBase)this.ExecuteReader("select referenceNo from consumers where ((circle = '' or circle = 'Unsorted') or (divi = '' or divi = 'Unsorted') or (subdivi = '' or subdivi = 'Unsorted'));").Rows)
                    {
                        foreach (DataRow row2 in (InternalDataCollectionBase)this.ExecuteReader("select connected,lastRead,rssi from meter where referenceNo = '" + row1["referenceNo"].ToString() + "';").Rows)
                        {
                            num1 = !(row2["connected"].ToString() == "0") ? 1 : 0;
                            if (DateTime.Parse(row2["lastRead"].ToString()) < DateTime.Now.AddDays(-1.0))
                                num1 = 10;
                            str2 = string.Concat((object)(float)((double)float.Parse(row2["rssi"].ToString()) / (double)num2 * (double)num3));
                        }
                        str1 += new JavaScriptSerializer().Serialize((object)new
                        {
                            referenceNo = row1["referenceNo"].ToString(),
                            count = num1,
                            rssi = str2
                        });
                        str1 += ",";
                    }
                    return str1.Substring(0, str1.Length - 1) + "]";
                }
                foreach (DataRow row3 in (InternalDataCollectionBase)this.ExecuteReader("Select referenceNo from consumers where subdivi = '" + parentNode + "' and !((circle = '' or circle = 'Unsorted') or (divi = '' or divi = 'Unsorted') or (subdivi = '' or subdivi = 'Unsorted'));").Rows)
                {
                    foreach (DataRow row4 in (InternalDataCollectionBase)this.ExecuteReader("select connected,lastRead,rssi from meter where referenceNo = '" + row3["referenceNo"].ToString() + "';").Rows)
                    {
                        num1 = !(row4["connected"].ToString() == "0") ? 1 : 0;
                        if (DateTime.Parse(row4["lastRead"].ToString()) < DateTime.Now.AddDays(-1.0))
                            num1 = 10;
                        str2 = string.Concat((object)(float)((double)float.Parse(row4["rssi"].ToString()) / (double)num2 * (double)num3));
                    }
                    str1 += new JavaScriptSerializer().Serialize((object)new
                    {
                        referenceNo = row3["referenceNo"].ToString(),
                        count = num1,
                        rssi = str2
                    });
                    str1 += ",";
                }
                return str1.Substring(0, str1.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetBatches(string parentNode)
        {
            try
            {
                string str = "[";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select areaID from customergroups where category = 'Batch' and parentNode = '" + parentNode + "';").Rows)
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {
                        areaID = row["areaID"].ToString()
                    });
                    str += ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string SetInfo(
          string referenceNo,
          string batchNo,
          string circle,
          string divi,
          string subdivi,
          string feeder,
          string txCode,
          string consumerName,
          string cnic,
          string contact,
          string address,
          string sodo,
          string email,
          string tariff,
          string secDep,
          string secDepDate,
          string appNo,
          string appDate,
          string sanctLoad,
          string txCap,
          string loadType,
          string installDate,
          string conndate)
        {
            try
            {
                this.ExecuteNonQurey("update consumers set batchNo = '" + batchNo + "',circle = '" + circle + "',divi = '" + divi + "',subdivi = '" + subdivi + "',feeder = '" + feeder + "',txCode = '" + txCode + "',consumerName = '" + consumerName + "',cnic = '" + cnic + "',contact = '" + contact + "',address = '" + address + "',sodo = '" + sodo + "',email = '" + email + "',tariff = '" + tariff + "',secDep = '" + secDep + "',secDepDate = '" + secDepDate + "',appNo = '" + appNo + "',appDate = '" + appDate + "',sanctLoad = '" + sanctLoad + "',txCap = '" + txCap + "',loadType = '" + loadType + "',installDate = '" + installDate + "',conndate = '" + conndate + "' where referenceNo = '" + referenceNo + "';");
                return "200";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetCustomerInfo(string referenceNo)
        {
            try
            {
                string str1 = "[";
                foreach (DataRow row1 in (InternalDataCollectionBase)this.ExecuteReader("select * from consumers where referenceNo = '" + referenceNo + "';").Rows)
                {
                    string str2 = "To be entered";
                    string str3 = "To be entered";
                    string str4 = "To be entered";
                    string str5 = "To be entered";
                    string str6 = "N/A";
                    string str7 = "N/A";
                    double num1 = 31.5546;
                    double num2 = 74.3572;
                    string str8 = "Not Confugured";
                    string str9 = "Not Configured";
                    foreach (DataRow row2 in (InternalDataCollectionBase)this.ExecuteReader("select * from meter where serial = '" + row1["meterNo"].ToString() + "';").Rows)
                    {
                        str2 = row2["mf"].ToString();
                        str3 = row2["simID"].ToString();
                        str4 = row2["deviceType"].ToString();
                        str5 = "0" + row2["phone"].ToString();
                        str6 = row2["times"].ToString();
                        str7 = row2["lastRead"].ToString();
                        num1 = double.Parse(row2["lat"].ToString());
                        num2 = double.Parse(row2["longituge"].ToString());
                        str8 = "0" + row2["smsAlert"].ToString();
                        str9 = row2["emailAlert"].ToString();
                    }
                    str1 += new JavaScriptSerializer().Serialize((object)new
                    {
                        referenceNo = row1[nameof(referenceNo)].ToString(),
                        meterNo = row1["meterNo"].ToString(),
                        batchNo = row1["batchNo"].ToString(),
                        circle = row1["circle"].ToString(),
                        divi = row1["divi"].ToString(),
                        subdivi = row1["subdivi"].ToString(),
                        feeder = row1["feeder"].ToString(),
                        txCode = row1["txCode"].ToString(),
                        consumerName = row1["consumerName"].ToString(),
                        cnic = row1["cnic"].ToString(),
                        contact = row1["contact"].ToString(),
                        address = row1["address"].ToString(),
                        sodo = row1["sodo"].ToString(),
                        email = row1["email"].ToString(),
                        tariff = row1["tariff"].ToString(),
                        secDep = row1["secDep"].ToString(),
                        secDepDate = row1["secDepDate"].ToString(),
                        appNo = row1["appNo"].ToString(),
                        appDate = row1["appDate"].ToString(),
                        sanctLoad = row1["sanctLoad"].ToString(),
                        txCap = row1["txCap"].ToString(),
                        loadType = row1["loadType"].ToString(),
                        installDate = row1["installDate"].ToString(),
                        conndate = row1["conndate"].ToString(),
                        initkWht = row1["initkWht"].ToString(),
                        initkWht1 = row1["initkWht1"].ToString(),
                        initkWht2 = row1["initkWht2"].ToString(),
                        mf = str2,
                        deviceType = str4,
                        simID = str3,
                        phone = str5,
                        lastCommunication = str6,
                        lastRead = str7,
                        latitude = num1,
                        longitude = num2,
                        smsAlert = str8,
                        emailAlert = str9
                    });
                    str1 += ",";
                }
                return str1.Substring(0, str1.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetFreeMeters()
        {
            try
            {
                string str1 = "[";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select serial,connected from meter where referenceNo = 'N/A';").Rows)
                {
                    string str2 = "Offline";
                    if (row["connected"].ToString() == "1")
                        str2 = "Online";
                    str1 += new JavaScriptSerializer().Serialize((object)new
                    {
                        serial = row["serial"].ToString(),
                        status = str2
                    });
                    str1 += ",";
                }
                return str1.Substring(0, str1.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string assignMeter(string referenceNo, string serial, string username, string password)
        {
            try
            {
                string str1 = "0";
                string str2 = "0";
                string str3 = "0";
                if (this.ExecuteReader("select meterNo from consumers where referenceNo = '" + referenceNo + "' and meterNo = '" + serial + "';").Rows.Count > 0)
                    return "500";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select Amount,OBIS from info1 where id = '" + serial + "' and (obis = '1.0.15.8.0.255' or OBIS = '1.0.15.8.1.255' or OBIS= '1.0.15.8.2.255') order by Time_stamp desc limit 3;").Rows)
                {
                    switch (row["OBIS"].ToString())
                    {
                        case "1.0.15.8.0.255":
                            str1 = row["Amount"].ToString();
                            continue;
                        case "1.0.15.8.1.255":
                            str2 = row["Amount"].ToString();
                            continue;
                        case "1.0.15.8.2.255":
                            str3 = row["Amount"].ToString();
                            continue;
                        default:
                            continue;
                    }
                }
                this.ExecuteNonQurey("update consumers set meterNo = '" + serial + "', initkWht = '" + str1 + "', initkWht1 = '" + str2 + "', initkWht2 = '" + str3 + "' where referenceNo = '" + referenceNo + "';");
                this.ExecuteNonQurey("update meter set referenceNo = 'N/A' where referenceNo ='" + referenceNo + "';");
                this.ExecuteNonQurey("update meter set referenceNo = '" + referenceNo + "' where serial ='" + serial + "';");
                return "200";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected bool IsDigitsOnly(string str)
        {
            foreach (char ch in str)
            {
                if (ch < '0' || ch > '9')
                    return false;
            }
            return true;
        }

        public string GetEvents(
          string referenceNo,
          string code,
          int syear,
          int smonth,
          int sday,
          int eyear,
          int eday,
          int emonth)
        {
            try
            {
                int num = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                if (eday + 1 > num)
                    --eday;
                DateTime dateTime1 = new DateTime(syear, smonth, sday);
                DateTime dateTime2 = new DateTime(eyear, emonth, eday + 1);
                string str1 = "[";
                DataTable dataTable1 = new DataTable();
                string str2 = "";
                DataTable dataTable2 = this.ExecuteReader("select serial from meter where referenceNo = '" + referenceNo + "';");
                foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
                    str2 = row["serial"].ToString();
                if (code == null)
                    dataTable2 = this.ExecuteReader("select * from eventlist where (eventCode != 119 and eventCode != 149) and msn = '" + str2 + "' and Time_stamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp desc;");
                else if (this.IsDigitsOnly(code))
                    dataTable2 = this.ExecuteReader("select * from eventlist where (eventCode != 119 and eventCode != 149) and msn = '" + str2 + "' and eventCode = '" + code + "' and Time_stamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp desc;");
                foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
                {
                    str1 += new JavaScriptSerializer().Serialize((object)new
                    {
                        eventCode = row["eventCode"].ToString(),
                        description = this.eventCodeTranslation(row["eventCode"].ToString()),
                        Time_stamp = row["Time_stamp"].ToString()
                    });
                    str1 += ",";
                }
                return str1.Substring(0, str1.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string GetLPCustomer(
          string referenceNo,
          int smonth,
          int syear,
          int sday,
          int eyear,
          int emonth,
          int eday)
        {
            try
            {
                string str1 = "";
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = this.ExecuteReader("select serial from meter where referenceNo = '" + referenceNo + "';");
                if (dataTable2.Rows.Count < 1)
                    return "";
                foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
                    str1 = row["serial"].ToString();
                int num1 = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                if (eday + 1 > num1)
                    --eday;
                DateTime now = DateTime.Now;
                DateTime dateTime1 = new DateTime(syear, smonth, sday);
                DateTime dateTime2 = new DateTime(eyear, emonth, eday + 1);
                string str2 = "[";
                DataTable dataTable3 = new DBGetSet()
                {
                    Query = ("SELECT * FROM loadprofile1 where serial='" + str1 + "' and Time_stamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp limit 9999;")
                }.ExecuteReader();
                if (dataTable3.Rows.Count < 1)
                    return "Error: No Data";
                int num2 = 1;
                int num3 = 0;
                for (int index = 0; index < dataTable3.Rows.Count; index += num2)
                {
                    DataRow row = dataTable3.Rows[index];
                    str2 = str2 + new JavaScriptSerializer().Serialize((object)new
                    {
                        Serial = str1,
                        kWh = float.Parse(row["kWh"].ToString()),
                        kW = float.Parse(row["kW"].ToString()),
                        kVARh = float.Parse(row["kVARh"].ToString()),
                        kVAR = float.Parse(row["kVAR"].ToString()),
                        Time = Convert.ToUInt64(((DateTime)row["Time_stamp"]).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds)
                    }) + ",";
                    num3 = index;
                }
                if (num3 != dataTable3.Rows.Count - 1)
                {
                    DataRow row = dataTable3.Rows[dataTable3.Rows.Count - 1];
                    str2 = str2 + new JavaScriptSerializer().Serialize((object)new
                    {
                        Serial = str1,
                        kWh = float.Parse(row["kWh"].ToString()),
                        kW = float.Parse(row["kW"].ToString()),
                        kVARh = float.Parse(row["kVARh"].ToString()),
                        kVAR = float.Parse(row["kVAR"].ToString()),
                        Time = Convert.ToUInt64(((DateTime)row["Time_stamp"]).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds)
                    }) + ",";
                }
                return str2.Substring(0, str2.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetCustomerIData(
          string referenceNo,
          int smonth,
          int syear,
          int sday,
          int eyear,
          int emonth,
          int eday)
        {
            try
            {
                string str1 = "";
                string str2 = "[";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("SELECT serial FROM meter where referenceNo='" + referenceNo + "';").Rows)
                    str1 = row["serial"].ToString();
                int num = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                if (eday + 1 > num)
                    --eday;
                DateTime dateTime1 = new DateTime(syear, smonth, sday);
                DateTime dateTime2 = new DateTime(eyear, emonth, eday + 1);
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select * from info1 where id= '" + str1 + "' and (OBIS = '1.0.13.7.0.255' or OBIS = '1.0.32.7.0.255' or OBIS = '1.0.52.7.0.255' or OBIS = '1.0.72.7.0.255' or OBIS = '1.0.31.7.0.255' or OBIS = '1.0.51.7.0.255' or OBIS = '1.0.71.7.0.255') and Time_stamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp limit 10000;").Rows)
                {
                    str2 += new JavaScriptSerializer().Serialize((object)new
                    {
                        Obis = row["OBIS"].ToString(),
                        Description = this.OBISTranslation(row["OBIS"].ToString()),
                        Value = double.Parse(row["Amount"].ToString()),
                        Unit = row["unit"].ToString(),
                        Time = Convert.ToUInt64(((DateTime)row["Time_stamp"]).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds)
                    });
                    str2 += ",";
                }
                return str2.Substring(0, str2.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetCustomerBData(
          string referenceNo,
          int smonth,
          int syear,
          int sday,
          int eyear,
          int emonth,
          int eday)
        {
            try
            {
                string str1 = "";
                string str2 = "[";
                double num1 = 0.0;
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                double num5 = 0.0;
                double num6 = 0.0;
                double num7 = 0.0;
                double num8 = 0.0;
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("SELECT serial FROM meter where referenceNo='" + referenceNo + "';").Rows)
                    str1 = row["serial"].ToString();
                int num9 = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                if (eday + 1 > num9)
                    --eday;
                DateTime dateTime1 = new DateTime(syear, smonth, sday);
                DateTime dateTime2 = new DateTime(eyear, emonth, eday + 1);
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select * from info1 where id= '" + str1 + "' and (OBIS = '1.0.15.8.0.255' or OBIS = '1.0.15.8.1.255' or OBIS = '1.0.15.8.2.255' or OBIS = '1.0.15.6.0.255') and Time_stamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp limit 4;").Rows)
                {
                    switch (row["OBIS"].ToString())
                    {
                        case "1.0.15.8.0.255":
                            num1 = double.Parse(row["Amount"].ToString());
                            continue;
                        case "1.0.15.8.1.255":
                            num2 = double.Parse(row["Amount"].ToString());
                            continue;
                        case "1.0.15.8.2.255":
                            num3 = double.Parse(row["Amount"].ToString());
                            continue;
                        case "1.0.15.6.0.255":
                            num7 = double.Parse(row["Amount"].ToString());
                            continue;
                        default:
                            continue;
                    }
                }
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select * from info1 where id= '" + str1 + "' and (OBIS = '1.0.15.8.0.255' or OBIS = '1.0.15.8.1.255' or OBIS = '1.0.15.8.2.255' or OBIS = '1.0.15.6.0.255') and Time_stamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp desc limit 4;").Rows)
                {
                    switch (row["OBIS"].ToString())
                    {
                        case "1.0.15.8.0.255":
                            num4 = double.Parse(row["Amount"].ToString());
                            continue;
                        case "1.0.15.8.1.255":
                            num5 = double.Parse(row["Amount"].ToString());
                            continue;
                        case "1.0.15.8.2.255":
                            num6 = double.Parse(row["Amount"].ToString());
                            continue;
                        case "1.0.15.6.0.255":
                            num8 = double.Parse(row["Amount"].ToString());
                            continue;
                        default:
                            continue;
                    }
                }
                return str2 + new JavaScriptSerializer().Serialize((object)new
                {
                    kWhst = double.Parse(num1.ToString("F0")),
                    kWhst1 = double.Parse(num2.ToString("F0")),
                    kWhst2 = double.Parse(num3.ToString("F0")),
                    mdicurr = double.Parse(num8.ToString("F0")),
                    kWhet = double.Parse(num4.ToString("F0")),
                    kWhet1 = double.Parse(num5.ToString("F0")),
                    kWhet2 = double.Parse(num6.ToString("F0")),
                    mdiprev = double.Parse(num7.ToString("F0"))
                }) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string getMutes(string username)
        {
            try
            {
                string str = "[";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select serial,referenceNo,lastRead from meter where lastRead < '" + DateTime.Now.AddDays(-1.0).ToString("yyyy/M/d HH:mm:ss") + "' and referenceNo != 'N/A';").Rows)
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {
                        serial = row["serial"].ToString(),
                        referenceNo = row["referenceNo"].ToString(),
                        lastRead = row["lastRead"].ToString()
                    });
                    str += ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string getallareas()
        {
            try
            {
                string str = "[";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select * from customergroups where category = 'Sub Division';").Rows)
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {
                        areaID = row["areaID"].ToString(),
                        category = row["category"].ToString(),
                        parentNode = row["parentNode"].ToString()
                    });
                    str += ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string getallusers()
        {
            try
            {
                string str = "[";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select * from users where typ != 'Admin';").Rows)
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {
                        username = row["username"].ToString(),
                        name = row["name"].ToString(),
                        contact = row["contact"].ToString()
                    });
                    str += ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string getalladivi(string username)
        {
            try
            {
                string str = "[";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select * from portalaccess where username = '" + username + "';").Rows)
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {
                        areaID = row["areaID"].ToString(),
                        category = row["category"].ToString()
                    });
                    str += ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string insertadivi(string username, string division)
        {
            try
            {
                string str = "";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select * from customergroups where areaID = '" + division + "' and category = 'Sub Division';").Rows)
                    str = row["parentNode"].ToString();
                if (this.ExecuteReader("select * from portalaccess where username = '" + username + "' and areaID = '" + division + "';").Rows.Count > 0)
                    return "Success";
                this.ExecuteNonQurey("insert into portalaccess(username,areaID,category,parentNode) values('" + username + "','" + division + "','Sub Division','" + str + "');");
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string deleteadivi(string username, string division)
        {
            try
            {
                this.ExecuteNonQurey("delete from portalaccess where username = '" + username + "' and areaID = '" + division + "';");
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        // Update 1
        public string getInventory()
        {
            try
            {
                string str = "[";
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select * from meterinventory;").Rows)
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {
                        serial = row["serial"].ToString(),
                        typ = row["typ"].ToString(),
                        time = row["Time_stamp"].ToString()
                    });
                    str += ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        // Update 2
        public string setInventory(string serial, string typ)
        {
            try
            {
                if (serial.Length < 1 || !this.IsDigitsOnly(serial))
                    return "No serial entered.";
                if (this.ExecuteReader("select serial from meterinventory where serial = '" + serial + "';").Rows.Count > 0)
                    return "Device already added to inventory";
                this.ExecuteNonQurey("insert into meterinventory(serial,typ,Time_stamp) values('" + serial + "','" + typ + "','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "')");
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string getBillnow(string batch, string username)
        {
            try
            {
                string str1 = "[";
                double num1 = 0.0;
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                double num5 = 0.0;
                double num6 = 0.0;
                double num7 = 0.0;
                double num8 = 0.0;
                double num9 = 0.0;
                double num10 = 0.0;
                double num11 = 0.0;
                double num12 = 0.0;
                double num13 = 0.0;
                string str2 = "";
                foreach (DataRow row1 in (InternalDataCollectionBase)this.ExecuteReader("select * from meter where exportbatch = '" + batch + "';").Rows)
                {
                    foreach (DataRow row2 in (InternalDataCollectionBase)this.ExecuteReader("select * from info1 where quantity = 0 and id = '" + row1["serial"].ToString() + "' order by Time_stamp desc limit 14;").Rows)
                    {
                        switch (row2["OBIS"].ToString())
                        {
                            case "1.0.15.8.0.255":
                                num1 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.15.8.1.255":
                                num2 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.15.8.2.255":
                                num3 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.94.92.0.255":
                                num4 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.94.92.1.255":
                                num5 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.94.92.2.255":
                                num6 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.15.6.0.255":
                                num7 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.15.6.1.255":
                                num8 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.15.6.2.255":
                                num9 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.15.2.0.255":
                                num10 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.15.2.1.255":
                                num11 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.15.2.2.255":
                                num12 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.0.1.0.255":
                                num13 = double.Parse(row2["Amount"].ToString());
                                continue;
                            case "1.0.0.1.2.255":
                                str2 = row2["Amount"].ToString();
                                continue;
                            default:
                                continue;
                        }
                    }
                    str1 += new JavaScriptSerializer().Serialize((object)new
                    {
                        serial = row1["serial"].ToString(),
                        kWh = num1,
                        kWht1 = num2,
                        kWht2 = num3,
                        kVARh = num4,
                        kVARht1 = num5,
                        kVARht2 = num6,
                        mdi = num7,
                        mdit1 = num8,
                        mdit2 = num9,
                        cmdi = num10,
                        cmdit1 = num11,
                        cmdit2 = num12,
                        mdireset = num13,
                        mdiresetdt = str2
                    });
                    str1 += ",";
                }
                return str1.Substring(0, str1.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetDeviceIData(string serial, int smonth, int syear)
        {
            try
            {
                string str = "[";
                DateTime dateTime1 = new DateTime(syear, smonth, 1);
                DateTime dateTime2 = dateTime1.AddMonths(1);
                foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select * from info1 where id= '" + serial + "' and (OBIS = '1.0.13.7.0.255' or OBIS = '1.0.32.7.0.255' or OBIS = '1.0.52.7.0.255' or OBIS = '1.0.72.7.0.255' or OBIS = '1.0.31.7.0.255' or OBIS = '1.0.51.7.0.255' or OBIS = '1.0.71.7.0.255') and Time_stamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp limit 10000;").Rows)
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {
                        Obis = row["OBIS"].ToString(),
                        Description = this.OBISTranslation(row["OBIS"].ToString()),
                        Value = double.Parse(row["Amount"].ToString()),
                        Unit = row["unit"].ToString(),
                        Time = Convert.ToUInt64(((DateTime)row["Time_stamp"]).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds)
                    });
                    str += ",";
                }
                return str.Substring(0, str.Length - 1) + "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string getuserbatches(string username)
        {
            string str = "[";
            return str.Substring(0, str.Length - 1) + "]";
        }

        public string getlatlong(string username)
        {
            string str = "[";
            foreach (DataRow row in (InternalDataCollectionBase)this.ExecuteReader("select serial,lat,longituge,connected from meter;").Rows)
            {
                if (row["connected"].ToString() == "1")
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {
                        status = "Online",
                        serial = row["serial"].ToString(),
                        lat = row["lat"].ToString(),
                        longituge = row["longituge"].ToString()
                    });
                    str += ",";
                }
                else
                {
                    str += new JavaScriptSerializer().Serialize((object)new
                    {
                        status = "Offline",
                        serial = row["serial"].ToString(),
                        lat = row["lat"].ToString(),
                        longituge = row["longituge"].ToString()
                    });
                    str += ",";
                }
            }
            return str.Substring(0, str.Length - 1) + "]";
        }
        //1
        public List<JObject> mainnode(string username)
        {
            List<JObject> jobjectList = new List<JObject>();
            DataTable dataTable = this.ExecuteReader("select utility, count(*) total from meter group by utility;");
            if (dataTable.Rows.Count < 1)
                return jobjectList;
            int num = 0;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                jobjectList.Add(new JObject()
                {
                    title = row["utility"].ToString(),
                    key = "a" + (object)num,
                    lazy = true,
                    folder = true,
                    tooltip = row["utility"].ToString() + " (" + row["total"].ToString() + ")",
                    icon = "fa fa-sitemap"
                });
                ++num;
            }
            return jobjectList;
        }
        //2
        public List<JObject> child(string username, string parent)
        {
            List<JObject> jobjectList = new List<JObject>();
            DataTable dataTable = this.ExecuteReader("select subutility, count(*) total from meter where utility = '" + parent + "' group by subutility;");
            if (dataTable.Rows.Count < 1)
                return jobjectList;
            int num = 0;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                jobjectList.Add(new JObject()
                {
                    title = row["subutility"].ToString(),
                    key = "b" + (object)(DateTime.Now.Ticks + (long)num),
                    lazy = true,
                    folder = true,
                    tooltip = row["subutility"].ToString() + " (" + row["total"].ToString() + ")",
                    icon = "fa fa-cubes"
                });
                ++num;
            }
            return jobjectList;
        }
        //3
        public List<JObject> grandchild(string username, string parent)
        {
            List<JObject> jobjectList = new List<JObject>();
            DataTable dataTable = this.ExecuteReader("select subsubutility, count(*) total from meter where subutility = '" + parent + "' group by subsubutility;");
            if (dataTable.Rows.Count < 1)
                return jobjectList;
            DateTime dateTime = new DateTime(2017, 9, 1);
            int num = 0;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                jobjectList.Add(new JObject()
                {
                    title = row["subsubutility"].ToString(),
                    key = "c" + (object)(DateTime.Now.Ticks + (long)num),
                    lazy = true,
                    folder = true,
                    tooltip = row["subsubutility"].ToString() + " (" + row["total"].ToString() + ")",
                    icon = "fa fa-cube"
                });
                ++num;
            }
            return jobjectList;
        }
        //4
        public List<JObject> greatgrandchild(string username, string parent)
        {
            int num = 0;
            List<JObject> jobjectList = new List<JObject>();
            DataTable dataTable = this.ExecuteReader("select subsubsubutility, count(*) total from meter where subsubutility = '" + parent + "' group by subsubsubutility;");
            if (dataTable.Rows.Count < 1)
                return jobjectList;
            DateTime dateTime = new DateTime(2017, 9, 1);
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                jobjectList.Add(new JObject()
                {
                    title = row["subsubsubutility"].ToString(),
                    key = "d" + (object)(DateTime.Now.Ticks + (long)num),
                    lazy = true,
                    folder = true,
                    tooltip = row["subsubsubutility"].ToString() + " (" + row["total"].ToString() + ")",
                    icon = "fa fa-user"
                });
                ++num;
            }
            return jobjectList;
        }

       

        public List<JObject> mainmeters()
        {
            int num = 0;
            List<JObject> jobjectList = new List<JObject>();
            DataTable dataTable = this.ExecuteReader("select subsubsubutility,connected,lastRead,times from meter order by subsubsubutility;");
            if (dataTable.Rows.Count < 1)
                return jobjectList;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                JObject jobject = new JObject();
                jobject.title = row["subsubsubutility"].ToString();
                jobject.key = row["subsubsubutility"].ToString();
                jobject.lazy = false;
                jobject.folder = false;
                jobject.tooltip = "Device Online. Last Reading: " + row["lastRead"].ToString() + ". Last Keep-Alive: " + row["times"].ToString();
                jobject.icon = "fa fa-heartbeat greenwhale";
                TimeSpan timeSpan = DateTime.Now.Subtract(DateTime.Parse(row["lastRead"].ToString()));
                if (row["connected"].ToString() == "0")
                {
                    jobject.tooltip = "Device Offline. Attempting to reconnect. Last Reading: " + row["lastRead"].ToString() + ". Last Keep-Alive: " + row["times"].ToString();
                    jobject.icon = "fa fa-medkit yellowwhale";
                }
                if (timeSpan.Days > 0)
                {
                    jobject.tooltip = "ALERT! Mute Device. Diagnosis required. Last Reading: " + row["lastRead"].ToString() + ". Last Keep-Alive: " + row["times"].ToString();
                    jobject.icon = "fa fa-ambulance redwhale";
                }
                jobjectList.Add(jobject);
                ++num;
            }
            return jobjectList;
        }

        public List<MeterProperties> metersInCircle(
          string username,
          string token,
          string parent)
        {
            int num = 0;
            List<MeterProperties> meterPropertiesList = new List<MeterProperties>();
            DataTable dataTable = this.ExecuteReader("select * from meter where area = '" + parent + "' order by serial;");
            if (dataTable.Rows.Count < 1)
                return meterPropertiesList;
            DateTime dateTime = new DateTime(2017, 9, 1);
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                MeterProperties meterProperties = new MeterProperties();
                meterProperties.area = row["area"].ToString();
                meterProperties.connected = "Device Offline";
                if (row["connected"].ToString() == "1")
                    meterProperties.connected = "Device Online";
                meterProperties.deviceGroup = row["deviceGroup"].ToString();
                meterProperties.deviceType = row["deviceType"].ToString();
                meterProperties.emailAlert = row["emailAlert"].ToString();
                meterProperties.exportbatch = row["exportbatch"].ToString();
                meterProperties.firmwareVersion = row["firmwareVersion"].ToString();
                meterProperties.job = row["job"].ToString();
                meterProperties.lastRead = row["lastRead"].ToString();
                meterProperties.lat = row["lat"].ToString();
                meterProperties.longituge = row["longituge"].ToString();
                meterProperties.phone = row["phone"].ToString();
                meterProperties.reading = row["reading"].ToString();
                meterProperties.relayDCRequest = row["relayDCRequest"].ToString();
                meterProperties.relayReconnect = row["relayReconnect"].ToString();
                meterProperties.relayStatus = row["relayStatus"].ToString();
                meterProperties.rssi = row["rssi"].ToString();
                meterProperties.serial = row["serial"].ToString();
                meterProperties.smsAlert = row["smsAlert"].ToString();
                meterProperties.subarea = row["subarea"].ToString();
                meterProperties.sync = row["sync"].ToString();
                meterProperties.tech = row["tech"].ToString();
                meterProperties.times = row["times"].ToString();
                meterProperties.userDeviceGroup = row["userDeviceGroup"].ToString();
                meterProperties.comments = row["comments"].ToString();
                meterProperties.latestkW = row["latestkW"].ToString();
                meterProperties.latestkVAR = row["latestkVAR"].ToString();
                meterProperties.latestkWh = row["latestkWh"].ToString();
                meterPropertiesList.Add(meterProperties);
                ++num;
            }
            return meterPropertiesList;
        }

        public List<MeterProperties> metersInDivision(
          string username,
          string token,
          string parent)
        {
            int num = 0;
            List<MeterProperties> meterPropertiesList = new List<MeterProperties>();
            DataTable dataTable = this.ExecuteReader("select * from meter where subarea = '" + parent + "' order by serial;");
            if (dataTable.Rows.Count < 1)
                return meterPropertiesList;
            DateTime dateTime = new DateTime(2017, 9, 1);
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                MeterProperties meterProperties = new MeterProperties();
                meterProperties.area = row["area"].ToString();
                meterProperties.connected = "Device Offline";
                if (row["connected"].ToString() == "1")
                    meterProperties.connected = "Device Online";
                meterProperties.deviceGroup = row["deviceGroup"].ToString();
                meterProperties.deviceType = row["deviceType"].ToString();
                meterProperties.emailAlert = row["emailAlert"].ToString();
                meterProperties.exportbatch = row["exportbatch"].ToString();
                meterProperties.firmwareVersion = row["firmwareVersion"].ToString();
                meterProperties.job = row["job"].ToString();
                meterProperties.lastRead = row["lastRead"].ToString();
                meterProperties.lat = row["lat"].ToString();
                meterProperties.longituge = row["longituge"].ToString();
                meterProperties.phone = row["phone"].ToString();
                meterProperties.reading = row["reading"].ToString();
                meterProperties.relayDCRequest = row["relayDCRequest"].ToString();
                meterProperties.relayReconnect = row["relayReconnect"].ToString();
                meterProperties.relayStatus = row["relayStatus"].ToString();
                meterProperties.rssi = row["rssi"].ToString();
                meterProperties.serial = row["serial"].ToString();
                meterProperties.smsAlert = row["smsAlert"].ToString();
                meterProperties.subarea = row["subarea"].ToString();
                meterProperties.sync = row["sync"].ToString();
                meterProperties.tech = row["tech"].ToString();
                meterProperties.times = row["times"].ToString();
                meterProperties.userDeviceGroup = row["userDeviceGroup"].ToString();
                meterProperties.comments = row["comments"].ToString();
                meterProperties.latestkW = row["latestkW"].ToString();
                meterProperties.latestkVAR = row["latestkVAR"].ToString();
                meterProperties.latestkWh = row["latestkWh"].ToString();
                meterPropertiesList.Add(meterProperties);
                ++num;
            }
            return meterPropertiesList;
        }

        public List<MeterProperties> metersInSubDivision(
          string username,
          string token,
          string parent)
        {
            int num = 0;
            List<MeterProperties> meterPropertiesList = new List<MeterProperties>();
            DataTable dataTable = this.ExecuteReader("select * from meter where subdivi = '" + parent + "' order by serial;");
            if (dataTable.Rows.Count < 1)
                return meterPropertiesList;
            DateTime dateTime = new DateTime(2017, 9, 1);
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                MeterProperties meterProperties = new MeterProperties();
                meterProperties.area = row["area"].ToString();
                meterProperties.connected = "Device Offline";
                if (row["connected"].ToString() == "1")
                    meterProperties.connected = "Device Online";
                meterProperties.deviceGroup = row["deviceGroup"].ToString();
                meterProperties.deviceType = row["deviceType"].ToString();
                meterProperties.emailAlert = row["emailAlert"].ToString();
                meterProperties.exportbatch = row["exportbatch"].ToString();
                meterProperties.firmwareVersion = row["firmwareVersion"].ToString();
                meterProperties.job = row["job"].ToString();
                meterProperties.lastRead = row["lastRead"].ToString();
                meterProperties.lat = row["lat"].ToString();
                meterProperties.longituge = row["longituge"].ToString();
                meterProperties.phone = row["phone"].ToString();
                meterProperties.reading = row["reading"].ToString();
                meterProperties.relayDCRequest = row["relayDCRequest"].ToString();
                meterProperties.relayReconnect = row["relayReconnect"].ToString();
                meterProperties.relayStatus = row["relayStatus"].ToString();
                meterProperties.rssi = row["rssi"].ToString();
                meterProperties.serial = row["serial"].ToString();
                meterProperties.smsAlert = row["smsAlert"].ToString();
                meterProperties.subarea = row["subarea"].ToString();
                meterProperties.sync = row["sync"].ToString();
                meterProperties.tech = row["tech"].ToString();
                meterProperties.times = row["times"].ToString();
                meterProperties.userDeviceGroup = row["userDeviceGroup"].ToString();
                meterProperties.comments = row["comments"].ToString();
                meterProperties.latestkW = row["latestkW"].ToString();
                meterProperties.latestkVAR = row["latestkVAR"].ToString();
                meterProperties.latestkWh = row["latestkWh"].ToString();
                meterPropertiesList.Add(meterProperties);
                ++num;
            }
            return meterPropertiesList;
        }

        public List<MeterProperties> metersInBatch(
          string username,
          string token,
          string parent)
        {
            int num = 0;
            List<MeterProperties> meterPropertiesList = new List<MeterProperties>();
            DataTable dataTable = this.ExecuteReader("select * from meter where batch = '" + parent + "' order by serial;");
            if (dataTable.Rows.Count < 1)
                return meterPropertiesList;
            DateTime dateTime = new DateTime(2017, 9, 1);
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                MeterProperties meterProperties = new MeterProperties();
                meterProperties.area = row["area"].ToString();
                meterProperties.connected = "Device Offline";
                if (row["connected"].ToString() == "1")
                    meterProperties.connected = "Device Online";
                meterProperties.deviceGroup = row["deviceGroup"].ToString();
                meterProperties.deviceType = row["deviceType"].ToString();
                meterProperties.emailAlert = row["emailAlert"].ToString();
                meterProperties.exportbatch = row["exportbatch"].ToString();
                meterProperties.firmwareVersion = row["firmwareVersion"].ToString();
                meterProperties.job = row["job"].ToString();
                meterProperties.lastRead = row["lastRead"].ToString();
                meterProperties.lat = row["lat"].ToString();
                meterProperties.longituge = row["longituge"].ToString();
                meterProperties.phone = row["phone"].ToString();
                meterProperties.reading = row["reading"].ToString();
                meterProperties.relayDCRequest = row["relayDCRequest"].ToString();
                meterProperties.relayReconnect = row["relayReconnect"].ToString();
                meterProperties.relayStatus = row["relayStatus"].ToString();
                meterProperties.rssi = row["rssi"].ToString();
                meterProperties.serial = row["serial"].ToString();
                meterProperties.smsAlert = row["smsAlert"].ToString();
                meterProperties.subarea = row["subarea"].ToString();
                meterProperties.sync = row["sync"].ToString();
                meterProperties.tech = row["tech"].ToString();
                meterProperties.times = row["times"].ToString();
                meterProperties.userDeviceGroup = row["userDeviceGroup"].ToString();
                meterProperties.latestkW = row["latestkW"].ToString();
                meterProperties.latestkVAR = row["latestkVAR"].ToString();
                meterProperties.latestkWh = row["latestkWh"].ToString();
                meterProperties.userDeviceGroup = row["latestkWh"].ToString();
                meterProperties.comments = row["comments"].ToString();
                meterPropertiesList.Add(meterProperties);
                ++num;
            }
            return meterPropertiesList;
        }

        public List<MeterProperties> meterProperties(string serial)
        {
            int num = 0;
            List<MeterProperties> meterPropertiesList = new List<MeterProperties>();
            DataTable dataTable = this.ExecuteReader("select * from meter where serial = '" + serial + "';");
            if (dataTable.Rows.Count < 1)
                return meterPropertiesList;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                MeterProperties meterProperties = new MeterProperties();
                meterProperties.area = row["area"].ToString();
                meterProperties.connected = "Device Offline";
                if (row["connected"].ToString() == "1")
                    meterProperties.connected = "Device Online";
                meterProperties.deviceGroup = row["deviceGroup"].ToString();
                meterProperties.deviceType = row["deviceType"].ToString();
                meterProperties.emailAlert = row["emailAlert"].ToString();
                meterProperties.exportbatch = row["exportbatch"].ToString();
                meterProperties.firmwareVersion = row["firmwareVersion"].ToString();
                meterProperties.job = row["job"].ToString();
                meterProperties.lastRead = row["lastRead"].ToString();
                meterProperties.lat = row["lat"].ToString();
                meterProperties.longituge = row["longituge"].ToString();
                meterProperties.phone = row["phone"].ToString();
                meterProperties.reading = row["reading"].ToString();
                meterProperties.relayDCRequest = row["relayDCRequest"].ToString();
                meterProperties.relayReconnect = row["relayReconnect"].ToString();
                meterProperties.relayStatus = row["relayStatus"].ToString();
                meterProperties.rssi = row["rssi"].ToString();
                meterProperties.serial = row[nameof(serial)].ToString();
                meterProperties.smsAlert = row["smsAlert"].ToString();
                meterProperties.subarea = row["subarea"].ToString();
                meterProperties.sync = row["sync"].ToString();
                meterProperties.tech = row["tech"].ToString();
                meterProperties.times = row["times"].ToString();
                meterProperties.userDeviceGroup = row["userDeviceGroup"].ToString();
                meterProperties.latestkW = row["latestkW"].ToString();
                meterProperties.latestkVAR = row["latestkVAR"].ToString();
                meterProperties.latestkWh = row["latestkWh"].ToString();
                meterProperties.comments = row["comments"].ToString();
                meterPropertiesList.Add(meterProperties);
                ++num;
            }
            return meterPropertiesList;
        }

        public List<EventData> getMeterEvents(string serial)
        {
            List<EventData> meterEvents = new List<EventData>();
            DataTable dataTable = this.ExecuteReader("select * from eventlist where msn = '" + serial + "' order by Time_stamp desc limit 1000;");
            if (dataTable.Rows.Count < 1)
                return meterEvents;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                meterEvents.Add(new EventData()
                {
                    msn = row["msn"].ToString(),
                    eventCode = row["eventCode"].ToString(),
                    description = this.eventCodeTranslation(row["eventCode"].ToString()),
                    Time_stamp = ((DateTime)row["Time_stamp"]).ToString()
                });
            return meterEvents;
        }

        public ApiKey authorization_service()
        {
            try
            {
                ApiKey apiKey = new ApiKey();
                apiKey.message = "Authentication Failed";
                apiKey.privatekey = "N/A";
                apiKey.status = "0";
                WebHeaderCollection headers = WebOperationContext.Current.IncomingRequest.Headers;
                string str1 = headers.GetValues("username")[0];
                string str2 = headers.GetValues("password")[0];
                string str3 = headers.GetValues("code")[0];
                if (!(str1 == "usaid") || !(str2 == "usaid@123@321"))
                    return apiKey;
                string base64String = Convert.ToBase64String(((IEnumerable<byte>)BitConverter.GetBytes(DateTime.UtcNow.ToBinary())).Concat<byte>((IEnumerable<byte>)Guid.NewGuid().ToByteArray()).ToArray<byte>());
                apiKey.privatekey = base64String;
                apiKey.status = "1";
                apiKey.message = "You are authenticated successfully. Private key will be valid for 30 Minutes";
                return apiKey;
            }
            catch (Exception ex)
            {
                return new ApiKey()
                {
                    message = ex.Message,
                    privatekey = "N/A",
                    status = "0"
                };
            }
        }

        public List<BillGen> bill_gen_service(
          string referenceNo,
          int smonth,
          int syear,
          int sday,
          int eyear,
          int emonth,
          int eday)
        {
            try
            {
                DateTime dateTime1 = new DateTime(syear, smonth, sday);
                DateTime dateTime2 = new DateTime(eyear, emonth, eday);
                DataTable dataTable = new DBGetSet()
                {
                    Query = ("select * from billingdata where `serial` = '" + referenceNo + "' and timeStamp between '" + dateTime1.ToString("yyyy/M/d HH:mm:ss") + "' and '" + dateTime2.ToString("yyyy/M/d HH:mm:ss") + "' order by `timeStamp` desc;")
                }.ExecuteReader();
                if (dataTable.Rows.Count < 1)
                    throw new IndexOutOfRangeException("No Records");
                return new List<BillGen>()
        {
          new BillGen()
          {
            total_kwh_consumed = Decimal.Parse(dataTable.Rows[0]["1.0.15.8.0.255"].ToString()) - Decimal.Parse(dataTable.Rows[dataTable.Rows.Count - 1]["1.0.15.8.0.255"].ToString()),
            total_kwh_offpeak = Decimal.Parse(dataTable.Rows[0]["1.0.15.8.2.255"].ToString()) - Decimal.Parse(dataTable.Rows[dataTable.Rows.Count - 1]["1.0.15.8.2.255"].ToString()),
            total_kwh_peak = Decimal.Parse(dataTable.Rows[0]["1.0.15.8.1.255"].ToString()) - Decimal.Parse(dataTable.Rows[dataTable.Rows.Count - 1]["1.0.15.8.1.255"].ToString()),
            curr_kwh_reading_total = Decimal.Parse(dataTable.Rows[0]["1.0.15.8.0.255"].ToString()),
            curr_kwh_reading_t1 = Decimal.Parse(dataTable.Rows[0]["1.0.15.8.1.255"].ToString()),
            curr_kwh_reading_t2 = Decimal.Parse(dataTable.Rows[0]["1.0.15.8.2.255"].ToString()),
            last_kwh_reading_total = Decimal.Parse(dataTable.Rows[dataTable.Rows.Count - 1]["1.0.15.8.0.255"].ToString()),
            last_kwh_reading_t1 = Decimal.Parse(dataTable.Rows[dataTable.Rows.Count - 1]["1.0.15.8.1.255"].ToString()),
            last_kwh_reading_t2 = Decimal.Parse(dataTable.Rows[dataTable.Rows.Count - 1]["1.0.15.8.2.255"].ToString())
          }
        };
            }
            catch (Exception ex)
            {
                return new List<BillGen>()
        {
          new BillGen() { customer_info = ex.ToString() }
        };
            }
        }
    }
}
