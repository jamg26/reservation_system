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
            room.setRoomOwner(txtRoomId.Text, txtRoomOwner.Text);
        }
    }
}
