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
        public static void Run(string filePath, int compoundCol, int sampleCol, int areaCol)
        {
            var progressWindow = (ProgressWindow)Application.OpenForms["Form2"];
            progressWindow.progressTextBox.SetText("Opening file...");

            // Get first worksheet
            var inputFile = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var excelPkg = new ExcelPackage(inputFile);
            ExcelWorksheet worksheet = excelPkg.Workbook.Worksheets.FirstOrDefault();

            // Read spreadsheet data into a map
            var dataMap = new Dictionary<string, List<KeyValuePair<string, string>>>();

            int totalRows = worksheet.Dimension.Rows;

            for (int i = 1; i <= totalRows; i++)
            {
                var compound = worksheet.Cells[i, compoundCol].Value.ToString();
                var sample = worksheet.Cells[i, sampleCol].Value.ToString();
                var area = worksheet.Cells[i, areaCol].Value.ToString();

                var pair = new KeyValuePair<string, string>(sample, area);

                if (dataMap.ContainsKey(compound))
                {
                    var pairList = dataMap[compound];
                    pairList.Add(pair);
                    dataMap[compound] = pairList;
                }
                else
                {
                    var pairList = new List<KeyValuePair<string, string>>() { pair };
                    dataMap.Add(compound, pairList);
                }
            }

            progressWindow.progressTextBox.AppendLine("Finished reading data.\r\nWriting data...");
            WriteOutputFile.FormatToColumns(excelPkg, dataMap);
            progressWindow.progressTextBox.AppendLine("Removing NA...");
            WriteOutputFile.RemoveNA(excelPkg);
            progressWindow.progressTextBox.AppendLine("Calculating ratios...");
            WriteOutputFile.CalculateRatios(excelPkg);
            // More WriteOutputFile....
            progressWindow.progressTextBox.AppendLine("Done");
            progressWindow.UseWaitCursor = false;
        }
    }
}