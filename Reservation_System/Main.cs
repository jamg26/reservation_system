﻿using System;
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
using System.Threading;

namespace Reservation_System {
    public partial class Main : Form {
        SqlConnection conn = dbClass.getConnection();
        private System.Data.DataTable clientInfo;
        private double price = 2000;
        

        public Main() {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e) {
            Thread a = new Thread(() => SplashScreen());
            a.Start();
            this.Width = 300;
            showHide("login", true);
            showHide("room", false);
            showHide("menu", false);
            showHide("client", false);
            setRoomState();
            getRoomsCount();
            getClientList();
            getReservedLog();
            getCheckoutLog();
            a.Abort();
            getLoginLog();
        }

        public void SplashScreen() {
            Application.Run(new LoadingScreen());
        }

        private void showHide(string panel, bool state) {
            if (panel == "login") {
                groupBoxLogin.Visible = state;
            }
            if (panel == "room") {
                RecentCheckOutTab.Visible = state;
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
            dateTimeFrom.Enabled = state;
            noOfDays.Enabled = state;
        }

        private void roomStateHandler(int row, System.Windows.Forms.PictureBox room) {
            Room roomm = new Room();
            System.Data.DataTable states = roomm.getRoomState();
            if (states.Rows[row][3].ToString() == "available") {
                room.Image = Properties.Resources.dooropen;
                btnReserve.Enabled = true;
                btnAvailable.Enabled = false;
                btnReserve.Visible = true;
                btnAvailable.Visible = false;
                txtStatus.Text = "Available";
                clientFieldState(true);
            } 
            if (states.Rows[row][3].ToString() == "reserved") {
                room.Image = Properties.Resources.doorPending;
                btnReserve.Enabled = false;
                btnAvailable.Enabled = true;
                btnReserve.Visible = false;
                btnAvailable.Visible = true;
                clientFieldState(false);
                txtStatus.Text = "Reserved";
            }
            if (states.Rows[row][3].ToString() == "occupied")  {
                room.Image = Properties.Resources.doorOccupied;
                btnReserve.Enabled = false;
                btnAvailable.Enabled = true;
                btnReserve.Visible = false;
                btnAvailable.Visible = true;
                clientFieldState(false);
                txtStatus.Text = "Occupied";
            }
            getClientList();
            getReservedLog();
            getCheckoutLog();
            getLoginLog();
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

        private void getClientList() {
            try {
                dbClass db = new dbClass();
                System.Data.DataTable dt = db.dbSelect("SELECT name, email, phone FROM client");
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;

            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void getReservedLog() {
            try {
                dbClass db = new dbClass();
                System.Data.DataTable dt = db.dbSelect("SELECT name, reserveddate, owner, email, phone, days FROM reservelog");
                dataGridView2.DataSource = dt;
                dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView2.Columns[0].ReadOnly = true;
                dataGridView2.Columns[1].ReadOnly = true;
                dataGridView2.Columns[2].ReadOnly = true;
                dataGridView2.Columns[3].ReadOnly = true;
                dataGridView2.Columns[4].ReadOnly = true;
                dataGridView2.Columns[5].ReadOnly = true;

            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void getCheckoutLog() {
            try {
                dbClass db = new dbClass();
                System.Data.DataTable dt = db.dbSelect("SELECT name, reserveddate, owner, email, phone, days FROM reservelog WHERE state='checkout'");
                dataGridView3.DataSource = dt;
                dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView3.Columns[0].ReadOnly = true;
                dataGridView3.Columns[1].ReadOnly = true;
                dataGridView3.Columns[2].ReadOnly = true;
                dataGridView3.Columns[3].ReadOnly = true;
                dataGridView3.Columns[4].ReadOnly = true;
                dataGridView3.Columns[5].ReadOnly = true;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void getLoginLog() {
            try {
                dbClass db = new dbClass();
                System.Data.DataTable dt = db.dbSelect("SELECT email, date, usertype FROM loginlog");
                dataGridView4.DataSource = dt;
                dataGridView4.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView4.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView4.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView4.Columns[0].ReadOnly = true;
                dataGridView4.Columns[1].ReadOnly = true;
                dataGridView4.Columns[2].ReadOnly = true;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
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

        private string getDateTime() {
            DateTime aDate = DateTime.Now;
            return aDate.ToString("dddd, dd MMMM yyyy hh:mm tt");
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            System.Data.DataTable dt = loginQuery(txtUsername.Text, txtPassword.Text);
            dbClass db = new dbClass();
            if (dt.Rows.Count > 0) {
                this.Width = 934;
                if (dt.Rows[0][3].ToString() == "admin") {
                    db.dbInsert("INSERT INTO loginlog (email, date, usertype) VALUES('" + txtUsername.Text + "', '" + getDateTime() + "', 'admin')");
                    showHide("login", false);
                    showHide("room", true);
                    showHide("menu", true);
                    labelUser.Text = dt.Rows[0][5].ToString();
                    labelUserType.Text = dt.Rows[0][3].ToString();
                    linkEditUsers.Visible = true;
                    MessageBox.Show("Welcome admin!");
                } else {
                    db.dbInsert("INSERT INTO loginlog (email, date, usertype) VALUES('" + txtUsername.Text + "', '" + getDateTime() + "', 'staff')");
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
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            dbClass db = new dbClass();
            if (txtRoomOwner.Text == "" || txtEmail.Text == "") {
                MessageBox.Show("Fill up all forms!");
            } else {
                System.Data.DataTable owner = db.dbSelect("SELECT * FROM client WHERE name='" + txtRoomOwner.Text + "'");
                if (owner.Rows.Count == 0) {
                    db.dbInsert("INSERT INTO client (name, email, phone) VALUES('" + txtRoomOwner.Text + "', '" + txtEmail.Text + "', '" + txtMobile.Text + "')");
                } else {
                    db.dbUpdate("UPDATE client SET name = '" + txtRoomOwner.Text + "', email='" + txtEmail.Text + "', phone='" + txtMobile.Text + "' WHERE name='" + txtRoomOwner.Text + "'");
                }
                db.dbUpdate("UPDATE room SET owner = '" + txtRoomOwner.Text + "', state='reserved', reserveddate='" + dateTimeFrom.Text + "', email='" + txtEmail.Text + "', phone='" + txtMobile.Text + "', days='" + noOfDays.Value + "' WHERE id=" + txtRoomId.Text);
                db.dbInsert("INSERT INTO reservelog (name, owner, reserveddate, email, phone, days) VALUES('Room " + txtRoomId.Text + "', '" + txtRoomOwner.Text + "', '" + dateTimeFrom.Text + "', '" + txtEmail.Text + "', '" + txtMobile.Text + "', '" + noOfDays.Value + "')");
                showHide("client", false);
                showHide("menu", true);
                setRoomState();
                getRoomsCount();
                sendMail(txtRoomOwner.Text, txtEmail.Text, txtRoomId.Text, dateTimeFrom.Text, noOfDays.Value);
                RecentCheckOutTab.Hide();
                RecentCheckOutTab.Show();
                MessageBox.Show("Room Reserved to " + txtRoomOwner.Text);
                clearFields();
            }
        }

        private void fetchClientInfo(int id) {
            dbClass db = new dbClass();
            this.clientInfo = db.dbSelect("SELECT * FROM room WHERE id=" + id);
            dateTimeFrom.Text = this.clientInfo.Rows[0][4].ToString();
            txtRoomOwner.Text = this.clientInfo.Rows[0][2].ToString();
            txtEmail.Text = this.clientInfo.Rows[0][5].ToString();
            txtMobile.Text = this.clientInfo.Rows[0][6].ToString();
            try {
                noOfDays.Value = Convert.ToInt32(this.clientInfo.Rows[0][7]);
            } catch (InvalidCastException) {
                noOfDays.Value = 1;
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
            DialogResult result = MessageBox.Show("Checkout", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK)) {
                dbClass db = new dbClass();
                db.dbUpdate("UPDATE room SET owner = '', state='available', reserveddate='', email='', phone='', days=1 WHERE id=" + txtRoomId.Text);
                db.dbUpdate("UPDATE reservelog SET state='checkout' WHERE name='Room " + txtRoomId.Text + "'");
                showHide("menu", true);
                showHide("client", false);
                setRoomState();
                getRoomsCount();
                RecentCheckOutTab.Hide();
                RecentCheckOutTab.Show();
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

        private void sendMail(string fullname, string email, string room, string date, decimal days) {
            this.price = this.price * Convert.ToDouble(days);
            double total = this.price * 0.70;
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
            mm.Body = "Hello " + fullname + " we would like to inform you that you have a pending reservation in Marton Suites.\nTo complete the reservation you need to pay PHP " + total + " OR 70% for downpayment policy within this day.\nYou can pay through our bank accounts, ATM or in cash.\n\nRESERVATION INFO:\nReservation Room: " + room + "\nReservation Date: " + date + "\nNumber of days: " + days + "\n\nPlease bring the reference of your payment in the time of reservation.\n\n\nRegards,\nMarton Suites Management";
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e) {
            txtRoomOwner.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtMobile.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private void btnTest_Click(object sender, EventArgs e) {
            MessageBox.Show(dateTimeFrom.Text);
        }
    }
}
