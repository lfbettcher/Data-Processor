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
using Path = System.IO.Path;

namespace ModernWPFcore.Pages
{
    /// <summary>
    /// Interaction logic for InputOutputPage.xaml
    /// </summary>
    public partial class InputOutputPage : Page
    {
        
        string applicationPath = Environment.CurrentDirectory;
        private string menuSelection = "Home";

        string templateFilePath =
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
            this.menuSelection = menuSelection;
            SetDefaultOptions();
            SetDefaultPaths();
        }

        // Set form options to default for workflow
        private void SetDefaultOptions()
        {
            switch (menuSelection)
            {
                case "Sciex6500":
                    InputText.IsChecked = true;
                    InputColumns.IsChecked = true;
                    OutputColumns.IsChecked = true;
                    break;
                case "SciexLipidyzer":
                    InputExcel.IsChecked = true;
                    InputRows.IsChecked = true;
                    OutputRows.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        // Set default template and output files
        private void SetDefaultPaths()
        {
            templateFilePath = $"{templateFilePath}\\{menuSelection}_template.xlsx";
            outputFilePath = $"{outputFilePath}\\{menuSelection}_output.xlsx";
            TemplateLocationTextBox.Text = Path.GetDirectoryName(templateFilePath) ?? string.Empty;
            TemplateFileNameTextBox.Text = Path.GetFileName(templateFilePath) ?? string.Empty;
            OutputLocationTextBox.Text = Path.GetDirectoryName(outputFilePath) ?? string.Empty;
            OutputFileNameTextBox.Text = Path.GetFileName(outputFilePath) ?? string.Empty;
        }

        // Add files to list with Add button
        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                RestoreDirectory = true,
                Filter = "Excel files (*.xlsx)|*.xlsx|Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 3
            };
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

        // Enables Drag and Drop files into file list box
        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePathsList = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var filePath in filePathsList)
                {
                    string fileName = Path.GetFileName(filePath);
                    fileNamesPaths.Add(new KeyValuePair<string, string>(fileName, filePath));
                    FileNamesListView.Items.Add(fileName);
                }
            }
        }

        // Removes files from list with Remove button
        private void RemoveFileButton_Click(object sender, RoutedEventArgs e)
        {
            while (FileNamesListView.SelectedIndex >= 0)
            {
                var fileName = FileNamesListView.Items.GetItemAt(FileNamesListView.SelectedIndex);
                FileNamesListView.Items.RemoveAt(FileNamesListView.SelectedIndex);
                fileNamesPaths.RemoveAll(item => item.Key.Equals(fileName));
            }
        }

        // Select template file
        private void BrowseTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                RestoreDirectory = true,
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FilterIndex = 1,
                CheckPathExists = true,
                CheckFileExists = true,
                FileName = menuSelection + "_template.xlsx"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                templateFilePath = openFileDialog.FileName;
                TemplateLocationTextBox.Text = Path.GetDirectoryName(templateFilePath) ?? string.Empty;
                TemplateFileNameTextBox.Text = Path.GetFileName(templateFilePath) ?? string.Empty;
            }
        }

        // Select output file location
        private void BrowseOutputButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FilterIndex = 1,
                CheckPathExists = true,
                FileName = menuSelection + "_output.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                outputFilePath = saveFileDialog.FileName;
                OutputLocationTextBox.Text = Path.GetDirectoryName(saveFileDialog.FileName) ?? string.Empty;
                OutputFileNameTextBox.Text = Path.GetFileName(saveFileDialog.FileName);
            }
        }

        // Summarizes all form options
        private Dictionary<string, string> GetOptions()
        {
            var options = new Dictionary<string, string>
            {
                { "InputType", InputExcel.IsChecked == true ? "excel" : "text"},
                { "SamplesIn", InputRows.IsChecked == true ? "rows" : "columns" },
                { "SamplesOut", OutputRows.IsChecked == true ? "rows" : "columns" },
                { "TemplatePath", templateFilePath },
                { "OutputFolder", Path.GetDirectoryName(outputFilePath) },
                { "OutputFileName", OutputFileNameTextBox.Text },
                { "TemplateTabName", TemplateTabName.Text },
                { "SampleLoc", SampleLoc.Text },
                { "CompoundLoc", CompoundLoc.Text },
                { "StartInCell", StartInCell.Text },
                { "AbsoluteQuantTabName", AbsoluteQuantTabName.Text }
            };
            return options;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var navigationService = NavigationService.GetNavigationService(this);
            var progressPage = new ProgressPage();
            navigationService?.Navigate(progressPage);
            var filePathList = fileNamesPaths.Select(file => file.Value).ToList();
            var options = GetOptions();
            //ProcessHandler.Run(menuSelection, filePathList, options, progressPage);
            ProcessHandler.Run(menuSelection, filePathList, options, progressPage, this);
        }
    }
}
