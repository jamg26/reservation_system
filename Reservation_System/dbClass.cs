using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace Reservation_System {
    class dbClass {

        SqlConnection connect = getConnection();

        public static SqlConnection getConnection() {
            string dbServer = "35.198.198.212";
            string dbName = "Reservation_System";
            string dbUser = "sa";
            SqlConnection conn = new SqlConnection("Data Source = " + dbServer + "; Initial Catalog = " + dbName + "; User ID=" + dbUser + ";Password=Jamuel1226;");
            return conn;
        }

        public void dbInsert(string query)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.InsertCommand = new SqlCommand(query, connect);
            connect.Open();
            sda.InsertCommand.ExecuteNonQuery();
            connect.Close();
        }

        public void dbUpdate(string query)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.UpdateCommand = new SqlCommand(query, connect);
            connect.Open();
            sda.UpdateCommand.ExecuteNonQuery();
            connect.Close();
        }

        public void dbDelete(string query)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.UpdateCommand = new SqlCommand(query, connect);
            connect.Open();
            sda.UpdateCommand.ExecuteNonQuery();
            connect.Close();
        }

        public System.Data.DataTable dbSelect(string query) {
            connect.Open();
            SqlCommand cmd = new SqlCommand(query, connect);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            connect.Close();
            return dt;
        }
    }
}
