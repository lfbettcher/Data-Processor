using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormCore
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void progressTextBox_TextChanged(object sender, EventArgs e)
        {
            progressTextBox.Text = "processing...";
        }
    }
}
