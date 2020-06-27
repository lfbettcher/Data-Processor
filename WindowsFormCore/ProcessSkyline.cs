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
            Run(string filePath, int compoundCol, int sampleCol, int areaCol)
        {
            var progressWindow = (ProgressWindow)Application.OpenForms["Form2"];
            progressWindow.progressTextBox.SetText("Opening file...");

            // Get first worksheet
            var inputFile = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var excelPkg = new ExcelPackage(inputFile);
            var worksheet = excelPkg.Workbook.Worksheets.FirstOrDefault();

            // Read spreadsheet data into a map
            var dataMap = new Dictionary<string, Dictionary<string, string>>();

            int totalRows = worksheet.Dimension.Rows;

            for (int i = 1; i <= totalRows; i++)
            {
                var compound = worksheet.Cells[i, compoundCol].Value.ToString();
                var sample = worksheet.Cells[i, sampleCol].Value.ToString();
                var area = worksheet.Cells[i, areaCol].Value.ToString();

                if (dataMap.ContainsKey(compound)) dataMap[compound].Add(sample, area);
                /*
                {
                    var sampleAreaMap = dataMap[compound];
                    sampleAreaMap.Add(sample, area);
                    dataMap[compound] = sampleAreaMap;
                }
                */
                else dataMap.Add(compound, new Dictionary<string, string> {{sample, area}});
            }
            return dataMap;
        }
    }
}