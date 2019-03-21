using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net.Mail;

namespace Reservation_System {
    public partial class Main : Form {
        SqlConnection conn = dbClass.getConnection();
        private System.Data.DataTable clientInfo;
        public Main() {
            InitializeComponent();
        }

        private void showHide(string panel, bool state) {
            if (panel == "login") {
                groupBoxLogin.Visible = state;
            }
            if (panel == "room") {
                RoomTabTool.Visible = state;
            }
            if (panel == "menu") {
                groupBoxMenu.Visible = state;
            }
            if (panel == "client") {
                groupBoxClient.Visible = state;
            }
        }

        private void clientFieldState(bool state) {
            txtRoomOwner.Enabled = state;
            txtEmail.Enabled = state;
            txtMobile.Enabled = state;
            cmbMonth.Enabled = state;
            cmbDay.Enabled = state;
            cmbHour.Enabled = state;
            cmbMinutes.Enabled = state;
        }

        private void roomStateHandler(int row, System.Windows.Forms.PictureBox room) {
            Room roomm = new Room();
            System.Data.DataTable states = roomm.getRoomState();
            if (states.Rows[row][3].ToString() == "available") {
                room.Image = Properties.Resources.doorAvailable;
                btnReserve.Enabled = true;
                btnAvailable.Enabled = false;
                txtStatus.Text = "Available";
                clientFieldState(true);
            } 
            if (states.Rows[row][3].ToString() == "reserved") {
                room.Image = Properties.Resources.doorPending;
                btnReserve.Enabled = false;
                btnAvailable.Enabled = true;
                clientFieldState(false);
                txtStatus.Text = "Reserved";
            }
            if (states.Rows[row][3].ToString() == "occupied")  {
                room.Image = Properties.Resources.doorOccupied;
                btnReserve.Enabled = false;
                btnAvailable.Enabled = true;
                clientFieldState(false);
                txtStatus.Text = "Occupied";
            }
        }

        private void setRoomState() {
            roomStateHandler(0, room1);
            roomStateHandler(1, room2);
            roomStateHandler(2, room3);
            roomStateHandler(3, room4);
            roomStateHandler(4, room5);
            roomStateHandler(5, room6);
            roomStateHandler(6, room7);
            roomStateHandler(7, room8);
            roomStateHandler(8, room9);
            roomStateHandler(9, room10);
            roomStateHandler(10, room11);
            roomStateHandler(11, room12);
            roomStateHandler(12, room13);
            roomStateHandler(13, room14);
            roomStateHandler(14, room15);
            roomStateHandler(15, room16);
            roomStateHandler(16, room17);
            roomStateHandler(17, room18);
            roomStateHandler(18, room19);
            roomStateHandler(19, room20);
        }

        private void getRoomsCount() {
            dbClass db = new dbClass();
            System.Data.DataTable res = db.dbSelect("SELECT * FROM room WHERE state='reserved'");
            System.Data.DataTable avail = db.dbSelect("SELECT * FROM room WHERE state='available'");
            labelReservedRooms.Text = res.Rows.Count.ToString(); 
            labelAvailableRooms.Text = avail.Rows.Count.ToString(); 
        }

        private void Main_Load(object sender, EventArgs e) {
            this.Width = 300;
            showHide("login", true);
            showHide("room", false);
            showHide("menu", false);
            showHide("client", false);
            setRoomState();
            getRoomsCount();
        }

        private System.Data.DataTable loginQuery(string username, string password) {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM security WHERE email = '" + username + "' AND password = '" + password + "' collate Latin1_General_CS_AS", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            System.Data.DataTable dt = loginQuery(txtUsername.Text, txtPassword.Text);
            if (dt.Rows.Count > 0) {
                this.Width = 1030;
                if (dt.Rows[0][3].ToString() == "admin") {
                    showHide("login", false);
                    showHide("room", true);
                    showHide("menu", true);
                    labelUser.Text = dt.Rows[0][5].ToString();
                    labelUserType.Text = dt.Rows[0][3].ToString();
                    linkEditUsers.Visible = true;
                    MessageBox.Show("Welcome admin!");
                } else {
                    showHide("login", false);
                    showHide("room", true);
                    showHide("menu", true);
                    labelUser.Text = dt.Rows[0][5].ToString();
                    labelUserType.Text = dt.Rows[0][3].ToString();
                    linkEditUsers.Visible = false;
                    MessageBox.Show("Welcome!");
                }
            } else {
                MessageBox.Show("Incorrect username/password!");
            }
        }

        private void label39_Click(object sender, EventArgs e) {
            showHide("client", false);
            showHide("menu", true);
        }

        private void clearFields() {
            txtRoomOwner.Text = "";
            txtEmail.Text = "";
            cmbMonth.SelectedIndex = -1;
            cmbDay.SelectedIndex = -1;
            cmbHour.SelectedIndex = -1;
            cmbMinutes.SelectedIndex = -1;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            dbClass db = new dbClass();
            if (txtRoomOwner.Text == "" || txtEmail.Text == "" || cmbMonth.Text == "" || cmbDay.Text == "" || cmbHour.Text == "" || cmbMinutes.Text == "") {
                MessageBox.Show("Fill up all forms!");
            } else {
                string date = cmbMonth.Text + " " + cmbDay.Text + " " + cmbHour.Text + " " + cmbMinutes.Text;
                db.dbUpdate("UPDATE room SET owner = '" + txtRoomOwner.Text + "', state='reserved', reserveddate='" + date + "', email='" + txtEmail.Text + "', phone='" + txtMobile.Text + "' WHERE id=" + txtRoomId.Text);
                showHide("client", false);
                showHide("menu", true);
                setRoomState();
                getRoomsCount();
                sendMail(txtRoomOwner.Text, txtEmail.Text, txtRoomId.Text, cmbMonth.Text + ", " + cmbDay.Text + " Time: " + cmbHour.Text + ":" + cmbMinutes.Text);
                RoomTabTool.Hide();
                RoomTabTool.Show();
                MessageBox.Show("Room Reserved to " + txtRoomOwner.Text);
                clearFields();
            }
        }

        private void fetchClientInfo(int id) {
            dbClass db = new dbClass();
            this.clientInfo = db.dbSelect("SELECT * FROM room WHERE id=" + id);
            var date = this.clientInfo.Rows[0][4].ToString().Split(' ');
            txtRoomOwner.Text = this.clientInfo.Rows[0][2].ToString();
            txtEmail.Text = this.clientInfo.Rows[0][5].ToString();
            txtMobile.Text = this.clientInfo.Rows[0][6].ToString();
            if (date[0] != "") {
                cmbMonth.Text = date[0];
                cmbDay.Text = date[1];
                cmbHour.Text = date[2];
                cmbMinutes.Text = date[3];
            } else {
                clearFields();
            }
        }

        private void room1_Click(object sender, EventArgs e) {
            fetchClientInfo(1);
            groupBoxClient.Text = "Client in room 1";
            txtRoomId.Text = "1";
            roomStateHandler(0, room1);
            showHide("menu", false);
            showHide("client", true);
        }

        private void room2_Click(object sender, EventArgs e) {
            fetchClientInfo(2);
            roomStateHandler(1, room2);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 2";
            txtRoomId.Text = "2";
        }

        private void room3_Click(object sender, EventArgs e) {
            fetchClientInfo(3);
            roomStateHandler(2, room3);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 3";
            txtRoomId.Text = "3";
        }

        private void room4_Click(object sender, EventArgs e) {
            fetchClientInfo(4);
            roomStateHandler(3, room4);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 4";
            txtRoomId.Text = "4";
        }

        private void room5_Click(object sender, EventArgs e) {
            fetchClientInfo(5);
            roomStateHandler(4, room5);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 5";
            txtRoomId.Text = "5";
        }

        private void room6_Click(object sender, EventArgs e) {
            fetchClientInfo(6);
            roomStateHandler(5, room6);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 6";
            txtRoomId.Text = "6";
        }

        private void room7_Click(object sender, EventArgs e) {
            fetchClientInfo(7);
            roomStateHandler(6, room7);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 7";
            txtRoomId.Text = "7";
        }

        private void room8_Click(object sender, EventArgs e) {
            fetchClientInfo(8);
            roomStateHandler(7, room8);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 8";
            txtRoomId.Text = "8";
        }

        private void room9_Click(object sender, EventArgs e) {
            fetchClientInfo(9);
            roomStateHandler(8, room9);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 9";
            txtRoomId.Text = "9";
        }

        private void room10_Click(object sender, EventArgs e) {
            fetchClientInfo(10);
            roomStateHandler(9, room10);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 10";
            txtRoomId.Text = "10";
        }

        private void room11_Click(object sender, EventArgs e) {
            fetchClientInfo(11);
            roomStateHandler(10, room11);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 11";
            txtRoomId.Text = "11";
        }

        private void room12_Click(object sender, EventArgs e) {
            fetchClientInfo(12);
            roomStateHandler(11, room12);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 12";
            txtRoomId.Text = "12";
        }

        private void room13_Click(object sender, EventArgs e) {
            fetchClientInfo(13);
            roomStateHandler(12, room13);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 13";
            txtRoomId.Text = "13";
        }

        private void room14_Click(object sender, EventArgs e) {
            fetchClientInfo(14);
            roomStateHandler(13, room14);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 14";
            txtRoomId.Text = "14";
        }

        private void room15_Click(object sender, EventArgs e) {
            fetchClientInfo(15);
            roomStateHandler(14, room15);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 15";
            txtRoomId.Text = "15";
        }

        private void room16_Click(object sender, EventArgs e) {
            fetchClientInfo(16);
            roomStateHandler(15, room16);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 16";
            txtRoomId.Text = "16";
        }

        private void room17_Click(object sender, EventArgs e) {
            fetchClientInfo(17);
            roomStateHandler(16, room17);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 17";
            txtRoomId.Text = "17";
        }

        private void room18_Click(object sender, EventArgs e) {
            fetchClientInfo(18);
            roomStateHandler(17, room18);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 18";
            txtRoomId.Text = "18";
        }

        private void room19_Click(object sender, EventArgs e) {
            fetchClientInfo(19);
            roomStateHandler(18, room19);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 19";
            txtRoomId.Text = "19";
        }

        private void room20_Click(object sender, EventArgs e) {
            fetchClientInfo(20);
            roomStateHandler(19, room20);
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 20";
            txtRoomId.Text = "20";
        }

        private void btnAvailable_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Are you sure to clear the room?", "Make room available", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK)) {
                dbClass db = new dbClass();
                db.dbUpdate("UPDATE room SET owner = '', state='available', reserveddate='', email='', phone='' WHERE id=" + txtRoomId.Text);
                showHide("menu", true);
                showHide("client", false);
                setRoomState();
                getRoomsCount();
                RoomTabTool.Hide();
                RoomTabTool.Show();
                MessageBox.Show("Room " + txtRoomId.Text + " is now available!");
            } else {

            }
        }

        private void labelLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            this.Width = 300;
            showHide("login", true);
            showHide("room", false);
            showHide("menu", false);
            showHide("client", false);
            txtUsername.Text = "";
            txtPassword.Text = "";
        }

        private void linkEditUsers_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            UserGridView user = new UserGridView();
            user.Show();
        }

        private void sendMail(string fullname, string email, string room, string date) {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("jammmg26@gmail.com", "12261994");

            MailAddress from = new MailAddress("jammmg26@gmail.com", "Marton Suites Room Reservation");
            MailAddress to = new MailAddress(email, fullname);
            MailMessage mm = new MailMessage(from, to);
            mm.Subject = "Marton Suites Room Reservation";
            mm.Body = "Hello " + fullname + " we would like to inform you that you have a pending reservation in Marton Suites.\nTo complete the reservation you need to pay the 70% downpayment policy within this day.\nYou can pay through our bank accounts, ATM or in cash.\n\nRESERVATION INFO:\nReservation Room: " + room + "\nReservation Date: " + date + "\n\nPlease bring the reference of your payment in the time of reservation.\n\n\nRegards,\nMarton Suites Management";
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
        }
    }
}
