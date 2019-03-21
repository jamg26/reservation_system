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
        public void setRoomState(int roomid) { 
            
        }
        public void setRoomOwner(int roomid, string owner) {
            dbClass room = new dbClass();
            room.dbUpdate("UPDATE room set owner = " + owner + " WHERE id = " + roomid);
        }
        public void delRoomOwner(int roomid) { 
        
        }
        public void getRoomInfo(int roomid) { 
            
        }
    }
}
