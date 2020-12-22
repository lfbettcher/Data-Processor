using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using Path = System.IO.Path;

namespace ModernWPFcore.Pages
{
    /// <summary>
    /// Interaction logic for InputOutputPage.xaml
    /// </summary>
    public partial class InputOutputPage : Page
    {
        public InputOutputPage()
        {
            InitializeComponent();

        }

        public InputOutputPage(string menuItem)
        {
            InitializeComponent();
            SetTemplateLocation(menuItem);
        }

        private void SetTemplateLocation(string menuItem)
        {
            TemplateLocationTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                                           + "\\sciex6500template.xlsx";
        }

        OpenFileDialog openFileDialog = new OpenFileDialog();

        // fileNames used for display in list view, filePaths used in program
        List<KeyValuePair<string, string>> fileNamesPaths = new List<KeyValuePair<string, string>>();

        // Select and Add files to list
        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
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

        // Select template file
        private void BrowseTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == true)
            {
                var templatePath = openFileDialog.FileName;
                TemplateLocationTextBox.Text = templatePath;
            }
        }
    }
}

