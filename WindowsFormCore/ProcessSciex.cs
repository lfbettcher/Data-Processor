using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormCore
{
    class ProcessSciex
    {
        public static Dictionary<string, Dictionary<string, string>> 
            ReadDataToMap(string filePath)
        {
            var progressWindow = (ProgressWindow)Application.OpenForms["Form2"];
            progressWindow.progressTextBox.AppendLine("Opening data file");

            var dataMap = new Dictionary<string, Dictionary<string, string>>();

            // Get first worksheet
            var inputFile = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var excelPkg = new ExcelPackage(inputFile);
            var worksheet = excelPkg.Workbook.Worksheets.FirstOrDefault();
            int cols = worksheet.Dimension.Columns;
            int rows = worksheet.Dimension.Rows;

            // Read spreadsheet data into a map
            for (int r = 2; r <= rows; r++)
            {
                // Compound in first column
                var compound = worksheet.Cells[r, 1].Value.ToString();
                var samplesMap = new Dictionary<string, string>();

                for (int c = 2; c <= cols; c++)
                {
                    // Sample name in first row
                    var sample = worksheet.Cells[1, c].Value.ToString();
                    var area = worksheet.Cells[r, c].Value.ToString();
                    samplesMap.Add(sample, area);
                }
                dataMap.Add(compound, samplesMap);
            }
            return dataMap;
        }
    }
}