﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormCore
{
    public partial class ProgressWindow : Form
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        public void progressTextBox_TextChanged(object sender, EventArgs e)
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
