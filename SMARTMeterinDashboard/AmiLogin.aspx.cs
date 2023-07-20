using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Net.Sockets;
using System.Net;
using System.Data.Odbc;
using System.Configuration;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public enum MessageType { Success, Error, Info, Warning };
        protected void Page_Load(object sender, EventArgs e)
        {
            up.RegisterAsyncPostBackControl(submit);
        }

        protected void SignIn()
        {
            try
            {
                int kont = 0;
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT * FROM users1 where username = '" + inputEmail3.Value + "' and password = '" + inputPassword3.Value + "';", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            kont++;
                        }
                        dr.Close();
                    }
                    connection.Close();
                }

                if (kont > 0)
                {
                    string keycode = "";
                    DateTime comp = new DateTime(2015, 12, 30, 10, 49, 07);
                    TimeSpan timeDifference = DateTime.Now - comp;
                    keycode = timeDifference.ToString();

                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("UPDATE users SET keycode ='" + keycode + "' WHERE username ='" + inputEmail3.Value + "';", connection))
                            command.ExecuteNonQuery();
                        connection.Close();
                    }

                    using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString))
                    {
                        connection.Open();
                        using (OdbcCommand command = new OdbcCommand("UPDATE users SET lastOnline ='" + DateTime.Now.ToString() + "' WHERE username ='" + inputEmail3.Value + "';", connection))
                            command.ExecuteNonQuery();
                        connection.Close();
                    }

                   

                    Session["username"] = inputEmail3.Value;
                    Session["bool"] = true;
                    Response.Redirect("main.aspx?t=" + keycode);
                }
                else
                {
                    string message = "Invalid Login Details!";
                    ShowMessage(message, MessageType.Error);
                }
            }
            catch(Exception)
            {
                string message = "Go away Hacker! No body likes you!!!";
                ShowMessage(message, MessageType.Error);
            }
        }

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            SignIn();
        }
    }
}