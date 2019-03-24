using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reservation_System {
    public partial class LoadingScreen : Form {
        public LoadingScreen() {
            InitializeComponent();
            labelFetch.Text = "Loading Major Functions";
        }

        private void LoadingScreen_Load(object sender, EventArgs e) {

        }
    }
}
