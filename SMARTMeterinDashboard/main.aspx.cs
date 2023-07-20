using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Configuration;
using System.Net;
using System.Data.Odbc;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WebApplication1
{
    public partial class main : System.Web.UI.Page
    {
        protected string myCString = "3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8";
        protected string kWhJSON = "3, 0, 0, 0, 0";
        protected string kWJSON = "0, 4, 0, 0, 0";
        protected string kVARhJSON = "0, 0, 5, 0, 0";
        protected string kVARJSON = "0, 0, 0, 0, 6";
        public enum MessageType { Success, Error, Info, Warning };
        protected void Page_Load(object sender, EventArgs e)
        {
            MainScript.RegisterAsyncPostBackControl(deviceGroupCreateButton);
            MainScript.RegisterAsyncPostBackControl(LinkButtonGroupSearch);
            MainScript.RegisterAsyncPostBackControl(LinkButtonEditGroup);
            MainScript.RegisterAsyncPostBackControl(LinkButtonEditGroupSave);
            MainScript.RegisterAsyncPostBackControl(LinkButtonEditGroupCancel);
            MainScript.RegisterAsyncPostBackControl(LinkButtonRefreshParents);
            MainScript.RegisterAsyncPostBackControl(SearchForGroupsAlloLinkButton);
            MainScript.RegisterAsyncPostBackControl(LinkButtonGroupsAlloToMeter);
            MainScript.RegisterAsyncPostBackControl(LinkButtonRefreshTree);
            MainScript.RegisterAsyncPostBackControl(LinkButtonHealthReport);
            MainScript.RegisterAsyncPostBackControl(LinkButtonTreeSearch);
            MainScript.RegisterAsyncPostBackControl(LinkButtonStatGraphGen);
            MainScript.RegisterAsyncPostBackControl(addThisMeter);
            MainScript.RegisterAsyncPostBackControl(LinkButtonLoadRecentAlarms);
            MainScript.RegisterAsyncPostBackControl(btnaccessT);
            MainScript.RegisterAsyncPostBackControl(btnactionT);
            MainScript.RegisterAsyncPostBackControl(btncommT);

            
            MainScript.RegisterAsyncPostBackControl(LinkButtonAddAutoMeter);
            MainScript.RegisterAsyncPostBackControl(LinkButtonAutoRegSearch);
            MainScript.RegisterAsyncPostBackControl(configure);
            MainScript.RegisterAsyncPostBackControl(LinkButtonAddPermissions);
            MainScript.RegisterAsyncPostBackControl(LinkButtonRemoveUser);
            MainScript.RegisterAsyncPostBackControl(LinkButtonAddUser);
            MainScript.RegisterAsyncPostBackControl(ftpLinkButtonGenerateBatchFiles);
            MainScript.RegisterAsyncPostBackControl(deleteImeter);
            MainScript.RegisterAsyncPostBackControl(meterRemove);
            MainScript.RegisterAsyncPostBackControl(goose);
            

            try
            {
                if (!((bool)(Session["bool"])))
                {
                    Response.Redirect("AmiLogin.aspx");
                }
            }
            catch (Exception) { Response.Redirect("AmiLogin.aspx"); }

            try
            {
                Session["bool"] = true;
                string fullname = "";
                if (typ.Text.Length < 1)
                {
                    string type = "";
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT * FROM users1 where keycode = '" + Request.QueryString["t"] + "';", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                type = (string)(dr["typ"]);
                                Session["asset"] = int.Parse(dr["asset"].ToString());
                                Session["load"] = int.Parse(dr["loadManage"].ToString());
                                Session["archive"] = int.Parse(dr["archive"].ToString());
                                Session["reading"] = int.Parse(dr["reading"].ToString());
                                Session["parameter"] = int.Parse(dr["parameter"].ToString());
                                fullname = (string)(dr["name"]);
                                usernameAccEdit.Value = dr["username"].ToString();
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    typ.Text = type;
                    Session["type"] = type;
                    Session["fullname"] = fullname;

                    if ((int)(Session["asset"]) == 0)
                    {
                        Ul1.Visible = false;
                        GridviewEventConfiguration.Enabled = false;
                        //manreg.Enabled = false;
                        //autoreg.Enabled = false;
                        //onlineMtrs.Enabled = false;
                        //ButtonAssetUpdate.Enabled = false;
                    }

                    if ((int)(Session["load"]) == 0)
                    {
                        //loadcnt.Enabled = false;
                    }

                    if ((int)(Session["archive"]) == 0)
                    {
                        Ul3.Visible = false;
                        //usgstats.Enabled = false;
                        //arcview.Enabled = false;
                        //fileexp.Enabled = false;
                    }

                    if ((int)(Session["reading"]) == 0)
                    {
                        Ul5.Visible = false;
                        
                    }

                    if ((int)(Session["parameter"]) == 0)
                    {
                        
                    }

                    if (type != "Admin")
                    {
                        Ul4.Visible = false;
                        
                    }

                    if (!(type == "Admin" || type == "User" || type == "Power User"))
                    {
                        Response.Redirect("AmiLogin.aspx");
                    }
                }

                if (!IsPostBack)
                {
                    loader();

                    LoadDropDownList();
                    BindData();
                    Loadmain2();
                    BindDatac();
                    populateEventGrid();
                    populateAlarmsGrid();

                    fillAssetGrid();
                    //loadfiles();
                    loadsmetersf();
                    


                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Source;
            }
        }

        private void ExecuteNonQurey(string query)
        {
            DBGetSet db = new DBGetSet();
            db.Query = query;
            db.ExecuteNonQuery();
        }

        private DataTable ExecuteReader(string query)
        {
            DBGetSet db = new DBGetSet();
            db.Query = query;
            return db.ExecuteReader();
        }

        private void BindData()
        {
            DBGetSet Db = new DBGetSet();
            Db.Query = "select * from exportbatch;";
            DataTable dt = Db.ExecuteReader();
            batches.Items.Clear();
            foreach(DataRow dr in dt.Rows)
            {
                batches.Items.Add(dr["BatchID"].ToString());
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private void BindDatac()
        {
            DBGetSet Db = new DBGetSet();
            Db.Query = "select * from customergroups;";
            DataTable dt = Db.ExecuteReader();
            GridView2.DataSource = dt;
            GridView2.DataBind();
        }

        protected void fillAssetGrid()
        {
            DataTable dt;
            dt = new DataTable();
            dt.Columns.Add("Serial");
            dt.Columns.Add("SIM Number");
            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("SELECT * FROM meter order by subsubsubutility;", connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DataRow drr = dt.NewRow();
                        drr["Serial"] = dr["subsubsubutility"].ToString();
                        drr["SIM Number"] = dr["phone"].ToString();
                        dt.Rows.Add(drr);
                    }
                    dr.Close();
                }
                connection.Close();
            }

        }


        void loader()
        {
            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("SELECT COUNT(DISTINCT utility) AS utility, COUNT(DISTINCT subutility) AS sutility, COUNT(DISTINCT subsubsubutility) AS subsubsubutility FROM dingrail.meter;", connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int utilityCount = Convert.ToInt32(dr["utility"]);

                        int sUtilityCount = Convert.ToInt32(dr["sutility"]);
                        int subsubutilityCount = Convert.ToInt32(dr["subsubsubutility"]);

                        // set values of paragraph tags with ids pM0, pNoD, and NoSM
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "document.getElementById('pM0').innerHTML = '" + utilityCount + "';" +
                            "document.getElementById('pNoD').innerHTML = '" + sUtilityCount + "';" +
                            "document.getElementById('NoSM').innerHTML = '" + subsubutilityCount + "';", true);
                    }
                    dr.Close();
                }
                connection.Close();
            }
        }
        protected void eventLogPopulate()
        {

            string query = "SELECT * FROM eventlist1 where (eventCode != 119 and eventCode != 149)  order by Time_stamp desc limit 1000;";
            if (res.Value != "" && IsDigitsOnly(res.Value))
            {
                query = "SELECT * FROM eventlist1 where msn = '" + res.Value + "' and (eventCode != 119 and eventCode != 149)  order by Time_stamp desc limit 2000;";
            }

            string t = "[";
            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand(query, connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        t += "[";
                        t += "'" + dr["msn"].ToString() + "',";
                        t += "'" + dr["eventCode"].ToString() + "',";
                        t += "'" + eventCodeTranslation(dr["eventCode"].ToString()) + "',";
                        t += "'" + dr["Time_stamp"].ToString() + "'";
                        t += "],";
                    }
                    dr.Close();
                }
                connection.Close();
            }

            t = ReplaceAt(t, t.Length - 1, ']');

            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addingRowsE(" + t + ");", true);
        }

        protected void AccessLogLogPopulate()
        {
            string temp = "[";

            DataTable dt;
            dt = new DataTable();
            dt.Columns.Add("Username");
            dt.Columns.Add("Occurence");
            dt.Columns.Add("ClientIP");

            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("SELECT * FROM systemaccesslog order by Time_stamp desc;", connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DataRow drr = dt.NewRow();
                        drr["Username"] = dr["username"].ToString();
                        drr["Occurence"] = ((DateTime)(dr["Time_stamp"])).ToString("yyyy/MM/dd HH:mm:ss");
                        drr["ClientIP"] = dr["clientIP"].ToString();
                        dt.Rows.Add(drr);
                        temp += "['" + dr["username"].ToString() + "', '" + ((DateTime)(dr["Time_stamp"])).ToString("yyyy/MM/dd HH:mm:ss") + "','"+dr["clientIP"].ToString()+"'],";
                
                    }
                    dr.Close();
                }
                connection.Close();
            }

            temp = ReplaceAt(temp, temp.Length - 1, ']');

            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addingaccessT(" + temp + ");", true);
        }

        protected void ActionsLogLogPopulate()
        {
            string temp = "[";

            DataTable dt;
            dt = new DataTable();
            dt.Columns.Add("Username");
            dt.Columns.Add("Activity");
            dt.Columns.Add("Meter Number");
            dt.Columns.Add("Time stamp");

            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("SELECT * FROM dingrail.actionslog order by Time_stamp desc;", connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        DataRow drr = dt.NewRow();
                        drr["Username"] = dr["Username"].ToString();
                        drr["Activity"] = dr["action"].ToString();
                        drr["Meter Number"] = dr["meter_no"].ToString();
                        drr["Time stamp"] = ((DateTime)(dr["Time_stamp"])).ToString("yyyy/MM/dd HH:mm:ss");
                        dt.Rows.Add(drr);

                        temp += "['" + dr["username"].ToString() + "','" + dr["action"].ToString() + "','" + dr["meter_no"].ToString() + "','" + ((DateTime)(dr["Time_stamp"])).ToString("yyyy/MM/dd HH:mm:ss") + "'],";


                    }
                    dr.Close();
                }
                connection.Close();
            }
            temp = ReplaceAt(temp, temp.Length - 1, ']');
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addingactionT(" + temp + ");", true);
        }

        protected void CommunicationsLogLogPopulate()
        {
            string temp = "[";

            DataTable dt;
            dt = new DataTable();
            dt.Columns.Add("Meter Serial");
            dt.Columns.Add("Activity");
            dt.Columns.Add("Packet");
            dt.Columns.Add("Time Stamp");

            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("SELECT * FROM communicationlog order by Time_stamp desc;", connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DataRow drr = dt.NewRow();
                        drr["Meter Serial"] = dr["serial"].ToString();
                        drr["Activity"] = dr["action"].ToString();
                        drr["Packet"] = dr["data"].ToString();
                        drr["Time Stamp"] = ((DateTime)(dr["Time_stamp"])).ToString("yyyy/MM/dd HH:mm:ss");
                        dt.Rows.Add(drr);
                        temp += "['" + dr["serial"].ToString() + "','" + dr["action"].ToString() + "','" + dr["data"].ToString() + "','" + ((DateTime)(dr["Time_stamp"])).ToString("yyyy/MM/dd HH:mm:ss") + "'],";

                    }
                    dr.Close();
                }
                connection.Close();
            }

            temp = ReplaceAt(temp, temp.Length - 1, ']');
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addingcommT(" + temp + ");", true);
        }

    
        protected void populateAlarmsGrid()
        {
            DataTable dt;
            dt = new DataTable();
            dt.Columns.Add("Event Code");
            dt.Columns.Add("Description");
            dt.Columns.Add("Alarm Status");
            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("SELECT * FROM alarms1;", connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DataRow drr = dt.NewRow();
                        drr["Event Code"] = dr["code"].ToString();
                        drr["Description"] = eventCodeTranslation(dr["code"].ToString());
                        drr["Alarm Status"] = MajorOrMinor(dr["major"].ToString());
                        dt.Rows.Add(drr);
                    }
                    dr.Close();
                }
                connection.Close();
            }
            GridViewAlarmsConfig.Font.Size = FontUnit.Smaller;
            GridViewAlarmsConfig.DataSource = dt;
            GridViewAlarmsConfig.DataBind();
        }

        protected string MajorOrMinor(string val)
        {
            if(val == "0")
            {
                return "Non Critical";
            }
            return "Critical";
        }

        protected void populateEventGrid()
        {
            DataTable dt;
            dt = new DataTable();
            dt.Columns.Add("Serial");
            dt.Columns.Add("Alert Dispatch Email ID");
            dt.Columns.Add("Alert Dispatch SMS Number");

            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("SELECT * FROM meter1 order by serial;", connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DataRow drr = dt.NewRow();
                        drr["Serial"] = dr["serial"].ToString();
                        drr["Alert Dispatch Email ID"] = dr["emailAlert"].ToString();
                        drr["Alert Dispatch SMS Number"] = dr["smsAlert"].ToString();
                        dt.Rows.Add(drr);
                    }
                    dr.Close();
                }
                connection.Close();
            }
            GridviewEventConfiguration.DataSource = dt;
            GridviewEventConfiguration.DataBind();
        }
        public void Loadmain2()
        {
            try
            {
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();

                    using (OdbcCommand command = new OdbcCommand("SELECT CONCAT (ROUND(SUM(latest_consumption), 3), ' kWh') AS TotalConsumption, CONCAT(ROUND((SELECT SUM(latest_consumption)FROM (SELECT meter_no, total_active_energy AS latest_consumption FROM dingrail.meter_data WHERE meter_no IN (SELECT DISTINCT subsubsubutility FROM dingrail.meter) AND (meter_no, id) IN (SELECT meter_no, MAX(id) AS max_id FROM dingrail.meter_data WHERE meter_no IN (SELECT DISTINCT subsubsubutility FROM dingrail.meter) GROUP BY meter_no)) AS latest_readings),3),' kWh') AS MonthlyQuantity, CONCAT(ROUND((SELECT SUM(latest_consumption) FROM (SELECT meter_no, total_active_energy AS latest_consumption FROM dingrail.meter_data WHERE meter_no IN (SELECT DISTINCT subsubsubutility FROM dingrail.meter) AND (meter_no, id) IN ( SELECT meter_no, MAX(id) AS max_id FROM dingrail.meter_data WHERE meter_no IN (SELECT DISTINCT subsubsubutility FROM dingrail.meter) GROUP BY meter_no) AND (Time_Stamp) = (SELECT MAX(Time_Stamp)FROM dingrail.meter_data))AS latest_readings),3),' kWh' ) AS DailyQuantity, CONCAT(ROUND(MIN(combine_power_factor), 3), '') AS MinimumPowerFactor, CONCAT(ROUND(MAX(combine_power_factor), 3), '') AS MaximumPowerFactor FROM ( SELECT meter_no, total_active_energy AS latest_consumption, combine_power_factor FROM dingrail.meter_data WHERE meter_no IN (SELECT DISTINCT subsubsubutility FROM dingrail.meter) AND (meter_no, Time_stamp) IN (SELECT meter_no, MIN(Time_stamp) AS Time_stamp FROM dingrail.meter_data WHERE meter_no IN (SELECT DISTINCT subsubsubutility FROM dingrail.meter) GROUP BY meter_no)) AS latest_readings;", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("TotalConsumption");
                        dt.Columns.Add("MonthlyQuantity");
                        dt.Columns.Add("DailyQuantity");
                        dt.Columns.Add("MinimumPowerFactor");
                        dt.Columns.Add("MaximumPowerFactor");

                        while (dr.Read())
                        {
                            DataRow drr = dt.NewRow();
                            drr["TotalConsumption"] = dr["TotalConsumption"].ToString();
                            drr["MonthlyQuantity"] = dr["MonthlyQuantity"].ToString();
                            drr["DailyQuantity"] = dr["DailyQuantity"].ToString();
                            drr["MinimumPowerFactor"] = dr["MinimumPowerFactor"].ToString();
                            drr["MaximumPowerFactor"] = dr["MaximumPowerFactor"].ToString();

                            dt.Rows.Add(drr);
                        }

                        // Assign the first row of the DataTable to the <p> elements
                        if (dt.Rows.Count > 0)
                        {
                            CPC.InnerText = dt.Rows[0]["TotalConsumption"].ToString();
                            Mtqoe.InnerText = dt.Rows[0]["MonthlyQuantity"].ToString();
                            Dtqoe.InnerText = dt.Rows[0]["DailyQuantity"].ToString();
                            MinPaf.InnerText = dt.Rows[0]["MinimumPowerFactor"].ToString();
                            MaxPF.InnerText = dt.Rows[0]["MaximumPowerFactor"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error); errLog.Text = ex.Message;
            }
        }

        protected void populateTreeView()
        {
            DataTable dt;
            dt = new DataTable();
            dt.Columns.Add("Serial");
            dt.Columns.Add("Connection");
            dt.Columns.Add("Relay Status");
            dt.Columns.Add("Last Keep-Alive");
            dt.Columns.Add("Last Read");
            dt.Columns.Add("Customer Code");
            dt.Columns.Add("SIM Number");
            dt.Columns.Add("Area");
            dt.Columns.Add("Sub Area");
            dt.Columns.Add("Lat");
            dt.Columns.Add("Long");
            dt.Columns.Add("Rssi");
            dt.Columns.Add("Firmware Version");
            dt.Columns.Add("Mode");
            dt.Columns.Add("Tech");
            dt.Columns.Add("Plan");

            TreeNode LESCO = new TreeNode();
            TreeNode area = new TreeNode();
            TreeNode subarea = new TreeNode();
            TreeNode connected = new TreeNode();
            TreeNode disconnected = new TreeNode();
            TreeNode MuteMeters = new TreeNode();
            int conn = 0;
            int disconn = 0;
            int mute = 0;
            string users = "[";
            string ausers = "[";
            string dusers = "[";

            string userst = "[";
            string auserst = "[";
            string duserst = "[";
            /* int areacount = 0;
             string[] AreaList = new string[1000];
             string[] SubAreaList = new string[1000];*/
            string query = "SELECT * FROM meter1 where deviceType = 'Three Phase' order by serial";
            string place = "Utility Co";

            if ((string)(Session["fullname"]) == "Qasim Moosvi")
            {
                query = "SELECT * FROM meter1 where serial = '3098163162' and deviceType = 'Three Phase';";
                place = "Utility Co";
            }

            if ((string)(Session["fullname"]) == "MEPCO")
            {
                query = "SELECT * FROM meter1 where serial = '3098000170' and deviceType = 'Three Phase';";
            }

            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand(query, connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DataRow drr = dt.NewRow();
                        drr["Serial"] = dr["serial"].ToString();
                        drr["Connection"] = dr["connected"].ToString();
                        drr["Relay Status"] = dr["relayStatus"].ToString();
                        drr["Last Keep-Alive"] = dr["times"].ToString();
                        drr["Last Read"] = ((DateTime)(dr["lastRead"])).ToString();
                        drr["Customer Code"] = dr["customerCode"].ToString();
                        drr["SIM Number"] = dr["phone"].ToString();
                        drr["Area"] = dr["area"].ToString();
                        drr["Sub Area"] = dr["subarea"].ToString();
                        drr["Lat"] = dr["lat"].ToString();
                        drr["Long"] = dr["longituge"].ToString();
                        drr["Rssi"] = dr["rssi"].ToString();
                        drr["Firmware Version"] = dr["firmwareVersion"].ToString();
                        drr["Tech"] = dr["tech"].ToString();
                        if (dr["mode"].ToString() == "0")
                        {
                            drr["Mode"] = "Mode 1";
                        }
                        else
                        {
                            drr["Mode"] = "Mode 2";
                        }
                        drr["Plan"] = dr["loadshedding"].ToString();
                        dt.Rows.Add(drr);
                    }
                    dr.Close();
                }
                connection.Close();
            }

            foreach (DataRow dr in dt.Rows)
            {
                DateTime lr = DateTime.Parse(dr["Last Read"].ToString());
                if (dr["Connection"].ToString() == "1")
                {
                    TreeNode properteea = new TreeNode(dr["Serial"].ToString());
                    ausers += "'" + dr["Serial"].ToString() + "',";
                    properteea.ToolTip = "Connected. Last Seen :" + dr["Last Keep-Alive"].ToString() + ". <br/>Last Read :" + dr["Last Read"].ToString() + "<br/>Firmware Version:" + dr["Firmware Version"].ToString();
                    properteea.Expanded = false;
                    if (dr["Rssi"].ToString() == "1")
                    {
                        properteea.ImageUrl = "low.png";
                        properteea.ToolTip += ". <br/>Signal Strength: Poor";
                    }
                    if (dr["Rssi"].ToString() == "2")
                    {
                        properteea.ImageUrl = "weak.png";
                        properteea.ToolTip += ". <br/>Signal Strength: Weak";
                    }
                    if (dr["Rssi"].ToString() == "3")
                    {
                        properteea.ImageUrl = "good.png";
                        properteea.ToolTip += ". <br/>Signal Strength: Good";
                    }
                    if (dr["Rssi"].ToString() == "4")
                    {
                        properteea.ImageUrl = "vgood.png";
                        properteea.ToolTip += ". <br/>Signal Strength: Excellent";
                    }
                    if (dr["Rssi"].ToString() == "5")
                    {
                        properteea.ImageUrl = "full.png";
                        properteea.ToolTip += ". <br/>Signal Strength: Max";
                    }
                    properteea.ToolTip += "<br/>Carrier Technology: " + dr["Tech"].ToString();
                    properteea.ToolTip += "<br/>Latitude:" + dr["Lat"].ToString() + " Longitude:" + dr["Long"].ToString();
                    properteea.ToolTip += "<br/>SIM Number:" + dr["SIM Number"].ToString() + "<br/>Customer Code:" + dr["Customer Code"].ToString();
                    if (dr["Relay Status"].ToString() == "0")
                    {
                        properteea.ToolTip += "<br/>Relay Status: Relay Disconnected";
                    }
                    else if (dr["Relay Status"].ToString() == "1")
                    {
                        properteea.ToolTip += "<br/>Relay Status: Relay Connected";
                    }
                    else
                    {
                        properteea.ToolTip += "<br/>Relay Status: Ready to Reconnect";
                    }
                    properteea.ToolTip += "<br/>Communication Mode: " + dr["Mode"];
                    properteea.ToolTip += "<br/>Communication Protocol: DLMS";
                    properteea.ToolTip += "<br/>Active Load Managemnet Plan: " + dr["Plan"];
                    connected.ChildNodes.Add(properteea);

                    auserst += "'" + properteea.ToolTip + "',";

                    conn++;
                }
                else
                {
                    DateTime DT = DateTime.Parse(dr["Last Read"].ToString());
                    double diff = (DateTime.Now - DT).TotalDays;

                    if (diff < 1)
                    {
                        TreeNode properteea = new TreeNode(dr["Serial"].ToString());
                        dusers += "'" + dr["Serial"].ToString() + "',";
                        properteea.ToolTip = "Waiting for device to Reconnect!<br/>Disconnected. Last Online: " + dr["Last Keep-Alive"].ToString() + ". <br/>Last Read :" + dr["Last Read"].ToString() + "<br/>Firmware Version:" + dr["Firmware Version"].ToString();
                        properteea.Expanded = false;
                        if (dr["Rssi"].ToString() == "1")
                        {
                            properteea.ImageUrl = "low.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Poor";
                        }
                        if (dr["Rssi"].ToString() == "2")
                        {
                            properteea.ImageUrl = "weak.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Weak";
                        }
                        if (dr["Rssi"].ToString() == "3")
                        {
                            properteea.ImageUrl = "good.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Good";
                        }
                        if (dr["Rssi"].ToString() == "4")
                        {
                            properteea.ImageUrl = "vgood.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Excellent";
                        }
                        if (dr["Rssi"].ToString() == "5")
                        {
                            properteea.ImageUrl = "full.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Max";
                        }
                        properteea.ToolTip += "<br/>Carrier Technology: " + dr["Tech"].ToString();
                        properteea.ToolTip += "<br/>Latitude:" + dr["Lat"].ToString() + " Longitude:" + dr["Long"].ToString();
                        properteea.ToolTip += "<br/>SIM Number:" + dr["SIM Number"].ToString() + "<br/>Customer Code:" + dr["Customer Code"].ToString();
                        if (dr["Relay Status"].ToString() == "0")
                        {
                            properteea.ToolTip += "<br/>Relay Status: Relay Disconnected";
                        }
                        else if (dr["Relay Status"].ToString() == "1")
                        {
                            properteea.ToolTip += "<br/>Relay Status: Relay Connected";
                        }
                        else
                        {
                            properteea.ToolTip += "<br/>Relay Status: Ready to Reconnect";
                        }
                        properteea.ToolTip += "<br/>Communication Mode: " + dr["Mode"];
                        properteea.ToolTip += "<br/>Communication Protocol: DLMS";
                        properteea.ToolTip += "<br/>Active Load Managemnet Plan: " + dr["Plan"];
                        disconnected.ChildNodes.Add(properteea);

                        duserst += "'" + properteea.ToolTip + "',";

                        disconn++;
                    }
                    else
                    {
                        TreeNode properteea = new TreeNode(dr["Serial"].ToString());
                        users += "'" + dr["Serial"].ToString() + "',";
                        properteea.ToolTip = "Attention! Device Mute.Investigation Required.<br/>Disconnected. Last Online: " + dr["Last Keep-Alive"].ToString() + ". <br/>Last Read :" + dr["Last Read"].ToString() + "<br/>Firmware Version:" + dr["Firmware Version"].ToString();
                        properteea.Expanded = false;
                        if (dr["Rssi"].ToString() == "1")
                        {
                            properteea.ImageUrl = "low.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Poor";
                        }
                        if (dr["Rssi"].ToString() == "2")
                        {
                            properteea.ImageUrl = "weak.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Weak";
                        }
                        if (dr["Rssi"].ToString() == "3")
                        {
                            properteea.ImageUrl = "good.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Good";
                        }
                        if (dr["Rssi"].ToString() == "4")
                        {
                            properteea.ImageUrl = "vgood.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Excellent";
                        }
                        if (dr["Rssi"].ToString() == "5")
                        {
                            properteea.ImageUrl = "full.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Max";
                        }
                        properteea.ToolTip += "<br/>Carrier Technology: " + dr["Tech"].ToString();
                        properteea.ToolTip += "<br/>Latitude:" + dr["Lat"].ToString() + " Longitude:" + dr["Long"].ToString();
                        properteea.ToolTip += "<br/>SIM Number:" + dr["SIM Number"].ToString() + "<br/>Customer Code:" + dr["Customer Code"].ToString();
                        if (dr["Relay Status"].ToString() == "0")
                        {
                            properteea.ToolTip += "<br/>Relay Status: Disconnected";
                        }
                        else if (dr["Relay Status"].ToString() == "1")
                        {
                            properteea.ToolTip += "<br/>Relay Status: Connected";
                        }
                        else
                        {
                            properteea.ToolTip += "<br/>Relay Status: Ready to Reconnect";
                        }
                        properteea.ToolTip += "<br/>Communication Mode: " + dr["Mode"];
                        properteea.ToolTip += "<br/>Communication Protocol: DLMS";
                        properteea.ToolTip += "<br/>Active Load Managemnet Plan: " + dr["Plan"];
                        MuteMeters.ChildNodes.Add(properteea);

                        userst += "'" + properteea.ToolTip + "',";

                        mute++;
                    }
                }
            }

            connected.Text = "Connected (" + conn + ")";
            connected.ImageUrl = "ok.png";
            disconnected.Text = "Disconnected (" + disconn + ")";
            disconnected.ImageUrl = "mute.png";
            MuteMeters.Text = "Mute Devices (" + mute + ")";
            MuteMeters.ImageUrl = "NotOk.png";
            MuteMeters.Expanded = false;
            LESCO.Text = place + "(" + (conn + disconn + mute) + ")";
            area.Text = "Total(" + (conn + disconn + mute) + ")";
            subarea.Text = "Total(" + (conn + disconn + mute) + ")";

            subarea.ChildNodes.Add(connected);
            subarea.ChildNodes.Add(disconnected);
            subarea.ChildNodes.Add(MuteMeters);
            subarea.ImageUrl = "home.png";
            area.ChildNodes.Add(subarea);
            LESCO.ChildNodes.Add(subarea);
            LESCO.Expanded = true;
            LESCO.ImageUrl = "office.png";
            area.Expanded = true;
            subarea.Expanded = true;

            //TreeViewMeterNodes.Nodes.Add(LESCO);

            if (mute < 1)
            {
                users += "'None']";
                userst += "'None']";
            }
            else
            {
                users = ReplaceAt(users, users.Length - 1, ']');
                userst = ReplaceAt(userst, userst.Length - 1, ']');
            }

            if (conn < 1)
            {
                ausers += "'None']";
                auserst += "'None']";
            }
            else
            {
                ausers = ReplaceAt(ausers, ausers.Length - 1, ']');
                auserst = ReplaceAt(auserst, auserst.Length - 1, ']');
            }

            if (disconn < 1)
            {
                dusers += "'None']";
                duserst += "'None']";
            }
            else
            {
                dusers = ReplaceAt(dusers, dusers.Length - 1, ']');
                duserst = ReplaceAt(duserst, duserst.Length - 1, ']');
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addTouu(" + users + "," + ausers + "," + dusers + "," + userst + "," + auserst + "," + duserst + ",'Total Devices(" + (conn + disconn + mute) + ")','Distribution');", true);
        }

        void customSortTree(string soo, string type)
        {
            TreeNode[] customgroups = new TreeNode[1000];
            TreeNode CircO = new TreeNode();
            CircO.ImageUrl = "cgroup.png";

            string users = "[";
            string ausers = "[";
            string dusers = "[";

            string userst = "[";
            string auserst = "[";
            string duserst = "[";

            int noofcustomgroups = 0;

            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("select * from devicegroups where category = '" + soo + "';", connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        customgroups[noofcustomgroups] = new TreeNode(dr["id"].ToString());
                        customgroups[noofcustomgroups].ChildNodes.Add(new TreeNode("Connected"));
                        customgroups[noofcustomgroups].ChildNodes.Add(new TreeNode("Disconnected"));
                        customgroups[noofcustomgroups].ChildNodes.Add(new TreeNode("Mute Devices"));

                        customgroups[noofcustomgroups].ChildNodes[0].ImageUrl = "Ok.png";
                        customgroups[noofcustomgroups].ChildNodes[1].ImageUrl = "mute.png";
                        customgroups[noofcustomgroups].ChildNodes[2].ImageUrl = "NotOk.png";
                        customgroups[noofcustomgroups].ImageUrl = "b.png";
                        CircO.ChildNodes.Add(customgroups[noofcustomgroups]);
                        noofcustomgroups++;
                    }
                    dr.Close();
                }
                connection.Close();
            }

            DataTable dt;
            dt = new DataTable();
            dt.Columns.Add("Serial");
            dt.Columns.Add("Connection");
            dt.Columns.Add("Relay Status");
            dt.Columns.Add("Last Keep-Alive");
            dt.Columns.Add("Last Read");
            dt.Columns.Add("Customer Code");
            dt.Columns.Add("SIM Number");
            dt.Columns.Add("Area");
            dt.Columns.Add("Sub Area");
            dt.Columns.Add("Lat");
            dt.Columns.Add("Long");
            dt.Columns.Add("Rssi");
            dt.Columns.Add("Firmware Version");
            dt.Columns.Add("Mode");
            dt.Columns.Add("Tech");
            dt.Columns.Add("deviceGroup");
            dt.Columns.Add("userDeviceGroup");
            dt.Columns.Add("Plan");
            dt.Columns.Add("ExportBatch");

            TreeNode LESCO = new TreeNode();
            TreeNode area = new TreeNode();
            TreeNode subarea = new TreeNode();
            TreeNode connected = new TreeNode();
            TreeNode disconnected = new TreeNode();
            TreeNode MuteMeters = new TreeNode();
            int conn = 0;
            int disconn = 0;
            int mute = 0;
            string query = "SELECT * FROM meter1 where deviceType = '" + type + "' order by serial;";
            string place = "LESCO";

            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand(query, connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DataRow drr = dt.NewRow();
                        drr["Serial"] = dr["serial"].ToString();
                        drr["Connection"] = dr["connected"].ToString();
                        drr["Relay Status"] = dr["relayStatus"].ToString();
                        drr["Last Keep-Alive"] = dr["times"].ToString();
                        drr["Last Read"] = ((DateTime)(dr["lastRead"])).ToString();
                        drr["Customer Code"] = dr["customerCode"].ToString();
                        drr["SIM Number"] = dr["phone"].ToString();
                        drr["Area"] = dr["area"].ToString();
                        drr["Sub Area"] = dr["subarea"].ToString();
                        drr["Lat"] = dr["lat"].ToString();
                        drr["Long"] = dr["longituge"].ToString();
                        drr["Rssi"] = dr["rssi"].ToString();
                        drr["Firmware Version"] = dr["firmwareVersion"].ToString();
                        drr["Tech"] = dr["tech"].ToString();
                        drr["ExportBatch"] = dr["exportbatch"];
                        if (dr["mode"].ToString() == "0")
                        {
                            drr["Mode"] = "Mode 1";
                        }
                        else
                        {
                            drr["Mode"] = "Mode 2";
                        }
                        drr["deviceGroup"] = dr["deviceGroup"].ToString();
                        drr["userDeviceGroup"] = dr["userDeviceGroup"].ToString();
                        drr["Plan"] = dr["loadshedding"].ToString();
                        dt.Rows.Add(drr);
                    }
                    dr.Close();
                }
                connection.Close();
            }

            TreeNode unsung = new TreeNode("Unsorted");
            unsung.ChildNodes.Add(new TreeNode("Connected"));
            unsung.ChildNodes[0].ImageUrl = "ok.png";
            unsung.ChildNodes.Add(new TreeNode("Disconnected"));
            unsung.ChildNodes[1].ImageUrl = "mute.png";
            unsung.ChildNodes.Add(new TreeNode("Mute Devices"));
            unsung.ChildNodes[2].ImageUrl = "NotOk.png";
            unsung.ImageUrl = "b.png";

            string saa = "userDeviceGroup";

            switch (soo)
            {
                case "Circle":
                    saa = "Area";
                    break;
                case "Feeder":
                    saa = "Sub Area";
                    break;
                case "Transformer":
                    saa = "deviceGroup";
                    break;
            }

            foreach (DataRow dr in dt.Rows)
            {
                int which = 0;
                bool isit = false;
                for (int i = 0; i < noofcustomgroups; i++)
                {
                    if (customgroups[i].Text == dr[saa].ToString())
                    {
                        which = i;
                        isit = true;
                    }
                }
                if (dr["Connection"].ToString() == "1")
                {
                    TreeNode properteea = new TreeNode(dr["Serial"].ToString());
                    properteea.ToolTip = "Connected. Last Seen :" + dr["Last Keep-Alive"].ToString() + ". <br/>Last Read :" + dr["Last Read"].ToString() + "<br/>Firmware Version:" + dr["Firmware Version"].ToString();
                    properteea.Expanded = false;
                    if (dr["Rssi"].ToString() == "1")
                    {
                        properteea.ImageUrl = "low.png";
                        properteea.ToolTip += ". <br/>Signal Strength: Poor";
                    }
                    if (dr["Rssi"].ToString() == "2")
                    {
                        properteea.ImageUrl = "weak.png";
                        properteea.ToolTip += ". <br/>Signal Strength: Weak";
                    }
                    if (dr["Rssi"].ToString() == "3")
                    {
                        properteea.ImageUrl = "good.png";
                        properteea.ToolTip += ". <br/>Signal Strength: Good";
                    }
                    if (dr["Rssi"].ToString() == "4")
                    {
                        properteea.ImageUrl = "vgood.png";
                        properteea.ToolTip += ". <br/>Signal Strength: Excellent";
                    }
                    if (dr["Rssi"].ToString() == "5")
                    {
                        properteea.ImageUrl = "full.png";
                        properteea.ToolTip += ". <br/>Signal Strength: Max";
                    }
                    properteea.ToolTip += "<br/>Carrier Technology: " + dr["Tech"].ToString();
                    properteea.ToolTip += "<br/>Latitude:" + dr["Lat"].ToString() + " Longitude:" + dr["Long"].ToString();
                    properteea.ToolTip += "<br/>SIM Number:" + dr["SIM Number"].ToString() + "<br/>Customer Code:" + dr["Customer Code"].ToString();
                    if (dr["Relay Status"].ToString() == "0")
                    {
                        properteea.ToolTip += "<br/>Relay Status: Relay Disconnected";
                    }
                    else if (dr["Relay Status"].ToString() == "1")
                    {
                        properteea.ToolTip += "<br/>Relay Status: Relay Connected";
                    }
                    else
                    {
                        properteea.ToolTip += "<br/>Relay Status: Ready to Reconnect";
                    }
                    properteea.ToolTip += "<br/>Communication Mode: " + dr["Mode"];
                    properteea.ToolTip += "<br/>Communication Protocol: DLMS";
                    properteea.ToolTip += "<br/>Active Load Managemnet Plan: " + dr["Plan"];
                    properteea.ToolTip += "<br/>Billing Batch: " + dr["ExportBatch"];
                    connected.ChildNodes.Add(properteea);
                    conn++;
                    if (!isit)
                    {
                        unsung.ChildNodes[0].ChildNodes.Add(properteea);

                    }
                    else
                    {
                        customgroups[which].ChildNodes[0].ChildNodes.Add(properteea);
                        customgroups[which].ChildNodes[0].ImageUrl = "ok.png";
                    }
                }
                else
                {
                    DateTime DT = DateTime.Parse(dr["Last Read"].ToString());
                    double diff = (DateTime.Now - DT).TotalDays;

                    if (diff < 1)
                    {
                        TreeNode properteea = new TreeNode(dr["Serial"].ToString());
                        properteea.ToolTip = "Waiting for device to Reconnect!<br/>Disconnected. Last Online: " + dr["Last Keep-Alive"].ToString() + ". <br/>Last Read :" + dr["Last Read"].ToString() + "<br/>Firmware Version:" + dr["Firmware Version"].ToString();
                        properteea.Expanded = false;
                        if (dr["Rssi"].ToString() == "1")
                        {
                            properteea.ImageUrl = "low.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Poor";
                        }
                        if (dr["Rssi"].ToString() == "2")
                        {
                            properteea.ImageUrl = "weak.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Weak";
                        }
                        if (dr["Rssi"].ToString() == "3")
                        {
                            properteea.ImageUrl = "good.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Good";
                        }
                        if (dr["Rssi"].ToString() == "4")
                        {
                            properteea.ImageUrl = "vgood.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Excellent";
                        }
                        if (dr["Rssi"].ToString() == "5")
                        {
                            properteea.ImageUrl = "full.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Max";
                        }
                        properteea.ToolTip += "<br/>Carrier Technology: " + dr["Tech"].ToString();
                        properteea.ToolTip += "<br/>Latitude:" + dr["Lat"].ToString() + " Longitude:" + dr["Long"].ToString();
                        properteea.ToolTip += "<br/>SIM Number:" + dr["SIM Number"].ToString() + "<br/>Customer Code:" + dr["Customer Code"].ToString();
                        if (dr["Relay Status"].ToString() == "0")
                        {
                            properteea.ToolTip += "<br/>Relay Status: Relay Disconnected";
                        }
                        else if (dr["Relay Status"].ToString() == "1")
                        {
                            properteea.ToolTip += "<br/>Relay Status: Relay Connected";
                        }
                        else
                        {
                            properteea.ToolTip += "<br/>Relay Status: Ready to Reconnect";
                        }
                        properteea.ToolTip += "<br/>Communication Mode: " + dr["Mode"];
                        properteea.ToolTip += "<br/>Communication Protocol: DLMS";
                        properteea.ToolTip += "<br/>Active Load Managemnet Plan: " + dr["Plan"];
                        properteea.ToolTip += "<br/>Billing Batch: " + dr["ExportBatch"];
                        disconnected.ChildNodes.Add(properteea);
                        disconn++;
                        if (!isit)
                        {
                            unsung.ChildNodes[1].ChildNodes.Add(properteea);
                        }
                        else
                        {
                            customgroups[which].ChildNodes[1].ChildNodes.Add(properteea);
                        }
                    }
                    else
                    {
                        TreeNode properteea = new TreeNode(dr["Serial"].ToString());
                        properteea.ToolTip = "Attention! Device Mute.Investigation Required.<br/>Disconnected. Last Online: " + dr["Last Keep-Alive"].ToString() + ". <br/>Last Read :" + dr["Last Read"].ToString() + "<br/>Firmware Version:" + dr["Firmware Version"].ToString();
                        properteea.Expanded = false;
                        if (dr["Rssi"].ToString() == "1")
                        {
                            properteea.ImageUrl = "low.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Poor";
                        }
                        if (dr["Rssi"].ToString() == "2")
                        {
                            properteea.ImageUrl = "weak.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Weak";
                        }
                        if (dr["Rssi"].ToString() == "3")
                        {
                            properteea.ImageUrl = "good.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Good";
                        }
                        if (dr["Rssi"].ToString() == "4")
                        {
                            properteea.ImageUrl = "vgood.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Excellent";
                        }
                        if (dr["Rssi"].ToString() == "5")
                        {
                            properteea.ImageUrl = "full.png";
                            properteea.ToolTip += ". <br/>Signal Strength: Max";
                        }
                        properteea.ToolTip += "<br/>Carrier Technology: " + dr["Tech"].ToString();
                        properteea.ToolTip += "<br/>Latitude:" + dr["Lat"].ToString() + " Longitude:" + dr["Long"].ToString();
                        properteea.ToolTip += "<br/>SIM Number:" + dr["SIM Number"].ToString() + "<br/>Customer Code:" + dr["Customer Code"].ToString();
                        if (dr["Relay Status"].ToString() == "0")
                        {
                            properteea.ToolTip += "<br/>Relay Status: Disconnected";
                        }
                        else if (dr["Relay Status"].ToString() == "1")
                        {
                            properteea.ToolTip += "<br/>Relay Status: Connected";
                        }
                        else
                        {
                            properteea.ToolTip += "<br/>Relay Status: Ready to Reconnect";
                        }
                        properteea.ToolTip += "<br/>Communication Mode: " + dr["Mode"];
                        properteea.ToolTip += "<br/>Communication Protocol: DLMS";
                        properteea.ToolTip += "<br/>Active Load Managemnet Plan: " + dr["Plan"];
                        properteea.ToolTip += "<br/>Billing Batch: " + dr["ExportBatch"];
                        MuteMeters.ChildNodes.Add(properteea);
                        mute++;

                        if (!isit)
                        {
                            unsung.ChildNodes[2].ChildNodes.Add(properteea);
                        }
                        else
                        {
                            customgroups[which].ChildNodes[2].ChildNodes.Add(properteea);
                        }
                    }
                }
            }

            CircO.ChildNodes.Add(unsung);
            for (int i = 0; i < CircO.ChildNodes.Count; i++)
            {
                int tup = 0;

                for (int j = 0; j < CircO.ChildNodes[i].ChildNodes.Count; j++)
                {
                    CircO.ChildNodes[i].ChildNodes[j].Text += "(" + CircO.ChildNodes[i].ChildNodes[j].ChildNodes.Count + ")";
                    tup += CircO.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                }

                CircO.ChildNodes[i].Text += "(" + tup + ")";
            }

            LESCO.Text = "Utility Co (" + (conn + disconn + mute) + ")";
            LESCO.Expanded = true;
            LESCO.ImageUrl = "home.png";
            CircO.Text = soo + "s(" + (conn + disconn + mute) + ")";
            /*area.Expanded = true;
            subarea.Expanded = true;*/
            CircO.Expanded = true;
            LESCO.ChildNodes.Add(CircO);

            for (int i = 0; i < noofcustomgroups; i++)
            {
                int a = 0, d = 0, m = 0;
                users = "[";
                ausers = "[";
                dusers = "[";

                userst = "[";
                auserst = "[";
                duserst = "[";

                foreach (TreeNode tu in customgroups[i].ChildNodes)
                {
                    foreach (TreeNode ta in tu.ChildNodes)
                    {
                        if (ta.Parent.Text.Contains("Connected"))
                        {
                            auserst += "'" + ta.ToolTip + "',";
                            ausers += "'" + ta.Text + "',";
                            a++;
                        }

                        if (ta.Parent.Text.Contains("Disconnected"))
                        {
                            dusers += "'" + ta.Text + "',";
                            duserst += "'" + ta.ToolTip + "',";
                            d++;
                        }

                        if (ta.Parent.Text.Contains("Mute"))
                        {
                            users += "'" + ta.Text + "',";
                            userst += "'" + ta.ToolTip + "',";
                            m++;
                        }
                    }
                }

                if (m < 1)
                {
                    users += "'None']";
                    userst += "'None']";
                }
                else
                {
                    users = ReplaceAt(users, users.Length - 1, ']');
                    userst = ReplaceAt(userst, userst.Length - 1, ']');
                }

                if (a < 1)
                {
                    ausers += "'None']";
                    auserst += "'None']";
                }
                else
                {
                    ausers = ReplaceAt(ausers, ausers.Length - 1, ']');
                    auserst = ReplaceAt(auserst, auserst.Length - 1, ']');
                }

                if (d < 1)
                {
                    dusers += "'None']";
                    duserst += "'None']";
                }
                else
                {
                    dusers = ReplaceAt(dusers, dusers.Length - 1, ']');
                    duserst = ReplaceAt(duserst, duserst.Length - 1, ']');
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addTouu(" + users + "," + ausers + "," + dusers + "," + userst + "," + auserst + "," + duserst + ",'Total Devices(" + (a + d + m) + ")','" + customgroups[i].Text + "');", true);
            }


            users = "[";
            ausers = "[";
            dusers = "[";

            userst = "[";
            auserst = "[";
            duserst = "[";
            int aa = 0, dd = 0, mm = 0;
            foreach (TreeNode tu in unsung.ChildNodes)
            {
                foreach (TreeNode ta in tu.ChildNodes)
                {
                    if (ta.Parent.Text.Contains("Connected"))
                    {
                        auserst += "'" + ta.ToolTip + "',";
                        ausers += "'" + ta.Text + "',";
                        aa++;
                    }

                    if (ta.Parent.Text.Contains("Disconnected"))
                    {
                        dusers += "'" + ta.Text + "',";
                        duserst += "'" + ta.ToolTip + "',";
                        dd++;
                    }

                    if (ta.Parent.Text.Contains("Mute"))
                    {
                        users += "'" + ta.Text + "',";
                        userst += "'" + ta.ToolTip + "',";
                        mm++;
                    }
                }
            }

            if (mm < 1)
            {
                users += "'None']";
                userst += "'None']";
            }
            else
            {
                users = ReplaceAt(users, users.Length - 1, ']');
                userst = ReplaceAt(userst, userst.Length - 1, ']');
            }

            if (aa < 1)
            {
                ausers += "'None']";
                auserst += "'None']";
            }
            else
            {
                ausers = ReplaceAt(ausers, ausers.Length - 1, ']');
                auserst = ReplaceAt(auserst, auserst.Length - 1, ']');
            }

            if (dd < 1)
            {
                dusers += "'None']";
                duserst += "'None']";
            }
            else
            {
                dusers = ReplaceAt(dusers, dusers.Length - 1, ']');
                duserst = ReplaceAt(duserst, duserst.Length - 1, ']');
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addTouu(" + users + "," + ausers + "," + dusers + "," + userst + "," + auserst + "," + duserst + ",'Total Devices(" + (aa + dd + mm) + ")','Unsorted Devices');", true);
        }

        protected void putOnATree(string srl)
        {
            bool f = false;
            double diff = 0;
            TreeNode circle = new TreeNode("Not Found(0)");
            TreeNode feeder = new TreeNode("Not Found(0)");
            TreeNode trafo = new TreeNode("Not Found(0)");
            TreeNode con = new TreeNode("Connected(0)");
            TreeNode discon = new TreeNode("Disconnected(0)");
            TreeNode mute = new TreeNode("Mute Device(0)");

            string users = "[";
            string ausers = "[";
            string dusers = "[";

            string userst = "[";
            string auserst = "[";
            string duserst = "[";
            int aa = 0, dd = 0, mm = 0;
            con.ImageUrl = "Ok.png";
            discon.ImageUrl = "mute.pmg";
            mute.ImageUrl = "NotOk.png";

            circle.ImageUrl = "home.png";
            feeder.ImageUrl = "b.png";
            trafo.ImageUrl = "b.png";

            int conee = 0;
            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("select * from meter1 where serial = '" + srl + "'", connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        circle.Text = dr["area"].ToString() + "(1)";
                        feeder.Text = dr["subarea"].ToString() + "(1)";
                        trafo.Text = dr["deviceGroup"].ToString() + "(1)";
                        conee = int.Parse(dr["connected"].ToString());
                        DateTime DT = DateTime.Parse(dr["lastRead"].ToString());
                        diff = (DateTime.Now - DT).TotalDays;
                        f = true;

                        if (conee == 1)
                        {
                            ausers += "'" + srl + "',";
                            auserst += "'Last-Reading:-" + DT.ToString("yyyy/M/dd") + "-" + DT.ToString("HH:mm:ss") + "',";
                            aa++;
                        }
                        else
                        {
                            if (diff > 1)
                            {
                                users += "'" + srl + "',";
                                userst += "'Last-Reading:-" + DT.ToString("yyyy/M/dd") + "-" + DT.ToString("HH:mm:ss") + "',";
                                mm++;
                            }
                            else if (f)
                            {
                                dusers += "'" + srl + "',";
                                duserst += "'Last-Reading:-" + DT.ToString("yyyy/M/dd") + "-" + DT.ToString("HH:mm:ss") + "',";
                                dd++;
                            }
                        }
                    }
                    dr.Close();
                }
                connection.Close();
            }



            if (mm < 1)
            {
                users += "'None']";
                userst += "'None']";
            }
            else
            {
                users = ReplaceAt(users, users.Length - 1, ']');
                userst = ReplaceAt(userst, userst.Length - 1, ']');
            }

            if (aa < 1)
            {
                ausers += "'None']";
                auserst += "'None']";
            }
            else
            {
                ausers = ReplaceAt(ausers, ausers.Length - 1, ']');
                auserst = ReplaceAt(auserst, auserst.Length - 1, ']');
            }

            if (dd < 1)
            {
                dusers += "'None']";
                duserst += "'None']";
            }
            else
            {
                dusers = ReplaceAt(dusers, dusers.Length - 1, ']');
                duserst = ReplaceAt(duserst, duserst.Length - 1, ']');
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addTouu(" + users + "," + ausers + "," + dusers + "," + userst + "," + auserst + "," + duserst + ",'" + trafo.Text + "','" + circle.Text + " - " + feeder.Text + "');", true);

        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

   
        protected void end_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("AmiLogin.aspx");
        }

        protected void refreshtree_Click(object sender, ImageClickEventArgs e)
        {
            populateTreeView();
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

        protected void btnLPRead_Click(object sender, EventArgs e)
        {
            try
            {
                if (res.Value != "" && IsDigitsOnly(res.Value) && LPr1.Value != "" && LPr2.Value != "")
                {
                    DataTable dt;
                    dt = new DataTable();
                    dt.Columns.Add("Meter Serial");
                    dt.Columns.Add("kWh");
                    dt.Columns.Add("kW");
                    dt.Columns.Add("kVARh");
                    dt.Columns.Add("kVAR");
                    dt.Columns.Add("Recording Time");

                    decimal[] yye = new decimal[10000];
                    decimal[] yyekW = new decimal[10000];
                    decimal[] yyekVARh = new decimal[10000];
                    decimal[] yyekVAR = new decimal[10000];
                    UInt64[] times = new UInt64[10000];
                    string[] tumes = new string[10000];
                    int count = 0;
                    string groot = "[";

                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT * FROM loadprofile1 where serial='" + res.Value + "' and Time_stamp between '" + DateTime.Parse(LPr1.Value).ToString("yyyy/M/d HH:mm:ss") + "' and '" + DateTime.Parse(LPr2.Value).ToString("yyyy/M/d HH:mm:ss") + "' order by Time_stamp desc;", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                DataRow drr = dt.NewRow();
                                drr["Meter Serial"] = res.Value;
                                drr["kWh"] = dr["kWh"].ToString();
                                drr["kW"] = dr["kW"].ToString();
                                drr["kVARh"] = dr["kVARh"].ToString();
                                drr["kVAR"] = dr["kVAR"].ToString();
                                drr["Recording Time"] = ((DateTime)(dr["Time_stamp"])).ToString("yyyy/MM/dd HH:mm:ss");
                                dt.Rows.Add(drr);

                                yye[count] = decimal.Round(Convert.ToDecimal((float)(dr["kWh"])), 3);
                                yyekW[count] = decimal.Round(Convert.ToDecimal((float)(dr["kW"])), 3);
                                yyekVARh[count] = decimal.Round(Convert.ToDecimal((float)(dr["kVARh"])), 3);
                                yyekVAR[count] = decimal.Round(Convert.ToDecimal((float)(dr["kVAR"])), 3);
                                times[count] = Convert.ToUInt64(((DateTime)(dr["Time_stamp"])).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
                                tumes[count] = dr["Time_stamp"].ToString();
                                
                                count++;
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    if(count<1)
                    {
                        ShowMessage("No entries for the selected interval!", MessageType.Error);
                        return;
                    }

                    SubmitBtn.Enabled = true;

                    kWhJSON = "";
                    kWJSON = "";
                    kVARJSON = "";
                    kVARhJSON = "";
                    string timesAlt = "";
                   // TimeSpanJSON = new string[count];
                    if (count > 0)
                    {
                        kWhJSON ="["+times[count - 1].ToString()+","+ yye[count - 1].ToString()+"]";
                        kWJSON = "["+times[count - 1].ToString()+","+yyekW[count - 1].ToString()+"]";
                        kVARhJSON ="["+times[count - 1].ToString()+"," +yyekVARh[count - 1].ToString()+"]";
                        kVARJSON = "[" + times[count - 1].ToString() + "," + yyekVAR[count - 1].ToString() + "]";
                        timesAlt = times[count - 1].ToString();
                        groot += "['"+res.Value+"','"+yye[count - 1].ToString()+"','"+yyekW[count - 1].ToString()+"','"+yyekVARh[count - 1].ToString()+"','"+yyekVAR[count - 1].ToString()+"','"+tumes[count - 1]+"']"; 
                       // TimeSpanJSON[0] = times[count - 1];

                        for (int j = count,i=1; j >= 2;i++, j--)
                        {
                            kWhJSON += ",[" + times[j - 2].ToString() + ", " + yye[j - 2] + "]";
                            kWJSON += ",[" + times[j - 2].ToString() + ", " + yyekW[j - 2] + "]";
                            kVARhJSON += ",[" + times[j - 2].ToString() + ", " + yyekVARh[j - 2] + "]";
                            kVARJSON += ",[" + times[j - 2].ToString() + ", " + yyekVAR[j - 2] + "]";
                            timesAlt += ", " + times[j - 2].ToString();
                            groot += ",['"+res.Value+"','"+yye[j- 2].ToString()+"','"+yyekW[j - 2].ToString()+"','"+yyekVARh[j - 2].ToString()+"','"+yyekVAR[j - 2].ToString()+"','"+tumes[j - 2]+"']"; 
                        }
                    }
                    groot += "]";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addingRowsLP(" +groot+ ");", true);

                    System.Web.UI.ScriptManager.RegisterStartupScript(LPUpdatePanel, LPUpdatePanel.GetType(), "resize", "cruncher('"+kWhJSON+"','"+kWJSON+"','"+kVARhJSON+"','"+kVARJSON+"','"+res.Value+"')", true);
                }
                else
                {
                    ShowMessage("Please select an active Device!", MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

  
   

  
  
        
   
     
   
      
        protected void GridviewEventConfiguration_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridviewEventConfiguration.PageIndex = e.NewPageIndex;
            populateEventGrid();
        }

        protected void GridviewEventConfiguration_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridviewEventConfiguration.EditIndex = e.NewEditIndex;
            this.populateEventGrid();
        }

        protected void GridviewEventConfiguration_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridviewEventConfiguration.Rows[e.RowIndex];
            string email = (row.FindControl("txtName") as TextBox).Text;
            string sms = (row.FindControl("txtCountry") as TextBox).Text;
            string serial = (row.FindControl("txtSerial") as TextBox).Text;
            
            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("update meter1 set smsAlert = '" + sms + "',emailAlert = '"+email+"' where serial = '"+serial+"' ;", connection))
                    command.ExecuteNonQuery();
                connection.Close();
            }
            GridviewEventConfiguration.EditIndex = -1;
            this.populateEventGrid();
        }

        protected void GridviewEventConfiguration_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridviewEventConfiguration.EditIndex = -1;
            this.populateEventGrid();
        }

        protected void GridViewAlarmsConfig_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewAlarmsConfig.PageIndex = e.NewPageIndex;
            this.populateAlarmsGrid();
        }

        protected void GridViewAlarmsConfig_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewAlarmsConfig.EditIndex = e.NewEditIndex;
            this.populateAlarmsGrid();
        }

        protected void GridViewAlarmsConfig_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewAlarmsConfig.EditIndex = -1;
            this.populateAlarmsGrid();
        }

        protected void GridViewAlarmsConfig_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            err.Text = "";
            try
            {
                GridViewRow row = GridViewAlarmsConfig.Rows[e.RowIndex];
                string code = (row.FindControl("txtCode") as TextBox).Text;
                string status = (row.FindControl("alarmStatusList") as DropDownList).SelectedValue;

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("update alarms1 set major = '" + status + "' where code = '" + code + "' ;", connection))
                        command.ExecuteNonQuery();
                    connection.Close();
                }
                GridViewAlarmsConfig.EditIndex = -1;
                this.populateAlarmsGrid();
            }
            catch (Exception ex)
            {
                err.Text = ex.ToString();
            }
        }


        
        protected void downloadHReport_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (res.Value != "" && IsDigitsOnly(res.Value))
                {
                    Document doc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
                    doc.Open();
                    Paragraph pa = new Paragraph("T-RECS Meter Health Report\nReport Type: Device Health Report\nMeter Serial:" + res.Value);
                    string imageURL = Server.MapPath(".") + "/trafo.jpg";
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                    //Resize image depend upon your need
                    jpg.ScaleToFit(140f, 60f);
                    //Give space before image
                    jpg.SpacingBefore = 10f;
                    //Give some space after the image
                    jpg.SpacingAfter = 1f;
                    jpg.Alignment = Element.ALIGN_RIGHT;
                    Paragraph credits = new Paragraph("Transfopower Industries (Pvt) Ltd.\n Copyright © Transfopower R&D Department 2015.");
                    Paragraph spaces = new Paragraph("\n\n");

                    credits.Alignment = Element.ALIGN_RIGHT;
                    credits.Font.Color = BaseColor.BLUE;
                    credits.Font.SetFamily(Font.FontFamily.COURIER.ToString());
                    credits.Font.Size = 8f;

                    pa.Alignment = Element.ALIGN_CENTER;
                    pa.Font.SetStyle(Font.BOLD);
                    pa.Font.Size = 14f;

                    doc.Add(pa);

                    PdfPTable table = new PdfPTable(2);
                    PdfPCell cell = new PdfPCell(new Phrase("Meter Health Report \n"));
                    cell.Colspan = 2;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);
                    table.HorizontalAlignment = 1;
                    
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT * FROM meter1 where serial = '" + res.Value + "';", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                table.AddCell("Serial");
                                table.AddCell(dr["serial"].ToString());
                                table.AddCell("Connection Status");
                                table.AddCell("Connected");
                                table.AddCell("Relay Status");
                                if (dr["relayStatus"].ToString() == "0")
                                {
                                    table.AddCell("Relay Disconnected");
                                }
                                else if (dr["relayStatus"].ToString() == "1")
                                {
                                    table.AddCell("Connected");
                                }
                                else
                                {
                                    table.AddCell("Ready to Connect");
                                }
                                table.AddCell("Last Keep-Alive");
                                table.AddCell(dr["times"].ToString());
                                table.AddCell("Last Read");
                                table.AddCell(((DateTime)(dr["lastRead"])).ToString());
                                table.AddCell("Customer Code");
                                table.AddCell(dr["customerCode"].ToString());
                                table.AddCell("SIM Number");
                                table.AddCell(dr["phone"].ToString());
                                table.AddCell("Lat");
                                table.AddCell(dr["lat"].ToString());
                                table.AddCell("Long");
                                table.AddCell(dr["longituge"].ToString());
                                table.AddCell("Rssi");
                                table.AddCell(double.Parse(dr["rssi"].ToString()) / 4 * 100 + "%");
                                table.AddCell("Firmware Version");
                                table.AddCell(dr["firmwareVersion"].ToString());
                                table.AddCell("Mode");
                                if (dr["mode"].ToString() == "0")
                                {
                                    table.AddCell("Mode 1");
                                }
                                else
                                {
                                    table.AddCell("Mode 2");
                                }
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    doc.Add(spaces);
                    doc.Add(table);
                    doc.Add(spaces);
                    doc.Add(jpg);
                    doc.Add(credits);
                    doc.Close();

                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Meter-Health_" + res.Value + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(doc);
                    Response.End();
                }
                else
                {
                    ShowMessage("Please select an active Device!", MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void ExcJSCommand(string steps)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), steps, true);
        }

   
 
   
 
        protected void read_Click(object sender, EventArgs e)
        {
            try
            {

                int n = 0;
                int msn = 0;
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT subsubsubutility FROM meter where subsubsubutility='" + res.Value + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            msn = (int)(dr["subsubsubutility"]);

                            n++;
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                if (n > 0)
                {
                    DataTable dt;
                    dt = new DataTable();
                    DateTime DT = DateTime.Now;
                    Session["msn"] = msn;
                    int connected = 0;
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT connected FROM meter WHERE subsubsubutility = '" + res.Value + "';", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                connected = (int)(dr["connected"]);
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    if (connected == 0)
                    {
                        ShowMessage("Meter disconnected", MessageType.Error);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "removeODRead();", true);
                        int IsReading = 0;

                        if (dataType.SelectedValue == "Meter Data")
                        {
                            IsReading = 1;
                            string temp = "[";
                            dt.Columns.Add("Meter Serial");
                            dt.Columns.Add("Description");
                            dt.Columns.Add("Value");
                            dt.Columns.Add("Time-Stamp");
                            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                            {
                                connection.Open();
                                using (OdbcCommand command = new OdbcCommand("SELECT * FROM dingrail.info1 where msn= " + (int)(Session["msn"]) + " order by Time_stamp desc limit 15000;", connection))

                                using (OdbcDataReader dr = command.ExecuteReader())
                                {
                                    while (dr.Read())
                                    {
                                        DataRow drr = dt.NewRow();
                                        drr["Meter Serial"] = res.Value;
                                        drr["Description"] = OBISTranslation(dr["OBIS"].ToString());
                                        drr["Value"] = dr["Amount"].ToString();
                                        drr["Time-Stamp"] = ((DateTime)(dr["Time_stamp"])).ToString();
                                        dt.Rows.Add(drr);

                                        temp += "['" + res.Value + "','" + OBISTranslation(dr["OBIS"].ToString()) + "','" + dr["Amount"].ToString() + "','" + ((DateTime)(dr["Time_stamp"])).ToString() + "'],";
                                    }
                                    dr.Close();
                                }
                                connection.Close();
                            }

                            temp = ReplaceAt(temp, temp.Length - 1, ']');

                            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addingRowsOD(" + temp + ");", true);

                            Session["DataTable"] = dt;
                            LinkButtonOnDemandDataDownload.Enabled = true;


                            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                            {
                                connection.Open();
                                using (OdbcCommand command = new OdbcCommand("UPDATE meter SET instantaneous ='1' WHERE subsubsubutility ='" + msn + "';", connection))
                                    command.ExecuteNonQuery();
                                connection.Close();
                            }

                            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                            {
                                connection.Open();
                                using (OdbcCommand command = new OdbcCommand("UPDATE meter SET demand ='1' WHERE subsubsubutility ='" + msn + "';", connection))
                                    command.ExecuteNonQuery();
                                connection.Close();
                            }



                            ShowMessage("Read Complete.", MessageType.Success);
                            System.Web.UI.ScriptManager.RegisterStartupScript(OnDemandReadingUpdatePanel, OnDemandReadingUpdatePanel.GetType(), "resize", "stop('ondemandBar','ondemandHide')", true);
                            UpdateTimer.Enabled = false;
                        }

                        else
                        {
                            int t = (int)(Session["ret"]);
                            t++;
                            Session["ret"] = t;

                            int inc = 20 + int.Parse(TextBoxData.Text);
                            TextBoxData.Text = inc.ToString();

                            if (t > 18 || (t > 12 && IsReading == 0) || inc > 200)
                            {
                                ShowMessage("Read Unsuccessful", MessageType.Error);
                                UpdateTimer.Enabled = false;
                                System.Web.UI.ScriptManager.RegisterStartupScript(OnDemandReadingUpdatePanel, OnDemandReadingUpdatePanel.GetType(), "resize", "stop('ondemandBar','ondemandHide')", true);
                                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                                {
                                    connection.Open();
                                    using (OdbcCommand command = new OdbcCommand("UPDATE meter SET demand ='0',try = 0 WHERE msn ='" + (int)(Session["msn"]) + "';", connection))
                                        command.ExecuteNonQuery();
                                    connection.Close();
                                }
                            }
                        }

                        UpdateTimer.Enabled = true;
                        Session["ret"] = 0;
                        System.Web.UI.ScriptManager.RegisterStartupScript(OnDemandReadingUpdatePanel, OnDemandReadingUpdatePanel.GetType(), "resize", "fop('ondemandBar','ondemandContainer','ondemandHide')", true);
                        TextBoxData.Text = "0";
                        //Voltage_Load("1.0.32.7.0.255", "1.0.52.7.0.255", "1.0.72.7.0.255", res.Value);

                    }
                }
                else { ShowMessage("Meter Not Registered", MessageType.Error); }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error); errLog.Text = ex.Message;
            }


        }

        protected string OBISTranslation(string Val)
        {
            switch (Val)
            {
                case "1.0.0.8.0.255":
                    return "Total Active Energy";
                case "1.0.94.92.0.255":
                    return "Total Reactive Energy";
                case "1.0.32.7.0.255":
                    return "Voltage Phase A";
                case "1.0.52.7.0.255":
                    return "Voltage Phase B";
                case "1.0.72.7.0.255":
                    return "Voltage Phase C";
                case "1.0.31.7.0.255":
                    return "Current A";
                case "1.0.51.7.0.255":
                    return "Current B";
                case "1.0.71.7.0.255":
                    return "Current C";
                case "1.0.72.8.0.255":
                    return "Current N";
                case "1.0.15.6.0.255":
                    return "Combined Active POWER";
                case "1.0.3.7.0.255":
                    return "Combined Reactive Power";
                case "1.0.14.7.0.255":
                    return "Combined Frequency";
                case "1.0.13.7.0.255":
                    return "Combined Power Factor";
                case "1.0.13.20.0.255":
                    return "Combined Active demand";
               
                default:
                    return "";
            }
        }


        protected void UpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                int IsReading = 0;
                int Insta = 0;
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT reading,demand FROM meter1 where serial='" + long.Parse(res.Value) + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            IsReading = int.Parse(dr["reading"].ToString());
                            Insta = int.Parse(dr["demand"].ToString());
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                if (Insta == 0)
                {
                    DateTime DT = new DateTime();
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT Time_stamp FROM info11 where msn ='" + (int)(Session["msn"]) + "' order by Time_stamp desc limit 1;", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                DT = (DateTime)(dr["Time_stamp"]);
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    DataTable dt;
                    dt = new DataTable();

                    if (dataType.SelectedValue == "Energy And Demand")
                    {
                        string temp = "[";

                        dt.Columns.Add("Meter Serial");
                        dt.Columns.Add("OBIS");
                        dt.Columns.Add("Description");
                        dt.Columns.Add("Value");
                        dt.Columns.Add("Quantity/Unit");
                        dt.Columns.Add("Time-Stamp");
                        using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                        {
                            connection.Open();
                            using (OdbcCommand command = new OdbcCommand("SELECT * FROM info11 where msn='" + (int)(Session["msn"]) + "' and quantity = 0 and Time_stamp = '" + DT.ToString("yyyy/M/d HH:mm:ss") + "';", connection))
                            using (OdbcDataReader dr = command.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    DataRow drr = dt.NewRow();
                                    drr["Meter Serial"] = res.Value;
                                    drr["OBIS"] = dr["OBIS"].ToString();
                                    drr["Description"] = OBISTranslation(dr["OBIS"].ToString());
                                    drr["Value"] = dr["Amount"].ToString();
                                    drr["Quantity/Unit"] = dr["unit"].ToString();
                                    drr["Time-Stamp"] = ((DateTime)(dr["Time_stamp"])).ToString();
                                    dt.Rows.Add(drr);

                                    temp += "['" + res.Value + "','" + dr["OBIS"].ToString() + "','" + OBISTranslation(dr["OBIS"].ToString()) + "','" + dr["Amount"].ToString() + "','" + dr["unit"].ToString() + "','" + ((DateTime)(dr["Time_stamp"])).ToString() + "'],";
                                }
                                dr.Close();
                            }
                            connection.Close();
                        }

                        temp = ReplaceAt(temp, temp.Length - 1, ']');

                        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addingRowsOD("+temp+");", true);

                        Session["DataTable"] = dt;
                        LinkButtonOnDemandDataDownload.Enabled = true;
                    }


                    if (dataType.SelectedValue == "Instantaneous")
                    {
                        string temp = "[";

                        dt = new DataTable();
                        dt.Columns.Add("Meter Serial");
                        dt.Columns.Add("OBIS");
                        dt.Columns.Add("Description");
                        dt.Columns.Add("Value");
                        dt.Columns.Add("Quantity/Unit");
                        dt.Columns.Add("Time-Stamp");

                        using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                        {
                            connection.Open();
                            using (OdbcCommand command = new OdbcCommand("SELECT * FROM info11 where msn='" + (int)(Session["msn"]) + "' and quantity = 1 and Time_stamp = '" + DT.ToString("yyyy/M/d HH:mm:ss") + "';", connection))
                            using (OdbcDataReader dr = command.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    DataRow drr = dt.NewRow();
                                    drr["Meter Serial"] = res.Value;
                                    drr["OBIS"] = dr["OBIS"].ToString();
                                    drr["Description"] = OBISTranslation(dr["OBIS"].ToString());
                                    drr["Value"] = dr["Amount"].ToString();
                                    drr["Quantity/Unit"] = dr["unit"].ToString();
                                    drr["Time-Stamp"] = ((DateTime)(dr["Time_stamp"])).ToString();
                                    dt.Rows.Add(drr);
                                    temp += "['" + res.Value + "','" + dr["OBIS"].ToString() + "','" + OBISTranslation(dr["OBIS"].ToString()) + "','" + dr["Amount"].ToString() + "','" + dr["unit"].ToString() + "','" + ((DateTime)(dr["Time_stamp"])).ToString() + "'],";
                                }
                                dr.Close();
                            }
                            connection.Close();
                        }

                        temp = ReplaceAt(temp, temp.Length - 1, ']');

                        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addingRowsOD(" + temp + ");", true);

                        Session["DataTable"] = dt;
                        LinkButtonOnDemandDataDownload.Enabled = true;
                    }

                    ShowMessage("Read Complete.", MessageType.Success);
                    System.Web.UI.ScriptManager.RegisterStartupScript(OnDemandReadingUpdatePanel, OnDemandReadingUpdatePanel.GetType(), "resize", "stop('ondemandBar','ondemandHide')", true);
                    UpdateTimer.Enabled = false;
                }
                else
                {
                    int t = (int)(Session["ret"]);
                    t++;
                    Session["ret"] = t;

                    int inc = 20 + int.Parse(TextBoxData.Text);
                    TextBoxData.Text = inc.ToString();

                    if (t > 18 || (t > 12 && IsReading == 0) || inc > 200)
                    {
                        ShowMessage("Read Unsuccessful", MessageType.Error);
                        UpdateTimer.Enabled = false;
                        System.Web.UI.ScriptManager.RegisterStartupScript(OnDemandReadingUpdatePanel, OnDemandReadingUpdatePanel.GetType(), "resize", "stop('ondemandBar','ondemandHide')", true);
                        using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                        {
                            connection.Open();
                            using (OdbcCommand command = new OdbcCommand("UPDATE meter1 SET demand ='0',try = 0 WHERE msn ='" + (int)(Session["msn"]) + "';", connection))
                                command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

  
  
  
     
    
        
        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            if (res.Value != "" && IsDigitsOnly(res.Value) && LPr1.Value != "" && LPr2.Value != "")
            {
                try
                {
                    Document doc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
                    doc.Open();
                    Paragraph pa = new Paragraph("T-RECS Load Profile Report\nReport Type: Custom Load Profile Report\nMeter Serial:" + res.Value);
                    string imageURL = Server.MapPath(".") + "/trafo.jpg";
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                    //Resize image depend upon your need
                    jpg.ScaleToFit(140f, 60f);
                    //Give space before image
                    jpg.SpacingBefore = 10f;
                    //Give some space after the image
                    jpg.SpacingAfter = 1f;
                    jpg.Alignment = Element.ALIGN_RIGHT;
                    Paragraph credits = new Paragraph("Transfopower Industries (Pvt) Ltd.\n Copyright © Transfopower R&D Department 2015.");
                    Paragraph spaces = new Paragraph("\n\n");

                    credits.Alignment = Element.ALIGN_RIGHT;
                    credits.Font.Color = BaseColor.BLUE;
                    credits.Font.SetFamily(Font.FontFamily.COURIER.ToString());
                    credits.Font.Size = 8f;

                    pa.Alignment = Element.ALIGN_CENTER;
                    pa.Font.SetStyle(Font.BOLD);
                    pa.Font.Size = 14f;

                    doc.Add(pa);

                    PdfPTable table = new PdfPTable(6);
                    PdfPCell cell = new PdfPCell(new Phrase("Load Profile Report \n"));
                    cell.Colspan = 6;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);
                    table.HorizontalAlignment = 1;

                    table.AddCell("Serial #");
                    table.AddCell("kWh");
                    table.AddCell("kW");
                    table.AddCell("kVARh");
                    table.AddCell("kVAR");
                    table.AddCell("Date Time");

                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("select * from loadprofile1 where serial = '" + res.Value + "' and Time_stamp between '" + DateTime.Parse(LPr1.Value).ToString("yyyy/M/d HH:mm:ss") + "' and '" + DateTime.Parse(LPr2.Value).ToString("yyyy/M/d HH:mm:ss") + "';", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                table.AddCell(dr["serial"].ToString());
                                table.AddCell(dr["kWh"].ToString());
                                table.AddCell(dr["kW"].ToString());
                                table.AddCell(dr["kVARh"].ToString());
                                table.AddCell(dr["kVAR"].ToString());
                                table.AddCell(((DateTime)(dr["Time_stamp"])).ToString());
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    doc.Add(spaces);
                    doc.Add(table);
                    doc.Add(spaces);
                    doc.Add(jpg);
                    doc.Add(credits);
                    doc.Close();

                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=LoadProfileData_" + res.Value + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(doc);
                    Response.End();
                }
                catch (Exception ex)
                {
                    ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
                }
            }
            else
            {
                ShowMessage("Please select an active Device!", MessageType.Error);
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                float kWh = 0, kVARh = 0,freq=0;
                Session["Serial"] = res.Value;
                int n = (int)(Session["msn"]);
                int job = 0;
                int Isreading = 0;
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT retry,reading FROM dingrail.meter where subsubsubutility = '" + n + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            job = (int)(dr["retry"]);
                            Isreading = int.Parse(dr["reading"].ToString());
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                if (job == 0)
                {
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT total_active_energy FROM dingrail.meter_data where meter_no= " + res.Value + " order by Time_stamp desc limit 1;", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                kWh = (float)(dr["total_active_energy"]);
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT total_reactive_energy FROM dingrail.meter_data where meter_no= " + res.Value + " order by Time_stamp desc limit 1;", connection))

                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                kVARh = (float)(dr["total_reactive_energy"]);
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT combine_frequency FROM dingrail.meter_data where meter_no= " + res.Value + " order by Time_stamp desc limit 1;", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                freq = (float)(dr["combine_frequency"]);
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    Session["kVARhVal"] = kVARh;
                    Session["kWhVal"] = kWh;
                    Session["freqVal"]= freq;
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT Time_stamp FROM dingrail.meter_data WHERE meter_no=" + res.Value + " order by Time_stamp desc limit 1;", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Session["date"] = dr["Time_stamp"].ToString();
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }



                    Session["T"] = DateTime.Now;
                    reportLink.Enabled = true;
                    ShowMessage("Read Complete.", MessageType.Success);
                    System.Web.UI.ScriptManager.RegisterStartupScript(TimedPanel, TimedPanel.GetType(), "resize", "quickReadValues('" + kWh + "','" + kVARh + "','" + freq + "','" + DateTime.Now.ToString() + "')", true);
                    Timer1.Enabled = false;
                    Session["retry"] = 0;
                }
                else
                {
                    int o = (int)(Session["retry"]);
                    o++;
                    Session["retry"] = o;
                    if (o > 25 || (o > 11 && Isreading == 0))
                    {
                        Timer1.Enabled = false;
                        ShowMessage("Read Unsuccessful.", MessageType.Error);
                        System.Web.UI.ScriptManager.RegisterStartupScript(TimedPanel, TimedPanel.GetType(), "resize", "loaderQuick(0)", true);
                        Session["retry"] = 0;
                        using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                        {
                            connection.Open();
                            using (OdbcCommand command = new OdbcCommand("UPDATE meter SET retry ='0',try = 0 WHERE subsubsubutility = '" + (int)(Session["msn"]) + "'", connection))
                                command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error); errLog.Text = ex.Message;
            }
        }

        protected void reportLink_Click(object sender, EventArgs e)
        {
            DateTime DT;
            try
            {
                DT = (DateTime)(Session["T"]);
                Document doc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();
                Paragraph pa = new Paragraph("T-RECS Quick Read Report\n \n \n \n");
                string imageURL = Server.MapPath(".") + "/trafo.jpg";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                //Resize image depend upon your need
                jpg.ScaleToFit(140f, 60f);
                //Give space before image
                jpg.SpacingBefore = 10f;
                //Give some space after the image
                jpg.SpacingAfter = 1f;
                jpg.Alignment = Element.ALIGN_RIGHT;
                Paragraph credits = new Paragraph("Transfopower Industries (Pvt) Ltd.\n Copyright © Transfopower R&D Department 2015.");
                Paragraph spaces = new Paragraph("\n\n\n");

                credits.Alignment = Element.ALIGN_RIGHT;
                credits.Font.Color = BaseColor.BLUE;
                credits.Font.SetFamily(Font.FontFamily.COURIER.ToString());
                credits.Font.Size = 8f;

                pa.Alignment = Element.ALIGN_CENTER;
                pa.Font.SetStyle(Font.BOLD);
                pa.Font.Size = 14f;

                PdfPTable table = new PdfPTable(2);
                PdfPCell cell = new PdfPCell(new Phrase("TPI34G Test Sample Reading Results"));
                cell.Colspan = 2;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
                table.HorizontalAlignment = 1;
                table.AddCell("Time Stamp");
                table.AddCell((string)(Session["date"]));
                table.AddCell("Meter Serial Number");
                table.AddCell((string)(Session["Serial"]));
                table.AddCell("kWh Total");
                table.AddCell(((float)(Session["kWhVal"])).ToString());
                table.AddCell("kVARh Total");
                table.AddCell(((float)(Session["kVARhVal"])).ToString());
                table.AddCell("Total Frequency");
                table.AddCell(((float)(Session["freqVal"])).ToString());
                doc.Add(pa);
                doc.Add(table);
                //doc.Add(paragraph);
                doc.Add(spaces);
                doc.Add(jpg);
                doc.Add(credits);
                doc.Close();

                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + DT.ToString() + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
                Response.End();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void LinkButtonRead_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(IsDigitsOnly(res.Value) && res.Value != ""))
                { ShowMessage("Please select an active Device!", MessageType.Error); return; }
                Timer1.Enabled = true;
                System.Web.UI.ScriptManager.RegisterStartupScript(TimedPanel, TimedPanel.GetType(), "resize", "loaderQuick(1)", true);
                int n = 0;
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT subsubsubutility FROM meter WHERE subsubsubutility = '" + res.Value + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                            n = (int)(dr["subsubsubutility"]);
                        dr.Close();
                    }
                    connection.Close();
                }

                Session["msn"] = n;
                Session["retry"] = 0;

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("UPDATE meter SET retry ='1' WHERE subsubsubutility ='" + n + "';", connection))
                        command.ExecuteNonQuery();
                    connection.Close();
                }

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("INSERT INTO actionslog(Username,Action,meter_no,Time_stamp) values('" + ((string)(Session["username"])) + "','Instant Read','" + res.Value + "','" + String.Format("{0:yyyy/M/d HH:mm:ss}", DateTime.Now) + "');", connection))


                        command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error); errLog.Text = ex.Message;
            }
        }

        protected void ButtonGenerateEventReport_Click(object sender, EventArgs e)
        {
            if (!(evntRprtdt1.Value == "" || evntRprtdt2.Value == ""))
            {
                try
                {
                    string query = "";
                    switch (eventPopupType.SelectedValue)
                    {
                        case "Relay Operation":
                            query = "select * from eventlist1 where msn = " + eventPopupDropDownlist.SelectedValue + " and (eventCode = 54 or eventCode = 55) and Time_stamp between '" + evntRprtdt1.Value + "' and '" + evntRprtdt2.Value + "'; ";
                            break;
                        case "Power Fail":
                            query = "select * from eventlist1 where msn = " + eventPopupDropDownlist.SelectedValue + " and (eventCode = 111 or eventCode = 112) and Time_stamp between '" + evntRprtdt1.Value + "' and '" + evntRprtdt2.Value + "'; ";
                            break;
                        case "MDI reset":
                            query = "select * from eventlist1 where msn = " + eventPopupDropDownlist.SelectedValue + " and eventCode = 101 and Time_stamp between '" + evntRprtdt1.Value + "' and '" + evntRprtdt2.Value + "'; ";
                            break;
                        case "Parametrization":
                            query = "select * from eventlist1 where msn = " + eventPopupDropDownlist.SelectedValue + " and eventCode = 102 and Time_stamp between '" + evntRprtdt1.Value + "' and '" + evntRprtdt2.Value + "'; ";
                            break;
                        case "Phase failure":
                            query = "select * from eventlist1 where msn = " + eventPopupDropDownlist.SelectedValue + " and (eventCode = 113 or eventCode = 143) and Time_stamp between '" + evntRprtdt1.Value + "' and '" + evntRprtdt2.Value + "'; ";
                            break;
                        case "Over/under Volt":
                            query = "select * from eventlist1 where msn = " + eventPopupDropDownlist.SelectedValue + " and (eventCode = 114 or eventCode = 115) and Time_stamp between '" + evntRprtdt1.Value + "' and '" + evntRprtdt2.Value + "'; ";
                            break;
                        case "Demand Over Load":
                            query = "select * from eventlist1 where msn = " + eventPopupDropDownlist.SelectedValue + " and eventCode = 116 and Time_stamp between '" + evntRprtdt1.Value + "' and '" + evntRprtdt2.Value + "'; ";
                            break;
                        case "Reverse Energy":
                            query = "select * from eventlist1 where msn = " + eventPopupDropDownlist.SelectedValue + " and (eventCode = 117 or eventCode = 147) and Time_stamp between '" + evntRprtdt1.Value + "' and '" + evntRprtdt2.Value + "'; ";
                            break;
                        case "Reverse Polarity":
                            query = "select * from eventlist1 where msn = " + eventPopupDropDownlist.SelectedValue + " and (eventCode = 118 or eventCode = 148) and Time_stamp between '" + evntRprtdt1.Value + "' and '" + evntRprtdt2.Value + "'; ";
                            break;
                        case "CT Bypass":
                            query = "select * from eventlist1 where msn = " + eventPopupDropDownlist.SelectedValue + " and (eventCode = 121 or eventCode = 151) and Time_stamp between '" + evntRprtdt1.Value + "' and '" + evntRprtdt2.Value + "'; ";
                            break;
                        case "All":
                            query = "select * from eventlist1 where msn = " + eventPopupDropDownlist.SelectedValue + " and Time_stamp between '" + evntRprtdt1.Value + "' and '" + evntRprtdt2.Value + "'; ";
                            break;
                        default:
                            break;
                    }

                    string[] dataT = new string[1000];
                    string[] dataS = new string[1000];
                    string[] dataC = new string[1000];
                    string[] dataD = new string[1000];
                    int count = 0;
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand(query, connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dataT[count] = ((DateTime)(dr["Time_stamp"])).ToString();
                                dataS[count] = dr["msn"].ToString();
                                dataC[count] = dr["eventCode"].ToString();
                                dataD[count] = eventCodeTranslation(dr["eventCode"].ToString());
                                count++;
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    Document doc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
                    doc.Open();
                    Paragraph pa = new Paragraph("T-RECS Event Report\n \nMeter Serial:" + eventPopupDropDownlist.SelectedValue + "\nEvent Type:" + eventPopupType.SelectedValue);
                    string imageURL = Server.MapPath(".") + "/trafo.jpg";
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                    //Resize image depend upon your need
                    jpg.ScaleToFit(140f, 60f);
                    //Give space before image
                    jpg.SpacingBefore = 10f;
                    //Give some space after the image
                    jpg.SpacingAfter = 1f;
                    jpg.Alignment = Element.ALIGN_RIGHT;
                    Paragraph credits = new Paragraph("Transfopower Industries (Pvt) Ltd.\n Copyright © Transfopower R&D Department 2015.");
                    Paragraph spaces = new Paragraph("\n\n");

                    credits.Alignment = Element.ALIGN_RIGHT;
                    credits.Font.Color = BaseColor.BLUE;
                    credits.Font.SetFamily(Font.FontFamily.COURIER.ToString());
                    credits.Font.Size = 8f;

                    pa.Alignment = Element.ALIGN_CENTER;
                    pa.Font.SetStyle(Font.BOLD);
                    pa.Font.Size = 14f;

                    doc.Add(pa);

                    PdfPTable table = new PdfPTable(4);
                    PdfPCell cell = new PdfPCell(new Phrase("Event Log\n"));
                    cell.Colspan = 4;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);
                    table.HorizontalAlignment = 1;

                    table.AddCell("Date Time");
                    table.AddCell("Serial #");
                    table.AddCell("Event Code");
                    table.AddCell("Description");

                    for (int i = 0; i < count; i++)
                    {
                        table.AddCell(dataT[i]);
                        table.AddCell(dataS[i]);
                        table.AddCell(dataC[i]);
                        table.AddCell(dataD[i]);
                    }
                    doc.Add(spaces);
                    doc.Add(table);
                    doc.Add(spaces);
                    doc.Add(jpg);
                    doc.Add(credits);
                    doc.Close();

                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + eventPopupDropDownlist.SelectedValue + "-Events-" + eventPopupType.SelectedValue + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(doc);
                    Response.End();
                }
                catch (Exception ex)
                {
                    ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
                }
            }
        }

        protected void btnMDIExceedReportGen_Click(object sender, EventArgs e)
        {
            string[] dataT = new string[1000];
            string[] dataS = new string[1000];
            string[] dataC = new string[1000];
            int count = 0;
            if (!(MDIr1.Value == "" || MDIr2.Value == "" || MDIEXceedValue.Text.Length < 1))
            {
                try
                {
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("select * from loadprofile1 where kW > " + MDIEXceedValue.Text + " and serial between '" + DropDownListMDIExceedR1.SelectedValue + "' and '" + DropDownListMDIExceedR2.SelectedValue + "' and Time_stamp between '" + MDIr1.Value + "' and '" + MDIr2.Value + "';", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dataT[count] = ((DateTime)(dr["Time_stamp"])).ToString();
                                dataS[count] = dr["serial"].ToString();
                                dataC[count] = dr["kW"].ToString();
                                count++;
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    Document doc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
                    doc.Open();
                    Paragraph pa = new Paragraph("T-RECS MDI Exceed Report\nReport Type: Custom MDI Exceed Report\nMDI Exceed Threshold:" + MDIEXceedValue.Text + "kW\nDevice Range:" + DropDownListMDIExceedR1.SelectedValue + "-" + DropDownListMDIExceedR2.SelectedValue);
                    string imageURL = Server.MapPath(".") + "/trafo.jpg";
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                    //Resize image depend upon your need
                    jpg.ScaleToFit(140f, 60f);
                    //Give space before image
                    jpg.SpacingBefore = 10f;
                    //Give some space after the image
                    jpg.SpacingAfter = 1f;
                    jpg.Alignment = Element.ALIGN_RIGHT;
                    Paragraph credits = new Paragraph("Transfopower Industries (Pvt) Ltd.\n Copyright © Transfopower R&D Department 2015.");
                    Paragraph spaces = new Paragraph("\n\n");

                    credits.Alignment = Element.ALIGN_RIGHT;
                    credits.Font.Color = BaseColor.BLUE;
                    credits.Font.SetFamily(Font.FontFamily.COURIER.ToString());
                    credits.Font.Size = 8f;

                    pa.Alignment = Element.ALIGN_CENTER;
                    pa.Font.SetStyle(Font.BOLD);
                    pa.Font.Size = 14f;

                    doc.Add(pa);

                    PdfPTable table = new PdfPTable(3);
                    PdfPCell cell = new PdfPCell(new Phrase("MDI Exceed Report \n"));
                    cell.Colspan = 3;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);
                    table.HorizontalAlignment = 1;

                    table.AddCell("Date Time");
                    table.AddCell("Serial #");
                    table.AddCell("MDI");

                    for (int i = 0; i < count; i++)
                    {
                        table.AddCell(dataT[i]);
                        table.AddCell(dataS[i]);
                        table.AddCell(dataC[i]);
                    }
                    doc.Add(spaces);
                    doc.Add(table);
                    doc.Add(spaces);
                    doc.Add(jpg);
                    doc.Add(credits);
                    doc.Close();

                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=MDI-Exceed.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(doc);
                    Response.End();
                }
                catch (Exception ex)
                {
                    ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
                }
            }
        }

        private System.Data.DataTable GetDataInst()
        {
            int n = 999999999;
            DateTime DT = new DateTime();
            string[] OBISList = new string[40];
            string[] AmountList = new string[40];
            string[] UnitList = new string[40];
            DataTable dt = new DataTable();
            try
            {
                int OBISCount = 0, AmountCount = 0, UnitCount = 0;

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT msn FROM meter1 WHERE serial = '" + res.Value + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                            n = (int)(dr["msn"]);
                        dr.Close();
                    }
                    connection.Close();
                }

                DateTime time = DateTime.Parse(csvSingleDT.Value);
                string Time_stamp = time.ToString("yyyy/M/d HH:mm:ss");

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT Time_stamp FROM info11 WHERE msn=" + n + " and quantity=1 and Time_stamp<'" + Time_stamp + "' order by Time_stamp desc limit 1;", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DT = (DateTime)(dr["Time_stamp"]);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                string stamp = String.Format("{0:yyyy/M/d HH:mm:ss}", DT);

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT Amount,unit FROM info11 WHERE msn='" + n + "' and quantity=1 AND Time_stamp = '" + stamp + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            AmountList[AmountCount] = ((float)(dr["Amount"])).ToString();
                            AmountCount++;
                            UnitList[UnitCount] = (string)(dr["unit"]);
                            UnitCount++;
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                dt.Columns.Add("Sr #");
                dt.Columns.Add("Item Name");
                dt.Columns.Add("Type (ASCII Text)");
                dt.Columns.Add("Description");

                dt.Rows.Add(1, "Reading Date", DT.ToString("yy/MM/dd"), "");
                dt.Rows.Add(2, "Reading Time", DT.ToString("HH:mm:ss"), "");
                dt.Rows.Add(3, "Reference Number", UnitList[19], "");
                dt.Rows.Add(4, "Meter Serial Number", res.Value, "");

                dt.Rows.Add(5, "Aggregate Active Power Import (KW+)", AmountList[0], "kW");
                dt.Rows.Add(6, "Aggregate Active Power Export (KW-)", AmountList[1], "kW");
                dt.Rows.Add(7, "Aggregate Reactive Power Import", AmountList[2], "kW");
                dt.Rows.Add(8, "Aggregate Reactive Power export", AmountList[3], "kW");
                dt.Rows.Add(9, "Voltage Phase A", AmountList[4], "Volts");
                dt.Rows.Add(10, "Voltage Phase B", AmountList[5], "Volts");
                dt.Rows.Add(11, "Voltage Phase C", AmountList[6], "Volts");
                dt.Rows.Add(12, "Current Phase A", AmountList[7], "Amperes");
                dt.Rows.Add(13, "Current Phase B", AmountList[8], "Amperes");
                dt.Rows.Add(14, "Current Phase C", AmountList[9], "Amperes");
                dt.Rows.Add(15, "Average Power Factor", AmountList[10], "");
                dt.Rows.Add(16, "Frequency", AmountList[11], "Hz");
                dt.Rows.Add(17, "Time", UnitList[12], "");
                dt.Rows.Add(18, "Date", UnitList[13], "");
                dt.Rows.Add(19, "Current Tariff", AmountList[14], "Number");

                return dt;
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
            return dt;
        }

        private System.Data.DataTable GetDataBilling()
        {
            int n = 999999999;
            DateTime DT = new DateTime();
            string[] OBISList = new string[40];
            string[] AmountList = new string[40];
            string[] UnitList = new string[40];
            DataTable dt = new DataTable();
            try
            {
                int OBISCount = 0, AmountCount = 0, UnitCount = 0;

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT msn FROM meter1 WHERE serial = '" + res.Value + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                            n = (int)(dr["msn"]);
                        dr.Close();
                    }
                    connection.Close();
                }

                DateTime time = DateTime.Parse(csvSingleDT.Value);
                string Time_stamp = time.ToString("yyyy/M/d HH:mm:ss");

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT Time_stamp FROM info11 WHERE msn=" + n + " and quantity=0 and Time_stamp<'" + Time_stamp + "' order by Time_stamp desc limit 1;", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DT = (DateTime)(dr["Time_stamp"]);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                string stamp = String.Format("{0:yyyy/M/d HH:mm:ss}", DT);

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT Amount,unit FROM info11 WHERE msn='" + n + "' and quantity=0 AND Time_stamp = '" + stamp + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            AmountList[AmountCount] = ((float)(dr["Amount"])).ToString();
                            AmountCount++;
                            UnitList[UnitCount] = (string)(dr["unit"]);
                            UnitCount++;
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT unit FROM info11 WHERE OBIS='0.0.96.1.10.255' and msn=" + n + " order by Time_stamp desc limit 1;", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            UnitList[19] = (string)(dr["unit"]);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                dt.Columns.Add("Sr #");
                dt.Columns.Add("Item Name");
                dt.Columns.Add("Type (ASCII Text)");
                dt.Columns.Add("Description");

                dt.Rows.Add(1, "Reading Date", DT.ToString("yy/MM/dd"), "");
                dt.Rows.Add(2, "Reading Time", DT.ToString("HH:mm:ss"), "");
                dt.Rows.Add(3, "Reference Number", UnitList[19], "");
                dt.Rows.Add(4, "Meter Serial Number", res.Value, "");

                dt.Rows.Add(5, "T1 Active kWh", AmountList[1], "kWh");
                dt.Rows.Add(6, "T2 Active kWh", AmountList[2], "kWh");
                dt.Rows.Add(7, "T3 Active kWh", "         ", "kWh");
                dt.Rows.Add(8, "T4 Active kWh", "         ", "kWh");
                dt.Rows.Add(9, "TL Active kWh", AmountList[0], "kWh");
                dt.Rows.Add(10, "T1 Reactive kVArh", AmountList[4], "kVArh");
                dt.Rows.Add(11, "T2 Reactive kVArh", AmountList[5], "kVArh");
                dt.Rows.Add(12, "T3 Reactive kVArh", "         ", "kVArh");
                dt.Rows.Add(13, "T4 Reactive kVArh", "         ", "kVArh");
                dt.Rows.Add(14, "TL Reactive kVArh", AmountList[3], "kVArh");
                dt.Rows.Add(15, "T1 Active MDI", AmountList[7], "kW");
                dt.Rows.Add(16, "T2 Active MDI", AmountList[8], "kW");
                dt.Rows.Add(17, "T3 Active MDI", "         ", "kW");
                dt.Rows.Add(18, "T4 Active MDI", "         ", "kW");
                dt.Rows.Add(19, "TL Active MDI", AmountList[6], "kW");
                dt.Rows.Add(20, "T1 Cumulative Active MDI", AmountList[10], "kW");
                dt.Rows.Add(21, "T2 Cumulative Active MDI", AmountList[11], "kW");
                dt.Rows.Add(22, "T3 Cumulative Active MDI", "         ", "kW");
                dt.Rows.Add(23, "T4 Cumulative Active MDI", "         ", "kW");
                dt.Rows.Add(24, "TL Cumulative Active MDI", AmountList[9], "kW");
                dt.Rows.Add(25, "MDI Reset Date", UnitList[13], "");
                dt.Rows.Add(26, "MDI Reset Time", UnitList[13], "");
                dt.Rows.Add(27, "MDI Reset Count Number", AmountList[12], "");
                ;
                return dt;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message,MessageType.Error);
            }
            return dt;
        }

        protected void LinkButtonCSVDownload_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(IsDigitsOnly(res.Value))) { ShowMessage("Select a device.", MessageType.Error); return; }

                DataTable dt = new DataTable();
                string tuup = "Instantaneous_Data";
                if(csvDDL.Text == "Billing")
                {
                    dt = GetDataBilling();
                    tuup = "Billing_Data";
                }
                else
                {
                    dt = GetDataInst();
                }

                //Build the CSV file data as a Comma separated string.
                string csv = string.Empty;

                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Header row for CSV file.
                    csv += column.ColumnName + ',';
                }

                //Add new line.
                csv += "\r\n";

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        //Add the Data rows.
                        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                    }

                    //Add new line.
                    csv += "\r\n";
                }

                //Download the CSV file.
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + tuup + "_" + res.Value + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(csv);
                Response.Flush();
                Response.End();
            }
            catch(Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void LinkButtonOnDemandDataDownload_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)(Session["DataTable"]);
                //Create a dummy GridView
                GridView GridViewV = new GridView();
                GridViewV.AllowPaging = false;
                GridViewV.DataSource = dt;
                GridViewV.DataBind();

                Paragraph pa = new Paragraph("T-RECS On Demand Data Reading Report \n\n");
                pa.Alignment = Element.ALIGN_CENTER;
                pa.Font.SetStyle(Font.BOLD);
                pa.Font.Size = 14f;

                Paragraph footnote = new Paragraph("T-RECS Data Core  Copyright© Transfopower Industries Pvt Limited.");

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition",
                    "attachment;filename=Read-Data.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridViewV.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                pdfDoc.Add(pa);
                htmlparser.Parse(sr);
                pdfDoc.Add(footnote);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                Response.End(); 

            }
            catch(Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void LinkButtonStatGraphGen_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag1 = false;
                bool flag2 = false;

                int count = 0, count1 = 0, kount = 0;
                string[] temptimeData = new string[1024];
                float[] tempAmount = new float[1024];

                long n;
                string OBIS1 = "", OBIS2 = "", OBIS3 = "";
                string title1 = "", title2 = "", title3 = "", titleY = "";

                if (GraphQuanityType.Text == "Cumulative Active Energy Absolute")
                {
                    OBIS1 = "1.0.15.8.0.255";

                    title1 = "TL";
                    titleY = "kWh";
                    flag1 = true;

                }
                if (GraphQuanityType.Text == "Cumulative Reactive Energy Absolute")
                {
                    OBIS1 = "1.0.94.92.0.255";
                    title1 = "TL";
                    titleY = "kVARh";
                    flag1 = true;
                }
                if (GraphQuanityType.Text == "Maximum Demand Active Absolute")
                {
                    OBIS1 = "1.0.15.6.0.255";
                    title1 = "TL";
                    titleY = "kW";
                    flag1 = true;
                }
                if (GraphQuanityType.Text == "Cumulative Maximum Demand Active Absolute")
                {
                    OBIS1 = "1.0.15.2.0.255";
                    title1 = "TL";
                    titleY = "kW";
                    flag1 = true;
                }
                if (GraphQuanityType.Text == "Average Power Factor")
                {
                    OBIS1 = "1.0.13.7.0.255";
                    title1 = "Avg PF";
                    titleY = "Value";
                    flag1 = true;
                }
                if (GraphQuanityType.Text == "Voltage")
                {
                    OBIS1 = "1.0.32.7.0.255";
                    OBIS2 = "1.0.52.7.0.255";
                    OBIS3 = "1.0.72.7.0.255";
                    title1 = "Phase A";
                    title2 = "Phase B";
                    title3 = "Phase C";
                    titleY = "Volts";
                    flag2 = true;
                }
                if (GraphQuanityType.Text == "Current")
                {
                    OBIS1 = "1.0.31.7.0.255";
                    OBIS2 = "1.0.51.7.0.255";
                    OBIS3 = "1.0.71.7.0.255";
                    title1 = "Phase A";
                    title2 = "Phase B";
                    title3 = "Phase C";
                    titleY = "Amperes";
                    flag2 = true;
                }

                string r1 = DateTime.Parse(rootG1.Value).ToString("yyyy/M/d HH:mm:ss");
                string r2 = DateTime.Parse(rootG2.Value).ToString("yyyy/M/d HH:mm:ss");

                count = 0;
                count1 = 0;
                kount = 0;
                temptimeData = new string[1024];
                tempAmount = new float[1024];
                float[] tempAmountTf1 = new float[1024];
                float[] tempAmountTf2 = new float[1024];
                n = 99999999999999;

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT meter_no FROM meter_data WHERE meter_no = '" + res.Value + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                            n = (int)(dr["meter_no"]);
                        dr.Close();
                    }
                    connection.Close();
                }

                if (n == 99999999999999)
                {
                    throw new TimeoutException("Record Not Found!");
                }
                if (flag1 == true || flag2 == true)
                {
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT Amount FROM info1 WHERE OBIS = '" + OBIS1 + "' AND msn ='" + n + "' and Time_stamp between '" + r1 + "' and '" + r2 + "';", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                tempAmount[kount] = (float)(dr["Amount"]);
                                kount++;
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }
                }
                if (flag2 == true)
                {
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT Amount FROM info1 WHERE OBIS = '" + OBIS2 + "' AND msn ='" + n + "' and Time_stamp between '" + r1 + "' and '" + r2 + "';", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                tempAmountTf1[count] = (float)(dr["Amount"]);
                                count++;
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT Amount FROM info1 WHERE OBIS = '" + OBIS3 + "' AND msn ='" + n + "' and Time_stamp between '" + r1 + "' and '" + r2 + "';", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                tempAmountTf2[count] = (float)(dr["Amount"]);
                                count++;
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }
                }
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT Time_stamp FROM info1 WHERE OBIS = '" + OBIS1 + "' AND msn ='" + n + "' and Time_stamp between '" + r1 + "' and '" + r2 + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            temptimeData[count1] = Convert.ToUInt64(((DateTime)(dr["Time_stamp"])).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds).ToString();
                            count1++;
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                string s1 = "", s2 = "", s3 = "";


                if (flag2 == true)
                {
                    if (count > 0)
                    {
                        s1 = "[" + temptimeData[0] + "," + tempAmount[0].ToString() + "]";
                        s2 = "[" + temptimeData[0] + "," + tempAmountTf1[0].ToString() + "]";
                        s3 = "[" + temptimeData[0] + "," + tempAmountTf2[0].ToString() + "]";

                        for (int i = 1; i < count; i++)
                        {
                            s1 += ",[" + temptimeData[i] + "," + tempAmount[i] + "]";
                            s2 += ",[" + temptimeData[i] + "," + tempAmountTf1[i] + "]";
                            s3 += ",[" + temptimeData[i] + "," + tempAmountTf2[i] + "]";
                        }
                    }
                    //JavaScriptSerializer fg = new JavaScriptSerializer();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "upitStat('" + s1 + "','" + s2 + "','" + s3 + "','" + title1 + "','" + title2 + "','" + title3 + "','" + titleY + "','" + GraphQuanityType.Text + "')", true);
                }
                if (flag1 == true)
                {
                    if (kount > 0)
                    {
                        s1 = "[" + temptimeData[0] + "," + tempAmount[0].ToString() + "]";

                        for (int i = 1; i < kount; i++)
                        {
                            s1 += ",[" + temptimeData[i] + "," + tempAmount[i] + "]";

                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "upitStat1('" + s1 + "','" + title1 + "','" + titleY + "','" + GraphQuanityType.Text + "')", true);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error); errLog.Text = ex.Message;
            }
        }
        protected void addThisMeter_Click(object sender, EventArgs e)
        {
            try
            {
                if (meterSrrl.Text.Length != 0)
                {
                    bool flag = false;
                    string comp = meterSrrl.Text;
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT subsubsubutility FROM meter where subsubsubutility = '" + meterSrrl.Text + "';", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (comp == (string)(dr["subsubsubutility"]))
                                {
                                    flag = true;
                                }
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    if (!flag)
                    {
                        int n = 0;
                        using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                        {
                            connection.Open();
                            using (OdbcCommand command = new OdbcCommand("SELECT id FROM meter;", connection))
                            using (OdbcDataReader dr = command.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    n = (int)(dr["id"]);
                                }
                                dr.Close();
                            }
                            connection.Close();
                        }
                        n++;
                        DateTime DT = new DateTime();
                        using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                        {
                            connection.Open();
                            using (OdbcCommand command = new OdbcCommand("INSERT INTO meter (id,utility, subutility, subsubutility,subsubsubutility,deviceType) VALUES (" + n + ", '" + Utility.Text +"', '" + UnitList.Text + "', '" + MachineList.Text + "', '" + comp + "', '" + DropDownListMeterType.Text + "')", connection))

                                command.ExecuteNonQuery();
                            connection.Close();
                        }

                        ExecuteNonQurey("delete from meterinventory where serial = '" + meterSrrl.Text + "';");
                        loadsmetersf();
                        upmeterSrrl.Update();
                        ShowMessage("Meter Added!", MessageType.Success);
                    }
                    else
                    {
                        ShowMessage("Meter Already added to the network.", MessageType.Error);
                    }
                }
                else
                {
                    ShowMessage("Please specify a valid and Unique Meter Serial", MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error); errLog.Text = ex.Message;
            }
        }
        protected void GridViewAutoReg_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)(Session["AutoDT"]);

                GridViewAutoReg.PageIndex = e.NewPageIndex;
                GridViewAutoReg.Font.Size = FontUnit.Smaller;
                GridViewAutoReg.DataSource = dt;
                GridViewAutoReg.DataBind();
            }
            catch(Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void LinkButtonAutoRegSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt;
                dt = new DataTable();
                dt.Columns.Add("Meter Serial");
                dt.Columns.Add("Last Seen");
                string query = "SELECT * FROM unregistered order by Time_stamp desc;";

                if(!(SearchBoxAutoReg.Value == ""))
                {
                    query = "SELECT * FROM unregistered where serial = '" + SearchBoxAutoReg.Value + "' order by Time_stamp desc;";
                }

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DataRow drr = dt.NewRow();
                            drr["Meter Serial"] = dr["serial"].ToString();
                            drr["Last Seen"] = ((DateTime)(dr["Time_stamp"])).ToString("yyyy/MM/dd HH:mm:ss");
                            dt.Rows.Add(drr);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }
                GridViewAutoReg.Font.Size = FontUnit.Smaller;
                GridViewAutoReg.DataSource = dt;
                GridViewAutoReg.DataBind();

                Session["AutoDT"] = dt;

                gridAutoRegUPanel.Update();
            }
            catch(Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void LoadDropDownList()
        {
            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("SELECT utility FROM dingrail.meter group by utility;", connection))
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string parent = reader.IsDBNull(0) ? "" : reader.GetString(0);
                        if (!string.IsNullOrEmpty(parent))
                        {
                            Parentnode.Items.Add(parent);
                        }
                    }
                    reader.Close();
                }
                connection.Close();
            }
            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("SELECT subutility FROM dingrail.meter group by subutility;", connection))
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string child = reader.IsDBNull(0) ? "" : reader.GetString(0);
                        if (!string.IsNullOrEmpty(child))
                        {
                            childnode.Items.Add(child);
                        }
                    }
                    reader.Close();
                }
                connection.Close();
            }
            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("SELECT subsubutility FROM dingrail.meter group by subsubutility;", connection))
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {


                        string grandchild = reader.IsDBNull(0) ? "" : reader.GetString(0);

                        if (!string.IsNullOrEmpty(grandchild))
                        {
                            Grandchild.Items.Add(grandchild);
                        }
                    }
                    reader.Close();
                }
                connection.Close();
            }
        }

        protected void LinkButtonAddAutoMeter_Click(object sender, EventArgs e)
        {
            try
            {
                string comp = GridViewAutoReg.SelectedRow.Cells[1].Text;
                if (comp == "")
                {
                    ShowMessage("No selection Made.", MessageType.Error);
                    return;
                }

                bool flag = false;
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT subsubsubutility FROM meter WHERE subsubsubutility = '" + comp + "'", connection))

                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            flag = true;
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                if (!flag)
                {
                    int n = 0;
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT id FROM meter;", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                n = (int)(dr["id"]);
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }
                    n++;
                    // Populating the hierarchical list


                    DateTime DT = new DateTime();
                    int index1 = Parentnode.SelectedIndex;
                    int index2 = childnode.SelectedIndex;
                    int index3 = Grandchild.SelectedIndex;

                    if (index1 >= 0 && index2 >= 0 && index3 >= 0)
                    {
                        string utility = Parentnode.Items[index1].ToString();
                        string subutility = childnode.Items[index2].ToString();
                        string subsubutility = Grandchild.Items[index3].ToString();

                        using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                        {

                            connection.Open();

                            using (OdbcCommand command = new OdbcCommand("INSERT INTO meter (id, utility, subutility, subsubutility,subsubsubutility, deviceType) VALUES (?, ?, ?, ?, ?, ?)", connection))
                            {
                                command.Parameters.AddWithValue("@id", n);
                                command.Parameters.AddWithValue("@utility", utility);
                                command.Parameters.AddWithValue("@subutility", subutility);
                                command.Parameters.AddWithValue("@subsubutility", subsubutility);
                                command.Parameters.AddWithValue("@subsubsubutility", comp);
                                command.Parameters.AddWithValue("@deviceType", DropDownListAutoMeterType.Text);
                                command.ExecuteNonQuery();
                            }

                            connection.Close();
                        }
                    }


                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("DELETE FROM unregistered where serial='" + comp + "';", connection))
                            command.ExecuteNonQuery();
                        connection.Close();
                    }

                    ShowMessage("Meter successfully added to Network.", MessageType.Success);
                }
                else
                {
                    ShowMessage("Meter already added to Network.", MessageType.Error);
                }


            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error); errLog.Text = ex.Message;
            }
        }

        protected void searchByUsername_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM users1;";
                if(searchBoxUsers.Value.Length > 0)
                {
                    query = "SELECT * FROM users1 where username = '"+searchBoxUsers.Value+"';";
                }
                DataTable dt;
                dt = new DataTable();
                dt.Columns.Add("Username");
                dt.Columns.Add("Authorization Level");
                dt.Columns.Add("Date Created");
                dt.Columns.Add("Name");
                dt.Columns.Add("Contact");
                dt.Columns.Add("C.N.I.C");
                dt.Columns.Add("e-mail");
                dt.Columns.Add("Last Online");

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DataRow drr = dt.NewRow();
                            drr["Username"] = dr["username"].ToString();
                            drr["Authorization Level"] = dr["typ"].ToString();
                            drr["Date Created"] = dr["dateCreated"].ToString();
                            drr["Name"] = dr["name"].ToString();
                            drr["Contact"] = dr["contact"].ToString();
                            drr["C.N.I.C"] = dr["cnic"].ToString();
                            drr["e-mail"] = dr["email"].ToString();
                            drr["Last Online"] = ((string)(dr["lastOnline"]));
                            dt.Rows.Add(drr);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }
                GridviewUserlist.Font.Size = FontUnit.Smaller;
                GridviewUserlist.DataSource = dt;
                GridviewUserlist.DataBind();

                Session["UsersDT"] = dt;

                UPanelUsers.Update();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void searchByName_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM users1;";
                if (searchBoxUsers.Value.Length > 0)
                {
                    query = "SELECT * FROM users1 where name = '" + searchBoxUsers.Value + "';";
                }
                DataTable dt;
                dt = new DataTable();
                dt.Columns.Add("Username");
                dt.Columns.Add("Authorization Level");
                dt.Columns.Add("Date Created");
                dt.Columns.Add("Name");
                dt.Columns.Add("Contact");
                dt.Columns.Add("C.N.I.C");
                dt.Columns.Add("e-mail");
                dt.Columns.Add("Last Online");

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DataRow drr = dt.NewRow();
                            drr["Username"] = dr["username"].ToString();
                            drr["Authorization Level"] = dr["typ"].ToString();
                            drr["Date Created"] = dr["dateCreated"].ToString();
                            drr["Name"] = dr["name"].ToString();
                            drr["Contact"] = dr["contact"].ToString();
                            drr["C.N.I.C"] = dr["cnic"].ToString();
                            drr["e-mail"] = dr["email"].ToString();
                            drr["Last Online"] = ((string)(dr["lastOnline"]));
                            dt.Rows.Add(drr);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }
                GridviewUserlist.Font.Size = FontUnit.Smaller;
                GridviewUserlist.DataSource = dt;
                GridviewUserlist.DataBind();

                Session["UsersDT"] = dt;

                UPanelUsers.Update();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void searchByContact_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM users1;";
                if (searchBoxUsers.Value.Length > 0)
                {
                    query = "SELECT * FROM users1 where contact = '" + searchBoxUsers.Value + "';";
                }
                DataTable dt;
                dt = new DataTable();
                dt.Columns.Add("Username");
                dt.Columns.Add("Authorization Level");
                dt.Columns.Add("Date Created");
                dt.Columns.Add("Name");
                dt.Columns.Add("Contact");
                dt.Columns.Add("C.N.I.C");
                dt.Columns.Add("e-mail");
                dt.Columns.Add("Last Online");

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DataRow drr = dt.NewRow();
                            drr["Username"] = dr["username"].ToString();
                            drr["Authorization Level"] = dr["typ"].ToString();
                            drr["Date Created"] = dr["dateCreated"].ToString();
                            drr["Name"] = dr["name"].ToString();
                            drr["Contact"] = dr["contact"].ToString();
                            drr["C.N.I.C"] = dr["cnic"].ToString();
                            drr["e-mail"] = dr["email"].ToString();
                            drr["Last Online"] = ((string)(dr["lastOnline"]));
                            dt.Rows.Add(drr);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }
                GridviewUserlist.Font.Size = FontUnit.Smaller;
                GridviewUserlist.DataSource = dt;
                GridviewUserlist.DataBind();

                Session["UsersDT"] = dt;

                UPanelUsers.Update();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void searchByCNIC_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM users1;";
                if (searchBoxUsers.Value.Length > 0)
                {
                    query = "SELECT * FROM users1 where cnic = '" + searchBoxUsers.Value + "';";
                }
                DataTable dt;
                dt = new DataTable();
                dt.Columns.Add("Username");
                dt.Columns.Add("Authorization Level");
                dt.Columns.Add("Date Created");
                dt.Columns.Add("Name");
                dt.Columns.Add("Contact");
                dt.Columns.Add("C.N.I.C");
                dt.Columns.Add("e-mail");
                dt.Columns.Add("Last Online");

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DataRow drr = dt.NewRow();
                            drr["Username"] = dr["username"].ToString();
                            drr["Authorization Level"] = dr["typ"].ToString();
                            drr["Date Created"] = dr["dateCreated"].ToString();
                            drr["Name"] = dr["name"].ToString();
                            drr["Contact"] = dr["contact"].ToString();
                            drr["C.N.I.C"] = dr["cnic"].ToString();
                            drr["e-mail"] = dr["email"].ToString();
                            drr["Last Online"] = ((string)(dr["lastOnline"]));
                            dt.Rows.Add(drr);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }
                GridviewUserlist.Font.Size = FontUnit.Smaller;
                GridviewUserlist.DataSource = dt;
                GridviewUserlist.DataBind();

                Session["UsersDT"] = dt;

                UPanelUsers.Update();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void btnsearchUser_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM users1;";
                if (searchBoxUsers.Value.Length > 0)
                {
                    query = "SELECT * FROM users1 where username = '" + searchBoxUsers.Value + "';";
                }
                DataTable dt;
                dt = new DataTable();
                dt.Columns.Add("Username");
                dt.Columns.Add("Authorization Level");
                dt.Columns.Add("Date Created");
                dt.Columns.Add("Name");
                dt.Columns.Add("Contact");
                dt.Columns.Add("C.N.I.C");
                dt.Columns.Add("e-mail");
                dt.Columns.Add("Last Online");

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DataRow drr = dt.NewRow();
                            drr["Username"] = dr["username"].ToString();
                            drr["Authorization Level"] = dr["typ"].ToString();
                            drr["Date Created"] = dr["dateCreated"].ToString();
                            drr["Name"] = dr["name"].ToString();
                            drr["Contact"] = dr["contact"].ToString();
                            drr["C.N.I.C"] = dr["cnic"].ToString();
                            drr["e-mail"] = dr["email"].ToString();
                            drr["Last Online"] = ((string)(dr["lastOnline"]));
                            dt.Rows.Add(drr);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }
                GridviewUserlist.Font.Size = FontUnit.Smaller;
                GridviewUserlist.DataSource = dt;
                GridviewUserlist.DataBind();

                Session["UsersDT"] = dt;

                UPanelUsers.Update();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void GridviewUserlist_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt = (DataTable)(Session["UsersDT"]);

            GridviewUserlist.PageIndex = e.NewPageIndex;
            GridviewUserlist.Font.Size = FontUnit.Smaller;
            GridviewUserlist.DataSource = dt;
            GridviewUserlist.DataBind();
        }

        protected void LinkButtonAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                int asst = 0, load = 0, arch = 0, readings = 0, configuration = 0;

                string message = "New User Account Created! Username:" + addAccUsername.Value + " Password:" + addAccPassword.Value;
                if (addAccUsername.Value.Length < 6 || addAccPassword.Value.Length < 6)
                {
                    message = "Error! Please select a secure Username & Password";
                    ShowMessage(message, MessageType.Info);
                    return;
                }
                if (addAccPassword.Value != addAccConfirmPassword.Value)
                {
                    message = "Error! Password Mismatch";
                    ShowMessage(message, MessageType.Info);
                    return;
                }
                if (accaddcnic.Value.Length < 13 || addAccountphone.Value.Length < 10)
                {
                    message = "Please enter a valid C.N.I.C and Contact number.";
                    ShowMessage(message, MessageType.Info);
                    return;
                }
                if (addAccountName.Value.Length < 1)
                {
                    message = "Name field cannot be Empty!";
                    ShowMessage(message, MessageType.Info);
                    return;
                }
                if (!(addaccemail.Value.Contains('@') && addaccemail.Value.Contains('.')))
                {
                    addaccemail.Value = "N/A";
                }

                string auth = usrtyp.SelectedValue;

                if (usrtyp.SelectedValue == "User")
                {
                    readings = 1;
                    arch = 1;
                }

                if (usrtyp.SelectedValue == "Admin")
                {
                    readings = 1;
                    arch = 1;
                    asst = 1;
                    configuration = 1;
                    load = 1;
                }

                if (usrtyp.SelectedValue == "Power User")
                {
                    readings = 1;
                    arch = 1;
                    asst = 1;
                    load = 1;
                    configuration = 1;
                }

                bool there = false;
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("select username from users1 where username = '" + addAccUsername.Value + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            there = true;
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                if (there)
                {
                    message = "Username Already Exists!";
                    ShowMessage(message, MessageType.Info);
                    return;
                }

                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("insert into users1(username,password,log,typ,keycode,lastOnline,dateCreated,name,contact,cnic,email,asset,loadManage,archive,reading,parameter) values('" + addAccUsername.Value + "','" + addAccPassword.Value + "',0,'" + auth + "','0','N/A','" + DateTime.Now.ToString() + "','" + addAccountName.Value + "','" + addAccountphone.Value + "','" + accaddcnic.Value + "','" + addaccemail.Value + "','" + asst + "','" + load + "','" + arch + "','" + readings + "','" + configuration + "');", connection))
                            command.ExecuteNonQuery();
                        connection.Close();
                    }

                    message = "Account Created. Username:" +addAccUsername.Value + " Password:" + addAccPassword.Value;
                    ShowMessage(message, MessageType.Info);
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void LinkButtonRemoveUser_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "No user Selected";
                string user = selecteduser.Text;

                if (user.Length > 1)
                {
                    if (selectedusertype.Text == "Admin" && (string)(Session["username"]) != user)
                    {
                        message = "You do NOT have authorization to remove this user!";
                        ShowMessage(message, MessageType.Error);
                    }
                    else
                    {
                        using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                        {
                            connection.Open();
                            using (OdbcCommand command = new OdbcCommand("delete from users1 where username = '" + user + "';", connection))
                                command.ExecuteNonQuery();
                            connection.Close();
                        }

                        message = "Account Deleted!";
                        ShowMessage(message, MessageType.Info);
                    }
                }
                else
                { ShowMessage(message, MessageType.Error); }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void GridviewUserlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            selecteduser.Text = GridviewUserlist.SelectedRow.Cells[1].Text;
            selectedusertype.Text = GridviewUserlist.SelectedRow.Cells[2].Text;
        }

        protected void LinkButtonAddPermissions_Click(object sender, EventArgs e)
        {
            try
            {
                if (selecteduser.Text.Length < 1) { ShowMessage("Please select an Account.", MessageType.Error); return; }
                if (!(selectedusertype.Text == "Admin"))
                {
                    string authorization = "";
                    int asst = 0, load = 0, arch = 0, readings = 0, configuration = 0;
                    foreach (System.Web.UI.WebControls.ListItem li in checklistPermissions.Items)
                    {
                        if (li.Selected)
                        {
                            switch (li.Value)
                            {
                                case "Asset Managemnet":
                                    asst = 1;
                                    break;
                                case "Load Management":
                                    load = 1;
                                    break;
                                case "Archives":
                                    arch = 1;
                                    break;
                                case "Real-Time Readings":
                                    readings = 1;
                                    break;
                                case "Device Configuration":
                                    configuration = 1;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (configuration == 1 || load == 1 || asst == 1)
                    {
                        authorization = "Power User";
                    }
                    else
                    {
                        authorization = "User";
                    }

                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("UPDATE users1 SET parameter ='" + configuration + "',asset = '" + asst + "',typ = '" + authorization + "',reading = '" + readings + "',archive = '" + arch + "',loadManage='" + load + "' WHERE username ='" + selecteduser.Text + "';", connection))
                            command.ExecuteNonQuery();
                        connection.Close();
                    }

                    string message = "Permissions Updated! Authorization Group:" + authorization + " Asset Managemet:" + stat(asst) + " Load Managemet:" + stat(load) + " Archives:" + stat(arch) + " Real-Time Readings:" + stat(arch) + " Parameterization/Configuration:" + stat(configuration);
                    ShowMessage(message, MessageType.Info);
                }
                else
                {
                    string mussage = "You are NOT authorized to manage this User!";
                    ShowMessage(mussage, MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected string stat(int t)
        {
            if (t == 1)
            {
                return "Access Granted";
            }
            else if (t == 0)
            {
                return "Access Revoked";
            }

            return "Status Unavailable";
        }

        protected void configure_Click(object sender, EventArgs e)
        {
            try
            {
                if (usernameAccEdit.Value.Length < 1 || oldpassword.Value.Length < 1 || newpassword.Value.Length < 1)
                {
                    ShowMessage("Please fill Username and Password Fields.", MessageType.Error);
                    return;
                }
                if (newpassword.Value.Length < 6) { ShowMessage("Password must be atleast 6 character/digits long.", MessageType.Error); return; }
                if (!Regex.IsMatch(oldpassword.Value, @"^[a-zA-Z0-9@.]*$") || !Regex.IsMatch(newpassword.Value, @"^[a-zA-Z0-9@.]*$")) { ShowMessage("Please use only letters and numbers for password.", MessageType.Error); return; }
                
                bool ishere = false;
                string passwordddaa = "";
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("select * from users1 where username = '" + usernameAccEdit.Value + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ishere = true;
                            passwordddaa = (string)(dr["password"]);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                if (ishere == false) { ShowMessage("Username not found.", MessageType.Error); return; }
                if (passwordddaa != oldpassword.Value) { ShowMessage("Password incorrect.", MessageType.Error); return; }

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("UPDATE users1 SET password ='" + newpassword.Value + "' WHERE username ='" + usernameAccEdit.Value + "';", connection))
                        command.ExecuteNonQuery();
                    connection.Close();
                }

                ShowMessage("Password changed successfully.", MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void FileUploadComplete(object sender, EventArgs e)
        {
          
        }

        protected void btnSystemPower_Click(object sender, EventArgs e)
        {

        }

        
    
        public string ReplaceAt(string input, int index, char newChar)
        {
            if (input == null)
            {
                return "";
            }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

  
        protected void deviceGroupCreateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (deviceGroupNameTextBox.Text.Length < 1)
                { ShowMessage("Invalid Name Input", MessageType.Error); return; }

                bool t = false;
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("select * from devicegroups where id = '" + deviceGroupNameTextBox.Text + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            t = true;
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                if(t)
                {
                    ShowMessage("Group Already Exists", MessageType.Error); return;
                }

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("insert into devicegroups(id,category) values('" + deviceGroupNameTextBox.Text + "','" + groupType.Text + "');", connection))
                        command.ExecuteNonQuery();
                    connection.Close();
                }

                ShowMessage("Group Created", MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void GridViewOfTheGroups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewOfTheGroups.PageIndex = e.NewPageIndex;
            popoulateTheGroups();
        }

        protected void LinkButtonGroupSearch_Click(object sender, EventArgs e)
        {
            popoulateTheGroups();
        }

        void popoulateTheGroups()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Group Name");
                dt.Columns.Add("Group Type");
                dt.Columns.Add("Parent Node");

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("select * from devicegroups;", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DataRow dd = dt.NewRow();
                            dd["Group Name"] = dr["id"].ToString();
                            dd["Group Type"] = dr["category"].ToString();
                            dd["Parent Node"] = dr["parentNode"].ToString();
                            dt.Rows.Add(dd);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                GridViewOfTheGroups.DataSource = dt;
                GridViewOfTheGroups.DataBind();

                groupgridupanel.Update();

            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void GridViewOfTheGroups_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string rowID = GridViewOfTheGroups.SelectedRow.Cells[1].Text;

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("delete FROM meter.devicegroups where id = '"+rowID+"';", connection))
                        command.ExecuteNonQuery();
                    connection.Close();
                }

                popoulateTheGroups();
                ShowMessage("Group Deleted :" + rowID, MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void LinkButtonEditGroup_Click(object sender, EventArgs e)
        {
            try
            {
                string rowID = GridViewOfTheGroups.SelectedRow.Cells[1].Text;

                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "EditWell('" + rowID + "');", true);
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void LinkButtonEditGroupSave_Click(object sender, EventArgs e)
        {
            try
            {
                string rowID = GridViewOfTheGroups.SelectedRow.Cells[1].Text;
                string parentNodd = "Factory";
                //if (DropDownListParentNode.Text.Length > 0)
                //{
                //    parentNodd = DropDownListParentNode.Text;
                //}
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("UPDATE dingrail.meter SET subsubutility = NULL WHERE subsubutility = ?", connection))
                    {
                        command.Parameters.AddWithValue("@subsubutility", DropDownListParentNode.Text);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }



                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("update meter set subsubutility = '" + DropDownListParentNode.Text + "',utility='" + parentNodd + "' where subutility='" + rowID + "';", connection))
                        command.ExecuteNonQuery();
                    connection.Close();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "EditWellNo();", true);

                ShowMessage("Group Info Updated", MessageType.Success);

                popoulateTheGroups();
                groupgridupanel.Update();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error); errLog.Text = ex.Message;
            }
        }


        protected void LinkButtonEditGroupCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "EditWellNo();", true);
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void LinkButtonRefreshParents_Click(object sender, EventArgs e)
        {
            try
            {
                JavaScriptSerializer fg = new JavaScriptSerializer();
                string[] gorio = new string[1000];
                int cun = 0;
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("select id from devicegroups;", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            gorio[cun] = dr["id"].ToString();
                            cun++;
                            DropDownListParentNode.Items.Add(dr["id"].ToString());
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                upPanel.Update();
                /*
                if (cun == 0) { return; }

                string[] groiss = new string[cun];
                for (int i = 0; i < cun; i++)
                {
                    groiss[i] = gorio[i];
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "fillParents('" + fg.Serialize(groiss) + "');", true);*/
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        void addingGroupsAllo(string sortee, string sorta)
        {
            //switch (sorta)
            //{
            //    case "Circle":
            //        DropDownListGroupAlloCircle.Items.Add(sortee);
            //        break;
            //    case "Transformer":
            //        DropDownListTransformer.Items.Add(sortee);
            //        break;
            //    case "Feeder":
            //        DropDownListGroupAlloFeeder.Items.Add(sortee);
            //        break;
            //    case "Custom Group":
            //        DropDownListCustomGroup.Items.Add(sortee);
            //        break;
            //    case "Sub Division":
            //        DropDownListSubDivision.Items.Add(sortee);
            //        break;
            //    case "Batch":
            //        DropDownListBatch.Items.Add(sortee);
            //        break;
            //}
        }

        protected void SearchForGroupsAlloLinkButton_Click(object sender, EventArgs e)
        {
            try
            {
                DropDownListsubutility.Items.Clear();
                DropDownListMachine.Items.Clear();
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT * FROM dingrail.meter;", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DropDownListsubutility.Items.Add(dr["subutility"].ToString());
                            DropDownListMachine.Items.Add(dr["subsubutility"].ToString());
                        }
                        dr.Close();
                    }
                    connection.Close();
                }
                alloupdate.Update();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error); errLog.Text = ex.Message;
            }
        }

        protected void LinkButtonGroupsAlloToMeter_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsDigitsOnly(res.Value)) { ShowMessage("Please Select a Smart Device.", MessageType.Error); return; }

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("update meter set subutility = '" + DropDownListsubutility.Text + "', subsubutility = '" + DropDownListMachine.Text + "' where subsubsubutility = '" + res.Value + "';", connection))
                        command.ExecuteNonQuery();
                    connection.Close();
                }

                ShowMessage(res.Value + ": Device Group Info Updated.", MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error); errLog.Text = ex.Message;
            }
        }

        protected void sortTreeCircle_Click(object sender, EventArgs e)
        {
            try
            {
                customSortTree("Circle", "Three Phase");
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void sortTreeFeeder_Click(object sender, EventArgs e)
        {
            try
            {
                customSortTree("Feeder", "Three Phase");
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void sortTreeTransformer_Click(object sender, EventArgs e)
        {
            try
            {
                customSortTree("Transformer", "Three Phase");
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void sortTreeCustomGroup_Click(object sender, EventArgs e)
        {
            try
            {
                customSortTree("Custom Group","Three Phase");
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void LinkButtonEvent100Report_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)(Session["dbEventRead"]);
                //Create a dummy GridView
                GridView GridViewV = new GridView();
                GridViewV.AllowPaging = false;
                GridViewV.DataSource = dt;
                GridViewV.DataBind();

                Paragraph pa = new Paragraph("T-RECS On Demand Event Reading Report \n\n");
                pa.Alignment = Element.ALIGN_CENTER;
                pa.Font.SetStyle(Font.BOLD);
                pa.Font.Size = 14f;

                Paragraph footnote = new Paragraph("T-RECS Data Core  Copyright© Transfopower Industries Pvt Limited.");

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition",
                    "attachment;filename=Read-Data.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridViewV.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                pdfDoc.Add(pa);
                htmlparser.Parse(sr);
                pdfDoc.Add(footnote);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                Response.End();

            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void LinkButtonTreeSearch_Click(object sender, EventArgs e)
        {
            try
            {
                putOnATree(searchTextTree.Value);
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

  
  
        protected void RFDataStoreLinkButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)(Session["DTRF"]);

                if(dt.Rows.Count < 1)
                {
                    ShowMessage("Data Already Aggregated from this file.", MessageType.Error); return;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "removeRFList();", true);

                foreach(DataRow dr in dt.Rows)
                {
                    string ty = "";
                    if (dr["ci"].ToString() == "2")
                    {
                        using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                        {
                            connection.Open();
                            using (OdbcCommand command = new OdbcCommand("insert into rfreadingb(serial,HHUDT,CI,PresentDT,kWh,mdi,kW) values('" + dr["meter serial number"].ToString() + "','" + dr["hhu date"].ToString() + " " + dr["hhu time"].ToString() + "','" + dr["ci"].ToString() + "','" + dr["present date"].ToString() + " " + dr["present time"].ToString() + "','" + dr["tl active kwh"].ToString() + "','" + dr["tl active mdi"].ToString() + "','" + dr["instantaneous active power"].ToString() + "');", connection))
                                command.ExecuteNonQuery();
                            connection.Close();
                        }

                        ty = " Billing";
                    }

                    if (dr["ci"].ToString() == "3")
                    {
                        using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                        {
                            connection.Open();
                            using (OdbcCommand command = new OdbcCommand("insert into rfreadingi(serial,HHUDT,CI,PresentDT,kW,mdi) values('" + dr["meter serial number"].ToString() + "','" + dr["hhu date"].ToString() + " " + dr["hhu time"].ToString() + "','" + dr["ci"].ToString() + "','" + dr["present date"].ToString() + " " + dr["present time"].ToString() + "','" + dr["active power p1"].ToString() + "','" + dr["current month active mdi tl"].ToString() + "');", connection))
                                command.ExecuteNonQuery();
                            connection.Close();
                        }
                        ty = " Instantaneous";
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addItRF('RF Meter: " + dr["meter serial number"].ToString() + ty +" Data Integrated');", true);
                }

                Session["DTRF"] = new DataTable();


                ShowMessage("RF Data Uploaded to server.", MessageType.Success);
            }
            catch(Exception ex)
            {
                ShowMessage("Invalid Data File.", MessageType.Error);
            }
        }

        protected void makeAndRFTree()
        {
            TreeNode RFNET = new TreeNode("Utility Co");
            TreeNode RFsubNet = new TreeNode("Network");

            string users = "[";
            string ausers = "[";
            string dusers = "[";

            string userst = "[";
            string auserst = "[";
            string duserst = "[";
            int t = 0;

            using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand("select * from rfmeter;", connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        TreeNode f = new TreeNode(dr["serial"].ToString());
                        f.ImageUrl = "heartbeat.png";
                        RFsubNet.ChildNodes.Add(f);
                        ausers += "'" + dr["serial"].ToString() + "',";
                        auserst += "'Integrated',";
                        t++;
                    }
                    dr.Close();
                }
                connection.Close();
            }

            dusers += "'None']";
            duserst += "'None']";
            users += "'None']";
            userst += "'None']";


            if (t < 1)
            {
                ausers += "'None']";
                auserst += "'None']";
            }
            else
            {
                ausers = ReplaceAt(ausers, ausers.Length - 1, ']');
                auserst = ReplaceAt(auserst, auserst.Length - 1, ']');
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addTouu(" + users + "," + ausers + "," + dusers + "," + userst + "," + auserst + "," + duserst + ",'Total Devices(" + t + ")','RF Network');", true);
        }

        protected void btnRFTree_Click(object sender, EventArgs e)
        {
            try
            {
                makeAndRFTree();
            }
            catch(Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

    
        protected void btnSPCircle_Click(object sender, EventArgs e)
        {
            try
            {
                customSortTree("Circle", "Single Phase");
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void btnSPFeeder_Click(object sender, EventArgs e)
        {
            try
            {
                customSortTree("Feeder", "Single Phase");
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void btnSPTransformer_Click(object sender, EventArgs e)
        {
            try
            {
                customSortTree("Transformer", "Single Phase");
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void btnSPCGroup_Click(object sender, EventArgs e)
        {
            try
            {
                customSortTree("Custom Group", "Single Phase");
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);errLog.Text = ex.Message;
            }
        }

        protected void timerForPopup_Tick(object sender, EventArgs e)
        {
            try
            {
                int code = 0;
                string serial = "";
                DateTime DT = new DateTime();
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT code,serial,Time_stamp FROM popup1 where serviced = 1 order by Time_stamp desc limit 1;", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            serial = (string)(dr["serial"]);
                            code = (int)(dr["code"]);
                            DT = (DateTime)(dr["Time_stamp"]);
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                if (code != 0)
                {
                    int major = 0;
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("SELECT major FROM alarms1 where code = " + code + ";", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                major = (int)(dr["major"]);
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("update popup1 set serviced = 0 where serial = '" + serial + "' and Time_stamp = '" + String.Format("{0:yyyy/M/d HH:mm:ss}", DT) + "';", connection))
                            command.ExecuteNonQuery();
                        connection.Close();
                    }

                    if (major == 1)
                    {
                        string mussage = "ALARM!!! Serial:" + serial + " Event Code:" + code + " Description:" + eventCodeTranslation(code.ToString()) + " DateTime Stamp:" + DT.ToString();
                        ShowMessage(mussage, MessageType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Warning);
            }
        }

        protected void timerForFD_Tick(object sender, EventArgs e)
        {
           try
           {
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "removeFDList();", true);
                string hu = "";
                bool isnot = false;
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT * FROM fdevices;", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            isnot = true;
                            if (DateTime.Now.Subtract((DateTime)(dr["lastcomm"])) < TimeSpan.Parse("00:010:20.9896330"))
                            { ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addItFD('" + dr["deviceID"].ToString() + "','" + dr["rssi"].ToString() + "','" + dr["connection"].ToString() + "','" + dr["lastcomm"].ToString() + "');", true); }
                            else
                            { ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addItFDL('" + dr["deviceID"].ToString() + "','" + dr["rssi"].ToString() + "','" + dr["connection"].ToString() + "','" + dr["lastcomm"].ToString() + "');", true); }
                            
                        }
                        dr.Close();
                    }
                    connection.Close();
                }
               if(!isnot)
               {
                   ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "SItFD();", true);
               }
           }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Warning);
            }
        }

     
 
  
 
     
 
        protected void LinkButtonLoadRecentAlarms_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "emptyaddit();", true);

                int[] ho = new int[30];
                int cou = 0;
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT * FROM alarms1 where major = 1;", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ho[cou] = (int)(dr["code"]);
                            cou++;
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT * from eventlist1 where Time_stamp between '" + DateTime.Now.AddHours(-1).ToString("yyyy/M/d HH:mm:ss") + "' and '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            for (int i = 0; i < cou;i++ )
                            {
                                if(ho[i]==int.Parse(dr["eventCode"].ToString()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "addIteva('Serail: " + dr["msn"].ToString() + "Event Code: " + dr["eventCode"].ToString() + "Time Stamp: " + dr["Time_stamp"].ToString() + "');", true);
                                }
                            }
                        }
                        dr.Close();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Warning);
            }
        }

        protected void ss_Click(object sender, EventArgs e)
        {
            try
            {
                eventLogPopulate();
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "alert('"+ex.Message+"');", true);
            }
        }

        protected void btncommT_Click(object sender, EventArgs e)
        {
            try
            {
                CommunicationsLogLogPopulate();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + ex.Message + "');", true);
            }
        }

        protected void btnactionT_Click(object sender, EventArgs e)
        {
            try
            {
                ActionsLogLogPopulate();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + ex.Message + "');", true);
            }
        }

        protected void btnaccessT_Click(object sender, EventArgs e)
        {
            try
            {
                AccessLogLogPopulate();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + ex.Message + "');", true);
            }
        }

        protected void LinkButtonRefreshTree_Click(object sender, EventArgs e)
        {
            try
            {
                populateTreeView();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + ex.Message + "');", true);
            }
        }

        protected void pinpoint_Click(object sender, EventArgs e)
        {
            try
            {
                if (res.Value != "" && res.Value != null && IsDigitsOnly(res.Value))
                {
                    float lat = 0;
                    float longitude = 0;
                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("select lat,longituge from meter1 where serial = '" + res.Value + "';", connection))
                        using (OdbcDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lat = (float)(dr["lat"]);
                                longitude = (float)(dr["longituge"]);
                            }
                            dr.Close();
                        }
                        connection.Close();
                    }

                    string info = "TPI-34G MSN:" + res.Value;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "setnewmark(" + lat + "," + longitude + ",'" + info + "');", true);
                }
                else
                {
                    ShowMessage("Please select a field device.", MessageType.Error);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + ex.Message + "');", true);
            }
        }

        protected void generateCustomReport_Click(object sender, EventArgs e)
        {
            try
            {
                string tuup = "Custom_Reports";
                DBGetSet db = new DBGetSet();
                DataTable dt = new DataTable();
                string r1 = Text1.Value;
                string r2 = Text2.Value;
                string strtsrl = fu.Text;
                string endsrl = fu1.Text;
                string constQ = "OBIS = ''";
                if(kWhcheck.Checked)
                {
                    constQ += " or OBIS = '" + kWhcheck.ToolTip+"' ";
                }
                if(kVARhcheck.Checked)
                {
                    constQ += " or OBIS = '" + kVARhcheck.ToolTip + "' ";
                }
                if (pfcheck.Checked)
                {
                    constQ += " or OBIS = '" + pfcheck.ToolTip + "' ";
                }
                if (mdiabscheck.Checked)
                {
                    constQ += " or OBIS = '" + mdiabscheck.ToolTip + "' ";
                }
                if (mdicumcheck.Checked)
                {
                    constQ += " or OBIS = '" + mdicumcheck.ToolTip + "' ";
                }
                if (mdirdatecheck.Checked)
                {
                    constQ += " or OBIS = '" + mdirdatecheck.ToolTip + "' ";
                }
                if (mdircountcheck.Checked)
                {
                    constQ += " or OBIS = '" + mdircountcheck.ToolTip + "' ";
                }

                db.Query = "SELECT Amount, unit, OBIS, id, Time_stamp, COUNT(*) AS ct FROM info1 WHERE (("+constQ+") and (id between '"+strtsrl+"' and '"+endsrl+"') and (Time_stamp between '"+r1+"' and '"+r2+"')) and Time_stamp IN (SELECT MAX(Time_stamp) FROM info1 where quantity = 0 GROUP by id) GROUP BY id,OBIS ORDER BY id;";
                dt = db.ExecuteReader();
                see.Text = db.Query;
                DataTable csvt = new DataTable();
                csvt.Columns.Add("Serial");
                csvt.Columns.Add("Quantity");
                csvt.Columns.Add("Value");
                csvt.Columns.Add("Unit");
                csvt.Columns.Add("DateTime");
                if(includeOBIScheck.Checked)
                {
                    csvt.Columns.Add("OBIS");
                }
                if(dt.Rows.Count > 0)
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        DataRow drr = csvt.NewRow();
                        drr["Serial"] = dr["id"];
                        drr["Quantity"] = OBISTranslation(dr["OBIS"].ToString());
                        drr["Value"] = dr["Amount"];
                        drr["Unit"] = dr["unit"];
                        drr["DateTime"] = dr["Time_stamp"];
                        if (includeOBIScheck.Checked)
                        {
                            drr["OBIS"] = dr["OBIS"];
                        }
                        csvt.Rows.Add(drr);
                    }
                }

                if(pfcheck.Checked)
                {
                    db.Query = "SELECT Amount, unit, OBIS, id, Time_stamp, COUNT(*) AS ct FROM info1 WHERE ((" + constQ + ") and (id between '" + strtsrl + "' and '" + endsrl + "') and (Time_stamp between '" + r1 + "' and '" + r2 + "')) and Time_stamp IN (SELECT MAX(Time_stamp) FROM info1 where quantity = 1 GROUP by id) GROUP BY id,OBIS ORDER BY id;";
                    dt = db.ExecuteReader();

                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow drr = csvt.NewRow();
                        drr["Serial"] = dr["id"];
                        drr["Quantity"] = OBISTranslation(dr["OBIS"].ToString());
                        drr["Value"] = dr["Amount"];
                        drr["Unit"] = dr["unit"];
                        drr["DateTime"] = dr["Time_stamp"];
                        if (includeOBIScheck.Checked)
                        {
                            drr["OBIS"] = dr["OBIS"];
                        }
                        csvt.Rows.Add(drr);
                    }
                }

                string csv = string.Empty;

                foreach (DataColumn column in csvt.Columns)
                {
                    csv += column.ColumnName + ',';
                }


                csv += "\r\n";

                foreach (DataRow row in csvt.Rows)
                {
                    foreach (DataColumn column in csvt.Columns)
                    {

                        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                    }


                    csv += "\r\n";
                }


                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + tuup + "_" + res.Value + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(csv);
                Response.Flush();
                Response.End();
            }
            catch(Exception ex)
            {
                see.Text = ex.ToString();
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BindData();
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string BatchID = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblBatchID")).Text;
                string FTPPath = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtFTPPath")).Text;
                string Username = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtUsername")).Text;
                string Password = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtPassword")).Text;
                string UploadTime = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtUploadTime")).Text;
                string UploadFrequency = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtUploadFrequency")).Text;

                DBGetSet db = new DBGetSet();
                db.Query = "update exportbatch set FTPPath = '" + FTPPath + "',Username = '" + Username + "',Password = '" + Password + "',UploadTime = '" + UploadTime + "',UploadFrequency = '" + UploadFrequency + "' where BatchID = '" + BatchID + "';";
                db.ExecuteNonQuery();
                db.Query = "SELECT * FROM exportbatch;";

                GridView1.EditIndex = -1;
                GridView1.DataSource = db.ExecuteReader();
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                seee.Text = ex.ToString();
                ShowMessage("Invalid Parameters", MessageType.Error);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string BatchID = ((TextBox)GridView1.FooterRow.FindControl("txtBatchID")).Text;
                string FTPPath = ((TextBox)GridView1.FooterRow.FindControl("txtFTPPath")).Text;
                string Username = ((TextBox)GridView1.FooterRow.FindControl("txtUsername")).Text;
                string Password = ((TextBox)GridView1.FooterRow.FindControl("txtPassword")).Text;
                string UploadTime = ((TextBox)GridView1.FooterRow.FindControl("txtUploadTime")).Text;
                string UploadFrequency = ((TextBox)GridView1.FooterRow.FindControl("txtUploadFrequency")).Text;

                DBGetSet db = new DBGetSet();
                db.Query = "insert into exportbatch(BatchID,FTPPath,Username,Password,UploadTime,UploadFrequency) values('" + BatchID + "','" + FTPPath + "','" + Username + "','" + Password + "','" + UploadTime + "','" + UploadFrequency + "')";
                db.ExecuteNonQuery();
                db.Query = "SELECT * FROM exportbatch;";

                GridView1.DataSource = db.ExecuteReader();
                GridView1.DataBind();
            }
            catch(Exception ex)
            {
                ShowMessage("Invalid Parameters.", MessageType.Error);
            }
        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            LinkButton lnkRemove = (LinkButton)sender;
            DBGetSet db = new DBGetSet();
            db.Query = "delete from exportbatch where BatchID = '" + lnkRemove.CommandArgument + "';";
            db.ExecuteNonQuery();
            db.Query = "SELECT * FROM exportbatch;";
            GridView1.DataSource = db.ExecuteReader();
            GridView1.DataBind();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindData();
        }

        protected void ftpLinkButtonGenerateBatchFiles_Click(object sender, EventArgs e)
        {
            try
            {
                if(batches.Text == null || batches.Text == "" || res.Value == null || res.Value == "")
                {
                    ShowMessage("ERROR: An Error Occured. Please select a Device and a Batch.", MessageType.Error);
                    return;
                }

                DBGetSet db = new DBGetSet();
                db.Query = "update meter set exportbatch = '"+batches.Text+"' where serial = '"+res.Value+"'";
                db.ExecuteNonQuery();

                ShowMessage("Meter:"+res.Value+" Added to Bactch:" + batches.Text, MessageType.Success);
            }
            catch(Exception ex)
            {
                ShowMessage("ERROR: An Error Occured.", MessageType.Error);
            }
        }

        protected void loadfiles()
        {
            //FTP Server URL.
            string ftp = "ftp://209.150.146.236:54218/";

            //FTP Folder name. Leave blank if you want to list files from root folder.
            string ftpFolder = "";

            try
            {
                //Create FTP Request.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                //Enter FTP Server credentials.
                request.Credentials = new NetworkCredential("ftptest", "");
                request.UsePassive = true;
                request.UseBinary = true;
                request.EnableSsl = false;

                //Fetch the Response and read it using StreamReader.
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                List<string> entries = new List<string>();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    //Read the Response as String and split using New Line character.
                    entries = reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                response.Close();

                //Create a DataTable.
                DataTable dtFiles = new DataTable();
                dtFiles.Columns.AddRange(new DataColumn[3] { new DataColumn("Name", typeof(string)),
                                                    new DataColumn("Size", typeof(decimal)),
                                                    new DataColumn("Date", typeof(string))});

                //Loop and add details of each File to the DataTable.
                foreach (string entry in entries)
                {
                    string[] splits = entry.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries);

                    //Determine whether entry is for File or Directory.
                    bool isFile = splits[0].Substring(0, 1) != "d";
                    bool isDirectory = splits[0].Substring(0, 1) == "d";

                    //If entry is for File, add details to DataTable.
                    if (isFile)
                    {
                        dtFiles.Rows.Add();
                        dtFiles.Rows[dtFiles.Rows.Count - 1]["Size"] = decimal.Parse(splits[4]) / 1024;
                        dtFiles.Rows[dtFiles.Rows.Count - 1]["Date"] = string.Join(" ", splits[5], splits[6], splits[7]);
                        string name = string.Empty;
                        for (int i = 8; i < splits.Length; i++)
                        {
                            name = string.Join(" ", name, splits[i]);
                        }
                        dtFiles.Rows[dtFiles.Rows.Count - 1]["Name"] = name.Trim();
                    }
                }

                //Bind the GridView.
                gvFiles.DataSource = dtFiles;
                gvFiles.DataBind();
            }
            catch (WebException ex)
            {
                throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            string fileName = (sender as LinkButton).CommandArgument;

            //FTP Server URL.
            string ftp = "ftp://116.58.54.221:54218/";

            //FTP Folder name. Leave blank if you want to Download file from root folder.
            string ftpFolder = "";

            try
            {
                //Create FTP Request.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                //Enter FTP Server credentials.
                request.Credentials = new NetworkCredential("ftptest", " ");
                request.UsePassive = true;
                request.UseBinary = true;
                request.EnableSsl = false;

                //Fetch the Response and read it into a MemoryStream object.
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                using (MemoryStream stream = new MemoryStream())
                {
                    //Download the File.
                    response.GetResponseStream().CopyTo(stream);
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
            }
            catch (WebException ex)
            {
                throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }
        }
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                BindDatac();
                GridView2.PageIndex = e.NewPageIndex;
                GridView2.DataBind();
            }
            catch(Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);
                errLog.Text = ex.Message;
            }
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GridView2.EditIndex = e.NewEditIndex;
                BindDatac();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);
                errLog.Text = ex.Message;
            }
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GridView2.EditIndex = -1;
                BindDatac();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);
                errLog.Text = ex.Message;
            }
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string id = ((Label)GridView2.Rows[e.RowIndex].FindControl("lblid")).Text;
                string areaID = ((TextBox)GridView2.Rows[e.RowIndex].FindControl("txtareaID")).Text;

                DBGetSet db = new DBGetSet();
                // db.Query = "update customergroups set areaID = '" + areaID + "',category = '" + category + "',parentNode = '" + parentNode + "' where id = '" + id + "';";
                db.ExecuteNonQuery();
                db.Query = "SELECT * FROM customergroups;";

                GridView2.EditIndex = -1;
                GridView2.DataSource = db.ExecuteReader();
                GridView2.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);
                errLog.Text = ex.Message;
            }
        }

        protected void lnkRemovec_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridView2.Rows.Count == 1) { ShowMessage("This row cannot be deleted. Please create another one before deleting.", MessageType.Info); return; }
                LinkButton lnkRemove = (LinkButton)sender;
                DBGetSet db = new DBGetSet();
                db.Query = "delete from customergroups where id = '" + lnkRemove.CommandArgument + "';";
                db.ExecuteNonQuery();
                db.Query = "SELECT * FROM customergroups;";
                GridView2.DataSource = db.ExecuteReader();
                GridView2.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);
                errLog.Text = ex.Message;
            }
        }

        protected void btnAddc_Click(object sender, EventArgs e)
        {
            try
            {
                string id = ((TextBox)GridView2.FooterRow.FindControl("txtid")).Text;
                string areaID = ((TextBox)GridView2.FooterRow.FindControl("txtareaID")).Text;

                DBGetSet db = new DBGetSet();
                //  db.Query = "insert into customergroups(id,areaID,category,parentNode) values('" + id + "','" + areaID + "','" + category + "','" + parentNode + "')";
                db.ExecuteNonQuery();
                db.Query = "SELECT * FROM customergroups;";

                GridView2.DataSource = db.ExecuteReader();
                GridView2.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);
                errLog.Text = ex.Message;
            }
        }

        protected void deleteImeter_Click(object sender, EventArgs e)
        {
            try
            {
                ExecuteNonQurey("delete from meterinventory where serial = '" + iMeter.Value + "';");
                loadsmetersf();
                upmeterSrrl.Update();
                ExcJSCommand("loadInventory();");
                ShowMessage("Device cleared from inventory",MessageType.Success);
            }
            catch(Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);
                errLog.Text = ex.Message;
            }
        }

        protected void JSScriptRun(string doit)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), doit, true);
        }

        protected void meterRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (res.Value.Length < 1) { ShowMessage("No Selection Made", MessageType.Error); return; }
                ExecuteNonQurey("delete from meter where subsubsubutility = '" + res.Value+"';");
                
                ShowMessage("Device Removed!",MessageType.Success);
                JSScriptRun("$('#refreshtree').click();");
            }
            catch(Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);
                errLog.Text = ex.Message;
            }
        }

        protected void loadsmetersf()
        {
            // Load serial numbers
            DataTable dtSerial = ExecuteReader("SELECT serial FROM meterinventory;");
            meterSrrl.Items.Clear();
            foreach (DataRow dr in dtSerial.Rows)
            {
                meterSrrl.Items.Add(dr["serial"].ToString());
            }

            // Load subsubutility
            DataTable dtSubSubUtility = ExecuteReader("SELECT subsubutility FROM dingrail.meter;");
            foreach (DataRow dr in dtSubSubUtility.Rows)
            {
                MachineList.Items.Add(dr["subsubutility"].ToString());
            }

            // Load subutility
            DataTable dtSubUtility = ExecuteReader("SELECT subutility FROM dingrail.meter;");
            foreach (DataRow dr in dtSubUtility.Rows)
            {
                UnitList.Items.Add(dr["subutility"].ToString());
            }
        }


        protected void goose_Click(object sender, EventArgs e)
        {
            try
            {
                loadsmetersf();
            }
            catch(Exception ex)
            {
                ShowMessage("An error occured. Check eLogs for details.", MessageType.Error);
                errLog.Text = ex.Message;
            }
        }
    }
}