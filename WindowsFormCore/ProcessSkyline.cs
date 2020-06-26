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
            ProgressWindow progressWindow = (ProgressWindow)Application.OpenForms["Form2"];
            progressWindow.progressTextBox.SetText("Opening file...");

            // Get first worksheet
            FileInfo inputFile = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            ExcelPackage ExcelPkg = new ExcelPackage(inputFile);
            ExcelWorksheet worksheet = ExcelPkg.Workbook.Worksheets.FirstOrDefault();

            // Read spreadsheet data into a map
            Dictionary<string, List<KeyValuePair<string, string>>> dataMap =
                new Dictionary<string, List<KeyValuePair<string, string>>>();

            int totalRows = worksheet.Dimension.Rows;

            for (int i = 1; i <= totalRows; i++)
            {
                string compound = worksheet.Cells[i, compoundCol].Value.ToString();
                string sample = worksheet.Cells[i, sampleCol].Value.ToString();
                string area = worksheet.Cells[i, areaCol].Value.ToString();

                KeyValuePair<string, string> pair =
                    new KeyValuePair<string, string>(sample, area);

                if (dataMap.ContainsKey(compound))
                {
                    List<KeyValuePair<string, string>> pairList = dataMap[compound];
                    pairList.Add(pair);
                    dataMap[compound] = pairList;
                }
                else
                {
                    List<KeyValuePair<string, string>> pairList =
                        new List<KeyValuePair<string, string>>() { pair };
                    dataMap.Add(compound, pairList);
                }
            }

            progressWindow.progressTextBox.AppendLine("Finished reading data.\r\nWriting data...");
            WriteOutputFile.FormatToColumns(ExcelPkg, dataMap);
            progressWindow.progressTextBox.AppendLine("Removing NA...");
            WriteOutputFile.RemoveNA(ExcelPkg);
            // More WriteOutputFile....
            progressWindow.progressTextBox.AppendLine("Done");
            progressWindow.UseWaitCursor = false;
        }
    }
}