using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormCore.Forms
{
    public partial class FormInputFile : Form
    {
        public FormInputFile()
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
                    textBoxFileName.AppendText(fileName);
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
            Dictionary<string, List<string>> isotopeMap = null;
            IsotopeCalc isotopeCalc = new IsotopeCalc();
            //isotopeMap = isotopeCalc.IsotopeMap(filePath);

            // Read data to map
            Dictionary<string, Dictionary<string, string>> dataMap = null;

            //if (skylineRadioButton.Checked)
            //{
            //    dataMap = ProcessSkyline.ReadDataToMap(filePath);
            //    if (dataMap.Count == 0) return;

            //    // Create isotope map
            //    isotopeMap = isotopeCalc.IsotopeMap(filePath);
            //}
            if (radioButtonExcel.Checked)
            {
                dataMap = ProcessSciex.ReadDataToMap(filePath);
                if (dataMap.Count == 0) return;
            }
            else if (radioButtonMultiQuant.Checked)
            {
                dataMap = ReadInput.ReadMultiQuantTxt(filePaths);
                var templatePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) +
                                   $"\\DataProcessor\\targeted_300_template-serum-mrm.xlsx";
                WriteOutputFile.WriteMapToSheetCompoundsInRows(progressWindow, dataMap, templatePath, "Relative Quant Data", 1);
                //WriteOutputFile.WriteMapToSheetCompoundsInRows(progressWindow, dataMap, templatePath, "Data Reproducibility", 2);
            }
            if (dataMap == null)
            {
                progressWindow.progressTextBox.AppendLine("Error reading file.");
                return;
            }

            progressWindow.progressTextBox.AppendLine("Finished reading data.\r\nWriting data");

            //WriteOutputFile.WriteSciex(replaceNA, missingValReplace, progressWindow, dataMap, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\DataProcessor\\targeted_300_template-tissue.xlsx");

        }

        private void FormInput_Load(object sender, EventArgs e)
        {
            LoadTheme();
        }

        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button) btns;
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
            }
        }

        private void panelInputFileBox_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panelInputFileBox.ClientRectangle, Color.Teal, ButtonBorderStyle.Solid);
        }
    }
}
