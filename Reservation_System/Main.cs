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

namespace Reservation_System {
    public partial class Main : Form {
        SqlConnection conn = dbClass.getConnection();
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

        private void Main_Load(object sender, EventArgs e) {
            showHide("login", true);
            showHide("room", false);
            showHide("menu", false);
            showHide("client", false);
            Room room = new Room();
            System.Data.DataTable states = room.getRoomState();
            MessageBox.Show(states.Rows[0][3].ToString());
            for (int x = 0; x < 20; x++) {
                if (states.Rows[x][3].ToString() == "available") {
                    room1.Image = Properties.Resources.doorAvailable;
                }
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

        private void btnLogin_Click(object sender, EventArgs e) {
            System.Data.DataTable dt = loginQuery(txtUsername.Text, txtPassword.Text);
            if (dt.Rows.Count > 0) {
                if (dt.Rows[0][3].ToString() == "admin") {
                    showHide("login", false);
                    showHide("room", true);
                    showHide("menu", true);
                    MessageBox.Show("Welcome admin!");
                } else {
                    MessageBox.Show("Welcome!");
                }
            } else {
                MessageBox.Show("Incorrect username/password!");
            }
        }

        private void room1_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 1";
            txtRoomId.Text = "1";
        }

        private void room2_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 2";
            txtRoomId.Text = "2";
        }

        private void label39_Click(object sender, EventArgs e) {
            showHide("client", false);
            showHide("menu", true);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Room room = new Room();
        }

        private void room3_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 3";
            txtRoomId.Text = "3";
        }

        private void room4_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 4";
            txtRoomId.Text = "4";
        }

        private void room5_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 5";
            txtRoomId.Text = "5";
        }

        private void room6_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 6";
            txtRoomId.Text = "6";
        }

        private void room7_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 7";
            txtRoomId.Text = "7";
        }

        private void room8_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 8";
            txtRoomId.Text = "8";
        }

        private void room9_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 9";
            txtRoomId.Text = "9";
        }

        private void room10_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 10";
            txtRoomId.Text = "10";
        }

        private void room11_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 11";
            txtRoomId.Text = "11";
        }

        private void room12_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 12";
            txtRoomId.Text = "12";
        }

        private void room13_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 13";
            txtRoomId.Text = "13";
        }

        private void room14_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 14";
            txtRoomId.Text = "14";
        }

        private void room15_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 15";
            txtRoomId.Text = "15";
        }

        private void room16_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 16";
            txtRoomId.Text = "16";
        }

        private void room17_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 17";
            txtRoomId.Text = "17";
        }

        private void room18_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 18";
            txtRoomId.Text = "18";
        }

        private void room19_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 19";
            txtRoomId.Text = "19";
        }

        private void room20_Click(object sender, EventArgs e) {
            showHide("menu", false);
            showHide("client", true);
            groupBoxClient.Text = "Client in room 20";
            txtRoomId.Text = "20";
        }
    }
}
