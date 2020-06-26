using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormCore
{
    public partial class OpenFileForm : Form
    {
        public OpenFileForm()
        {
            InitializeComponent();
        }

        OpenFileDialog openFileDialog = new OpenFileDialog();
        string filePath = "C:\\Users\\Lisa\\OneDrive\\Desktop\\input.xlsx";
        string fileName = string.Empty;

        private void openFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get path of selectd file
                filePath = openFileDialog.FileName;
                fileName = openFileDialog.SafeFileName;

                filePathBox.Text = filePath;
                fileNameBox.Text = fileName;
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (filePath != string.Empty)
            {
                Form2 form2 = new Form2();
                form2.Show();

                if (skylineRadioButton.Checked)
                {
                    ProcessSkyline.Run(filePath, 1, 3, 10);
                }
                else if (radioButton1.Checked)
                {
                    // placeholder
                }
                else if (radioButton2.Checked)
                {
                    // placeholder
                }
            }
        }

    }
}
