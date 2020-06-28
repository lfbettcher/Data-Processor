using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormCore
{
    class ProcessSkyline
    {
        public static Dictionary<string, Dictionary<string, string>> 
            Run(string filePath)
        {
            var progressWindow = (ProgressWindow)Application.OpenForms["Form2"];
            progressWindow.progressTextBox.SetText("Opening file...");

            var dataMap = new Dictionary<string, Dictionary<string, string>>();

            // Get first worksheet
            var inputFile = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var excelPkg = new ExcelPackage(inputFile);
            var worksheet = excelPkg.Workbook.Worksheets.FirstOrDefault();
            int cols = worksheet.Dimension.Columns;
            int rows = worksheet.Dimension.Rows;

            // Get column index of compound, sample name, and area
            int compoundCol = -1, sampleCol = -1, areaCol = -1;
            for (int c = 1; c <= cols; c++)
            {
                if (string.Equals(worksheet.Cells[1, c].Value.ToString(), "Peptide")) compoundCol = c;
                if (string.Equals(worksheet.Cells[1, c].Value.ToString(), "Replicate")) sampleCol = c;
                if (string.Equals(worksheet.Cells[1, c].Value.ToString(), "Area")) areaCol = c;
            }

            // ERROR: Columns not found
            if (compoundCol == -1 || sampleCol == -1 || areaCol == -1)
            {
                progressWindow.progressTextBox.AppendLine("ERROR: Columns not found");
                return dataMap;
            }

            // Read spreadsheet data into a map
            for (int i = 1; i <= rows; i++)
            {
                var compound = worksheet.Cells[i, compoundCol].Value.ToString();
                var sample = worksheet.Cells[i, sampleCol].Value.ToString();
                var area = worksheet.Cells[i, areaCol].Value.ToString();

                if (dataMap.ContainsKey(compound)) dataMap[compound].Add(sample, area);
                else dataMap.Add(compound, new Dictionary<string, string> {{sample, area}});
            }
            return dataMap;
        }
    }
}