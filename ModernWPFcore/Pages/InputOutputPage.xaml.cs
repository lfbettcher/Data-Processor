using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Windows.Navigation;
using ModernWPFcore.ReadInput;
using Path = System.IO.Path;

namespace ModernWPFcore.Pages
{
    /// <summary>
    /// Interaction logic for InputOutputPage.xaml
    /// </summary>
    public partial class InputOutputPage : Page
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        string applicationPath = Environment.CurrentDirectory;

        private string templateFilePath =
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\DataProcessor";
        string outputFilePath = 
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\DataProcessor";

        // fileNames used for display in list view, filePaths used in program
        List<KeyValuePair<string, string>> fileNamesPaths = new List<KeyValuePair<string, string>>();

        public InputOutputPage()
        {
            InitializeComponent();
        }

        public InputOutputPage(string menuSelection)
        {
            InitializeComponent();
            SetTemplateLocation(menuSelection);
        }

        // Select and Add files to list
        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 3;
            if (InputExcel.IsChecked == true) openFileDialog.FilterIndex = 1;
            else if (InputText.IsChecked == true) openFileDialog.FilterIndex = 2;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    var fileName = Path.GetFileName(filePath);
                    fileNamesPaths.Add(new KeyValuePair<string, string>(fileName, filePath));
                    FileNamesListView.Items.Add(fileName);
                }
            }
        }

        // Removes files from list with button
        private void RemoveFileButton_Click(object sender, RoutedEventArgs e)
        {
            while (FileNamesListView.SelectedIndex >= 0)
            {
                var fileName = FileNamesListView.Items.GetItemAt(FileNamesListView.SelectedIndex);
                FileNamesListView.Items.RemoveAt(FileNamesListView.SelectedIndex);
                fileNamesPaths.RemoveAll(item => item.Key.Equals(fileName));
            }
        }

        // Enables Drag and Drop files into file list box
        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePathsList = (string[]) e.Data.GetData(DataFormats.FileDrop);
                foreach (var filePath in filePathsList)
                {
                    string fileName = Path.GetFileName(filePath);
                    fileNamesPaths.Add(new KeyValuePair<string, string>(fileName, filePath));
                    FileNamesListView.Items.Add(fileName);
                }
            }
        }

        // Default template file
        private void SetTemplateLocation(string menuSelection)
        {
            if (menuSelection == "Sciex6500")
            {
                templateFilePath = applicationPath + "\\Sciex6500_template.xlsx";
            }
            else if (menuSelection == "SciexLipidyzer")
            {
                templateFilePath = applicationPath + "\\SciexLipidyzer_template.xlsx";
            }
            TemplateLocationTextBox.Text = templateFilePath;
        }

        // Select template file
        private void BrowseTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == true)
            {
                templateFilePath = openFileDialog.FileName;
                TemplateLocationTextBox.Text = templateFilePath;
            }
        }

        private void BrowseOutputButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            folderBrowser.FileName = "Select this folder";

            if (folderBrowser.ShowDialog() == true)
            {
                outputFilePath = Path.GetDirectoryName(folderBrowser.FileName);
                OutputLocationTextBox.Text = outputFilePath;
                OutputFileNameTextBox.Text = "Sciex6500_output.xlsx";
            }
        }

        // Summarizes all form options
        private Dictionary<string, string> GetOptions()
        {
            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "SamplesIn", InputRows.IsChecked == true ? "rows" : "columns" },
                { "SamplesOut", OutputRows.IsChecked == true ? "rows" : "columns" },
                { "TemplatePath", templateFilePath },
                { "OutputPath", outputFilePath },
                { "OutputFileName", OutputFileNameTextBox.Text }
            };
            return options;
        }

        private List<string> GetFilePathList(List<KeyValuePair<string, string>> fileNamesPaths)
        {
            return fileNamesPaths.Select(item => item.Value).ToList();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            var progressPage = new ProgressPage();
            ns.Navigate(progressPage);
            var filePathList = GetFilePathList(fileNamesPaths);
            var options = GetOptions();
            var dataMap = ReadMultiQuantTextInput.ReadMultiQuantText(filePathList, options, progressPage);
        }
    }
}
