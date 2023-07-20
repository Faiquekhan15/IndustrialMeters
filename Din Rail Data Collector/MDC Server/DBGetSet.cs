using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;
using System.Configuration;

namespace MDC
{
    class DBGetSet
    {
        private string myconn = "DRIVER={MySQL ODBC 3.51 Driver};Database=dingrail;Server=localhost;Port=3306;UID=root;PWD=12345;";
        public string Query { get; set; }

        public DataTable ExecuteReader()
        {
            DataTable dt = new DataTable();
            using (OdbcConnection connection = new OdbcConnection(myconn))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand(Query, connection))
                using (OdbcDataReader dr = command.ExecuteReader())
                {
                    dt.Load(dr);
                    dr.Close();
                }
                connection.Close();
            }
            return dt;
        }

        public void ExecuteNonQuery()
        {
            using (OdbcConnection connection = new OdbcConnection(myconn))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand(Query, connection))
                    command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
