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

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }

    public static class WinFormsExtensions
    {
        public static void SetText(this TextBox source, string text)
        {
            source.Text = text;
        }

        public static void AppendLine(this TextBox source, string text)
        {
            if (source.Text.Length == 0) source.Text = text;
            else source.AppendText("\r\n" + text);
        }
    }
}
