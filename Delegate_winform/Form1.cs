using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Delegate_winform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.OnReturnValueToFather += new Component.button.ReturnValueTofather(returnValue);
        }

        void returnValue()
        {
            textBox1.Text += "调用1次";
        }

        private void button1_Load(object sender, EventArgs e)
        {
            //button1.OnReturnValueToFather += new Component.button.ReturnValueTofather(returnValue);
        }

        

        
    }
}
