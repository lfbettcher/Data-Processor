using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OfficeOpenXml;

namespace WindowsFormCore
{
    class WriteOutputFile
    {
        public static void Main(bool removeNA, bool replaceNA, string missingValPercent, string missingValReplace, 
            ProgressWindow progressWindow, Dictionary<string, List<KeyValuePair<string, string>>> dataMap)
        {
            var excelPkg = new ExcelPackage();

            FormatToColumns(excelPkg, dataMap);

            if (removeNA)
            {
                progressWindow.progressTextBox.AppendLine("Removing NA...");
                RemoveNA(excelPkg, missingValPercent);
            }

            if (replaceNA)
            {
                progressWindow.progressTextBox.AppendLine($"Replacing NA with {missingValReplace}...");
                ReplaceNA(excelPkg, "Compounds Detected", missingValReplace);
            }

            /*
            progressWindow.progressTextBox.AppendLine("Calculating ratios...");
            CalculateRatios(excelPkg);
            */
            // More WriteOutputFile....
            progressWindow.progressTextBox.AppendLine("Done");
            progressWindow.UseWaitCursor = false;
        }

        public static void FormatToColumns(ExcelPackage excelPkg,
            Dictionary<string, List<KeyValuePair<string, string>>> dataMap)
        {
            var compoundList = new List<string>(dataMap.Keys);
            int numCompounds = dataMap.Count - 1;
            int numSamples = dataMap[compoundList[1]].Count;

            // Write data to new sheet
            ExcelWorksheet outputSheet = excelPkg.Workbook.Worksheets.Add("Formatted Data");

            // Populate first row with compounds
            outputSheet.Cells[1, 1].Value = "Sample";
            for (int col = 2; col <= numCompounds + 1; col++)
            {
                outputSheet.Cells[1, col].Value = compoundList[col - 1];
            }

            // Write each sample (row)
            for (int i = 2; i <= numSamples + 1; i++)
            {
                // Write sample number in first column
                outputSheet.Cells[i, 1].Value = i - 1;

                for (int j = 2; j <= numCompounds + 1; j++)
                {
                    // Check if sample name in first column matches Key and compound name in first row matches Value
                    if (string.Equals(outputSheet.Cells[i, 1].Value.ToString(), dataMap[compoundList[j - 1]][i - 2].Key) &&
                        string.Equals(outputSheet.Cells[1, j].Value, compoundList[j - 1]))
                    {
                        // Write peak area
                        var peakArea = dataMap[compoundList[j - 1]][i - 2].Value;

                        if (double.TryParse(peakArea, out double peakAreaNum))
                        {
                            outputSheet.Cells[i, j].Value = peakAreaNum;
                        }
                        else
                        {
                            outputSheet.Cells[i, j].Value = peakArea;
                        }
                    }
                }
            }
            outputSheet.Cells[2, 2, numSamples + 1, numCompounds + 1].Style.Numberformat.Format = "0";

            SaveFile(excelPkg, "out");
        }

        public static void RemoveNA(ExcelPackage excelPkg, string missingValPercent)
        {
            // Make copy of sheet and remove #NA
            var detectedSheet = excelPkg.Workbook.Worksheets.Copy("Formatted Data", "Compounds Detected");

            int cols = detectedSheet.Dimension.Columns;
            int rows = detectedSheet.Dimension.Rows;

            var cutoffCount = 1.0;

            if (double.TryParse(missingValPercent, out var cutoffPercent))
            {
                cutoffCount = (100 - cutoffPercent) / 100 * (rows - 1);
                System.Diagnostics.Debug.WriteLine(cutoffCount);
            }

            // Delete columns below cutoff count
            for (int col = cols; col > 1; col--)
            {
                var count = detectedSheet.Cells[2, col, rows, col].Count(n => double.TryParse(n.Text, out var num));
                if (count < cutoffCount) detectedSheet.DeleteColumn(col);
            }
            SaveFile(excelPkg, "out");
        }

        public static void ReplaceNA(ExcelPackage excelPkg, string sheetName, string missingValReplace)
        {
            var worksheet = excelPkg.Workbook.Worksheets[sheetName];
            int cols = worksheet.Dimension.Columns;
            int rows = worksheet.Dimension.Rows;

            // Replace with number
            if (double.TryParse(missingValReplace, out var replaceNum))
            {
                foreach (var cell in worksheet.Cells[2, 2, rows, cols])
                {
                    var value = cell.Value.ToString();
                    if (!double.TryParse(value, out _)) cell.Value = replaceNum;
                }

            }
            // Replace with string
            else
            {
                foreach (var cell in worksheet.Cells[2, 2, rows, cols])
                {
                    var value = cell.Value.ToString();
                    if (!double.TryParse(value, out _)) cell.Value = missingValReplace;
                }
            }

            worksheet.Cells[2, 2, rows, cols].Style.Numberformat.Format = "0";
            SaveFile(excelPkg, "out");
        }

        public static void CalculateRatios(ExcelPackage excelPackage)
        {

        }

        /* SAVE FILE */
        public static void SaveFile(ExcelPackage excelPkg, string filename)
        {
            excelPkg.SaveAs(new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{filename}.xlsx"));
        }

        /* GET COLUMN LETTER */
        public static string GetColumnLetter(int columnNumber)
        {
            int dividend = columnNumber;
            string columnLetter = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnLetter = Convert.ToChar(65 + modulo).ToString() + columnLetter;
                dividend = (dividend - modulo) / 26;
            }
            return columnLetter;
        }

    }
}
