using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Reservation_System
{
    class Room
    {
        public System.Data.DataTable getRoomState() {
            System.Data.DataTable dt;
            dbClass room = new dbClass();
            dt = room.dbSelect("SELECT * FROM room");
            return dt;
        }
    }
}
