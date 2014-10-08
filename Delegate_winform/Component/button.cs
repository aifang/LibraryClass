using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Delegate_winform.Component
{
    public partial class button : UserControl
    {

        public delegate void ReturnValueTofather();
        public event ReturnValueTofather OnReturnValueToFather;

        public void returnValue()
        {
            OnReturnValueToFather();
        }

        public button()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //returnValue();
            OnReturnValueToFather();
        }
    }
}
