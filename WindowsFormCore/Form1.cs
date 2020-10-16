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
using System.Windows.Forms.VisualStyles;

namespace WindowsFormCore
{
    public partial class OpenFileForm : Form
    {
        public OpenFileForm()
        {
            InitializeComponent();
        }

        OpenFileDialog openFileDialog = new OpenFileDialog();

        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\input.xlsx"; // for testing
        string fileName = string.Empty;

        List<string> filePaths = new List<string>();
        List<string> fileNames = new List<string>();

        private void openFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            //openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get path(s) of selected file(s)
                foreach (var file in openFileDialog.FileNames)
                {
                    filePath = file;
                    filePaths.Add(filePath);
                    filePathBox.AppendText(filePath);
                }
                // Get name(s) of selected file(s)
                foreach (var file in openFileDialog.SafeFileNames)
                {
                    fileName = file;
                    fileNames.Add(fileName);
                    fileNameBox.AppendText(fileName);
                }
                //filePath = openFileDialog.FileNames;
                //fileName = openFileDialog.SafeFileName;

                //filePathBox.Text = filePath;
                //fileNameBox.Text = fileName;
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (filePath == string.Empty) return;

            var progressWindow = new ProgressWindow();
            progressWindow.Show();

            // Create isotope map
            //Dictionary<string, List<string>> isotopeMap = null;
            //IsotopeCalc isotopeCalc = new IsotopeCalc();
            //isotopeMap = isotopeCalc.IsotopeMap(filePath);

            // Read data to map
            Dictionary<string, Dictionary<string, string>> dataMap = null;
            if (skylineRadioButton.Checked)
            {
                dataMap = ProcessSkyline.ReadDataToMap(filePath);
                if (dataMap.Count == 0) return;
            }
            else if (sciexRadioButton.Checked)
            {
                dataMap = ProcessSciex.ReadDataToMap(filePath);
                if (dataMap.Count == 0) return;
            }
            else if (NormQcRadioButton.Checked)
            {
                dataMap = ProcessSciex.ReadDataToMap(filePath);
                //Normalize.NormalizeToQC(filePath, dataMap);
            }
            else if (multiquantTxtRadioButton.Checked)
            {
                dataMap = ReadInput.ReadMultiQuantTxt(filePaths);
            }
            if (dataMap == null)
            {
                progressWindow.progressTextBox.AppendLine("Error reading file.");
                return;
            }

            progressWindow.progressTextBox.AppendLine("Finished reading data.\r\nWriting data");

            bool removeNA = removeMissingCheckBox.Checked;
            var missingValPercent = string.IsNullOrEmpty(missingValueBox.Text)
                ? missingValueBox.PlaceholderText
                : missingValueBox.Text;

            bool replaceNA = replaceMissingValueCheckBox.Checked;
            var missingValReplace = string.IsNullOrEmpty(replaceMissingValueTextBox.Text)
                ? replaceMissingValueTextBox.PlaceholderText
                : replaceMissingValueTextBox.Text;

            //WriteOutputFile.WriteSciex(replaceNA, missingValReplace, progressWindow, dataMap, filePath);

            //WriteOutputFile.Run(removeNA, replaceNA, missingValPercent, missingValReplace, progressWindow, dataMap, isotopeCalc);
        }
    }

}
