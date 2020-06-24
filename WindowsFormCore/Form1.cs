using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormCore
{
    public partial class OpenFileDialogForm : Form
    {
        public OpenFileDialogForm()
        {
            InitializeComponent();
        }

        OpenFileDialog openFileDialog = new OpenFileDialog();

        private void openFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Excel files (*.xlsx;*.xls;*.csv)|*.xlsx;*.xls;*.csv|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            var filePath = string.Empty;

            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                textBox1.Text = openFileDialog.FileName;
                textBox2.Text = openFileDialog.SafeFileName;

                // Get path of selectd file
                filePath = openFileDialog.FileName;

                // Read contents of file into stream
                var fileStream = openFileDialog.OpenFile();


            }
        }
    }
}
