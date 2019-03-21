using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reservation_System {
    public partial class UserGridView : Form {
        public UserGridView() {
            InitializeComponent();
        }

        private void UserGridView_Load(object sender, EventArgs e) {
            try {
                dbClass db = new dbClass();
                System.Data.DataTable dt = db.dbSelect("SELECT id, fullname, email, password, usertype FROM security");
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;

            } catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e) {
            this.Hide();
            User user = new User();
            user.userid = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            user.edit = true;
            user.Show();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Hide();
            User user = new User();
            user.edit = false;
            user.Show();
        }
    }
}
