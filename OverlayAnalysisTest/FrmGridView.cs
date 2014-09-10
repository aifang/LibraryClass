using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OverlayAnalysisTest
{
    public partial class FrmGridView : Form
    {
        public FrmGridView()
        {
            InitializeComponent();
        }
        public DataTable Table
        {
            get;
            set;
        }

        private void FrmGridView_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Table;
        }
    }
}
