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
        private double price = 2000;
        System.Data.DataTable balance;
        int reference;
        string name, owner, resdate, email, phone, days, reff;

        public Main() {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e) {
            this.Width = 300;
            this.ActiveControl = btnLogin;
            getBalance();
            setRoomState();
            getRoomsCount();
            getClientList();
            getReservedLog();
            getCheckoutLog();
            getLoginLog();
            genReference();
            getRoomInd();
            showHide("login", true);
            showHide("room", false);
            showHide("menu", false);
            showHide("client", false);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            }

        private void genReference() {
            Random random = new Random();  
            this.reference = random.Next(10000000, 99999999);
        }

        private void showHide(string panel, bool state) {
            if (panel == "login") {
                panelLogin.Visible = state;
            }
            if (panel == "room") {
                RecentCheckOutTab.Visible = state;
            }
            if (panel == "menu") {
                panelMenu.Visible = state;
            }
            if (panel == "client") {
                panelClient.Visible = state;
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
                labelRef.Text = this.reference.ToString();
                btnCancel.Visible = false;
                btnPaid.Visible = false;
                txtBalance.Visible = false;
                label27.Visible = false;
                room.Image = Properties.Resources.dooropen;
                btnReserve.Enabled = true;
                btnAvailable.Enabled = false;
                btnReserve.Visible = true;
                btnAvailable.Visible = false;
                labelTotal.Visible = true;
                txtTotal.Visible = true;
                txtStatus.Text = "Available";
                clientFieldState(true);
            } 

            if (states.Rows[row][3].ToString() == "reserved") {
                btnAvailable.Visible = false;
                btnCancel.Visible = true;
                btnPaid.Visible = true;
                txtBalance.Text = this.balance.Rows[row][0].ToString();
                txtBalance.Visible = true;
                label27.Visible = true;
                room.Image = Properties.Resources.doorOccupied;
                btnReserve.Enabled = false;
                btnAvailable.Enabled = true;
                btnReserve.Visible = false;
                btnAvailable.Visible = true;
                clientFieldState(false);
                txtStatus.Text = "Reserved";
            }

            if (states.Rows[row][3].ToString() == "occupied")  {
                btnCancel.Visible = false;
                btnPaid.Visible = false;
                txtBalance.Text = this.balance.Rows[row][0].ToString();
                txtBalance.Visible = true;
                room.Image = Properties.Resources.doorPending;
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
            getRoomInd();
            txtTotal.Text = (noOfDays.Value * 2000).ToString() + ".00";
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
            System.Data.DataTable res = db.dbSelect("SELECT * FROM room WHERE state='occupied'");
            System.Data.DataTable avail = db.dbSelect("SELECT * FROM room WHERE state='available'");
            System.Data.DataTable pen = db.dbSelect("SELECT * FROM room WHERE state='reserved'");
            System.Data.DataTable t_res = db.dbSelect("SELECT * FROM room_ind");
            labelReservedRooms.Text = res.Rows.Count.ToString(); 
            labelAvailableRooms.Text = avail.Rows.Count.ToString();
            labelPending.Text = pen.Rows.Count.ToString();
            labelt_res.Text = t_res.Rows.Count.ToString();
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
                System.Data.DataTable dt = db.dbSelect("SELECT name, reserveddate as 'reserved date', owner, email, phone, days, balance, reference FROM reservelog");
                dataGridView2.DataSource = dt;
                dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView2.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView2.Columns[0].ReadOnly = true;
                dataGridView2.Columns[1].ReadOnly = true;
                dataGridView2.Columns[2].ReadOnly = true;
                dataGridView2.Columns[3].ReadOnly = true;
                dataGridView2.Columns[4].ReadOnly = true;
                dataGridView2.Columns[5].ReadOnly = true;
                dataGridView2.Columns[6].ReadOnly = true;
                dataGridView2.Columns[7].ReadOnly = true;

            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void getBalance() {
            dbClass db = new dbClass();
            System.Data.DataTable dt = db.dbSelect("SELECT balance from reservelog");
            this.balance = dt;
        }

        private void getCheckoutLog() {
            try {
                dbClass db = new dbClass();
                System.Data.DataTable dt = db.dbSelect("SELECT name, checkoutdate as 'checkout date', owner, email, phone, days, balance, reference FROM reservelog WHERE state='checkout'");
                dataGridView3.DataSource = dt;
                dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView3.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //dataGridView3.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView3.Columns[0].ReadOnly = true;
                dataGridView3.Columns[1].ReadOnly = true;
                dataGridView3.Columns[2].ReadOnly = true;
                dataGridView3.Columns[3].ReadOnly = true;
                dataGridView3.Columns[4].ReadOnly = true;
                dataGridView3.Columns[5].ReadOnly = true;
                dataGridView3.Columns[6].ReadOnly = true;
                dataGridView3.Columns[7].ReadOnly = true;
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
                } else {
                    db.dbInsert("INSERT INTO loginlog (email, date, usertype) VALUES('" + txtUsername.Text + "', '" + getDateTime() + "', 'staff')");
                    showHide("login", false);
                    showHide("room", true);
                    showHide("menu", true);
                    labelUser.Text = dt.Rows[0][5].ToString();
                    labelUserType.Text = dt.Rows[0][3].ToString();
                    linkEditUsers.Visible = false;
                }
                getLoginLog();
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
            if (txtEmail.Text.Contains('@')) {
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
                    decimal percent = (decimal)0.70;
                    double formula = Convert.ToDouble((noOfDays.Value * 2000) - (((noOfDays.Value * 2000) * percent)));
                    db.dbInsert("INSERT INTO room_ind (name, owner, reserveddate, email, phone, days, reference) VALUES('Room " + txtRoomId.Text + "', '" + txtRoomOwner.Text + "', '" + getDateTime() + "', '" + txtEmail.Text + "', '" + txtMobile.Text + "', '" + noOfDays.Value + "', '" + this.reference + "')");
                    db.dbInsert("INSERT INTO reservelog (name, owner, reserveddate, email, phone, days, balance, reference) VALUES('Room " + txtRoomId.Text + "', '" + txtRoomOwner.Text + "', '" + getDateTime() + "', '" + txtEmail.Text + "', '" + txtMobile.Text + "', '" + noOfDays.Value + "', '" + formula + "', '" + this.reference + "')");
                    showHide("client", false);
                    showHide("menu", true);
                    setRoomState();
                    getRoomsCount();
                    getCheckoutLog();
                    sendMail(txtRoomOwner.Text, txtEmail.Text, txtRoomId.Text, dateTimeFrom.Text, noOfDays.Value);
                    RecentCheckOutTab.Hide();
                    RecentCheckOutTab.Show();
                    MessageBox.Show("Room Reserved to " + txtRoomOwner.Text);
                    clearFields();
                    genReference();
                }
            } else {
                MessageBox.Show("Enter valid email address!");
            }
        }

        private void fetchClientInfo(int id) {
            dbClass db = new dbClass();
            this.clientInfo = db.dbSelect("SELECT * FROM room WHERE id=" + id);
            dateTimeFrom.Text = this.clientInfo.Rows[0][4].ToString();
            txtRoomOwner.Text = this.clientInfo.Rows[0][2].ToString();
            txtEmail.Text = this.clientInfo.Rows[0][5].ToString();
            txtMobile.Text = this.clientInfo.Rows[0][6].ToString();
            labelRef.Text = this.clientInfo.Rows[0]["reference"].ToString();
            try {
                noOfDays.Value = Convert.ToInt32(this.clientInfo.Rows[0][7]);
            } catch (InvalidCastException) {
                noOfDays.Value = 1;
            }
        }

        private void room1_Click(object sender, EventArgs e) {
            fetchClientInfo(1);
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
            txtRoomId.Text = "2";
        }

        private void room3_Click(object sender, EventArgs e) {
            fetchClientInfo(3);
            roomStateHandler(2, room3);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "3";
        }

        private void room4_Click(object sender, EventArgs e) {
            fetchClientInfo(4);
            roomStateHandler(3, room4);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "4";
        }

        private void room5_Click(object sender, EventArgs e) {
            fetchClientInfo(5);
            roomStateHandler(4, room5);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "5";
        }

        private void room6_Click(object sender, EventArgs e) {
            fetchClientInfo(6);
            roomStateHandler(5, room6);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "6";
        }

        private void room7_Click(object sender, EventArgs e) {
            fetchClientInfo(7);
            roomStateHandler(6, room7);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "7";
        }

        private void room8_Click(object sender, EventArgs e) {
            fetchClientInfo(8);
            roomStateHandler(7, room8);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "8";
        }

        private void room9_Click(object sender, EventArgs e) {
            fetchClientInfo(9);
            roomStateHandler(8, room9);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "9";
        }

        private void room10_Click(object sender, EventArgs e) {
            fetchClientInfo(10);
            roomStateHandler(9, room10);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "10";
        }

        private void room11_Click(object sender, EventArgs e) {
            fetchClientInfo(11);
            roomStateHandler(10, room11);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "11";
        }

        private void room12_Click(object sender, EventArgs e) {
            fetchClientInfo(12);
            roomStateHandler(11, room12);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "12";
        }

        private void room13_Click(object sender, EventArgs e) {
            fetchClientInfo(13);
            roomStateHandler(12, room13);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "13";
        }

        private void room14_Click(object sender, EventArgs e) {
            fetchClientInfo(14);
            roomStateHandler(13, room14);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "14";
        }

        private void room15_Click(object sender, EventArgs e) {
            fetchClientInfo(15);
            roomStateHandler(14, room15);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "15";
        }

        private void room16_Click(object sender, EventArgs e) {
            fetchClientInfo(16);
            roomStateHandler(15, room16);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "16";
        }

        private void room17_Click(object sender, EventArgs e) {
            fetchClientInfo(17);
            roomStateHandler(16, room17);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "17";
        }

        private void room18_Click(object sender, EventArgs e) {
            fetchClientInfo(18);
            roomStateHandler(17, room18);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "18";
        }

        private void room19_Click(object sender, EventArgs e) {
            fetchClientInfo(19);
            roomStateHandler(18, room19);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "19";
        }

        private void room20_Click(object sender, EventArgs e) {
            fetchClientInfo(20);
            roomStateHandler(19, room20);
            showHide("menu", false);
            showHide("client", true);
            txtRoomId.Text = "20";
        }

        private void btnAvailable_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Checkout", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK)) {
                dbClass db = new dbClass();
                db.dbUpdate("UPDATE room SET owner = '', state='available', reserveddate='', email='', phone='', days=1, reference='' WHERE id=" + txtRoomId.Text);
                db.dbUpdate("UPDATE reservelog SET state='checkout', checkoutdate='" + getDateTime() + "' WHERE name='Room " + txtRoomId.Text + "'");
                showHide("menu", true);
                showHide("client", false);
                setRoomState();
                getRoomsCount();
                getCheckoutLog();
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
            mm.Body = "Hello " + fullname + " we would like to inform you that you have a pending reservation in Marton Suites.\nTo complete the reservation you need to pay PHP " + total + " OR 70% for downpayment policy within this day.\nYou can pay through our bank accounts, ATM or in cash.\n\nRESERVATION INFO:\nReservation Room: " + room + "\nReservation Date: " + date + "\nNumber of days: " + days + "\nReference: " + this.reference + "\n\nPlease bring the reference of your payment in the time of reservation.\n\n\nRegards,\nMarton Suites Management";
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
            this.price = 2000;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e) {
            txtRoomOwner.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtMobile.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private void btnTest_Click(object sender, EventArgs e) {
            MessageBox.Show(dateTimeFrom.Text);
        }

        private void noOfDays_ValueChanged(object sender, EventArgs e) {
            txtTotal.Text = (noOfDays.Value * 2000).ToString() + ".00" ;
        }

        private void txtUsername_Enter(object sender, EventArgs e) {
            if (txtUsername.Text == "name@company.com") {
                txtUsername.Text = "";
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e) {
            if (txtPassword.Text == "password") {
                txtPassword.Text = "";
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e) {
            if (txtUsername.Text == "") {
                txtUsername.Text = "name@company.com";
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e) {
            if (txtPassword.Text == "") {
                txtPassword.Text = "password";
            }
        }

        private void btnPaid_Click(object sender, EventArgs e) {
            dbClass db = new dbClass();
            db.dbUpdate("UPDATE room SET state='occupied' WHERE id='" + txtRoomId.Text + "'");
            showHide("client", false);
            showHide("menu", true);
            setRoomState();
            getCheckoutLog();
            MessageBox.Show("Success!");
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Cancel", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK)) {
                dbClass db = new dbClass();
                db.dbUpdate("UPDATE room SET owner = '', state='available', reserveddate='', email='', phone='', days=1 WHERE id=" + txtRoomId.Text);
                showHide("menu", true);
                showHide("client", false);
                setRoomState();
                getRoomsCount();
                getCheckoutLog();
                RecentCheckOutTab.Hide();
                RecentCheckOutTab.Show();
                MessageBox.Show("Room " + txtRoomId.Text + " is now available!");
            } else {

            }
        }

        private void getRoomInd() {
            dbClass db = new dbClass();
            System.Data.DataTable dt = db.dbSelect("SELECT reference,name,owner,reserveddate as 'date',email,phone,days FROM room_ind");
            dataGridView5.DataSource = dt;
            dataGridView5.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView5.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView5.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView5.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView5.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView5.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView5.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            try {
                this.reff = dataGridView5.CurrentRow.Cells[0].Value.ToString();
                this.name = dataGridView5.CurrentRow.Cells[1].Value.ToString();
                this.owner = dataGridView5.CurrentRow.Cells[2].Value.ToString();
                this.resdate = dataGridView5.CurrentRow.Cells[3].Value.ToString();
                this.email = dataGridView5.CurrentRow.Cells[4].Value.ToString();
                this.phone = dataGridView5.CurrentRow.Cells[5].Value.ToString();
                this.days = dataGridView5.CurrentRow.Cells[6].Value.ToString();
            } catch (Exception) { }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            dbClass db = new dbClass();
            DialogResult result = MessageBox.Show("Check in?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK)) {
                db.dbUpdate("UPDATE room SET owner = '" + this.owner + "', state='reserved', reserveddate='" + this.resdate + "', email='" + this.email + "', phone='" + this.phone + "', days='" + this.days + "', reference='" + this.reff + "' WHERE name='" + this.name + "'");
                db.dbUpdate("DELETE FROM room_ind WHERE reference='" + this.reff + "'");
                setRoomState();
                getRoomsCount();
                getCheckoutLog();
                showHide("client", false);
                showHide("menu", true);
                MessageBox.Show(this.owner + " has been checked in!");
            } else { }
        }
    }
}
