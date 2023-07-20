using System.Configuration;
using System.Data;
using System.Data.Odbc;

namespace WcfDinRail
{

  public class DBGetSet
  {
        private string myconn = "DRIVER={MySQL ODBC 3.51 Driver};Database=dingrail;Server=localhost;Port=3306;UID=root;PWD=12345;";
        public string Query { get; set; }

    public DataTable ExecuteReader()
    {
      DataTable dataTable = new DataTable();
            using (OdbcConnection connection = new OdbcConnection(myconn))
            {
                connection.Open();
        using (OdbcCommand odbcCommand = new OdbcCommand(this.Query, connection))
        {
          using (OdbcDataReader reader = odbcCommand.ExecuteReader())
          {
            dataTable.Load((IDataReader) reader);
            reader.Close();
          }
        }
        connection.Close();
      }
      return dataTable;
    }

    public void ExecuteNonQuery()
    {
            using (OdbcConnection connection = new OdbcConnection(myconn))
            {
                connection.Open();
        using (OdbcCommand odbcCommand = new OdbcCommand(this.Query, connection))
          odbcCommand.ExecuteNonQuery();
        connection.Close();
      }
    }
  }
}
