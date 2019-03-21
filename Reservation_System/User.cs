using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reservation_System {
    public partial class User : Form {
        public string userid;
        public bool edit;
        public User() {
            InitializeComponent();
        }

        private void User_Load(object sender, EventArgs e) {
            btnReg.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            if (this.edit == true) {
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
                dbClass db = new dbClass();
                System.Data.DataTable user = db.dbSelect("SELECT * FROM security WHERE id=" + this.userid);
                txtFullName.Text = user.Rows[0][5].ToString();
                txtEmail.Text = user.Rows[0][1].ToString();
                txtPassword.Text = user.Rows[0][2].ToString();
                cmbType.Text = user.Rows[0][3].ToString();
            } else {
                btnReg.Visible = true;
            }
        }

        private void btnReg_Click(object sender, EventArgs e) {
            dbClass db = new dbClass();
            db.dbInsert("INSERT INTO security (email, password, usertype, fullname) VALUES('" + txtEmail.Text + "', '" + txtPassword.Text + "', '" + cmbType.Text + "', '" + txtFullName.Text +  "')");
            this.Hide();
            MessageBox.Show("User Added!");
        }

        private void User_FormClosed(object sender, FormClosedEventArgs e) {
            if (this.edit != true) {
                UserGridView user = new UserGridView();
                user.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            dbClass db = new dbClass();
            db.dbUpdate("UPDATE security SET email = '" + txtEmail.Text + "', password = '" + txtPassword.Text + "', usertype = '" + cmbType.Text + "', fullname = '" + txtFullName.Text + "' WHERE id = " + this.userid);
            this.Hide();
            MessageBox.Show("User Updated!");
        }

        private void button3_Click(object sender, EventArgs e) {
            dbClass db = new dbClass();
            db.dbDelete("DELETE FROM security WHERE id=" + this.userid);
            this.Hide();
            MessageBox.Show("User " + txtFullName.Text + " deleted!");
        }
    }
}
