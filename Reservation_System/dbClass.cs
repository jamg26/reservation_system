using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Reservation_System {
    class dbClass {
        public static SqlConnection getConnection() {
            string dbServer = "35.198.198.212";
            string dbName = "Reservation_System";
            string dbUser = "sa";
            SqlConnection conn = new SqlConnection("Data Source = " + dbServer + "; Initial Catalog = " + dbName + "; User ID=" + dbUser + ";Password=Jamuel26;");
            return conn;
        }
    }
}
