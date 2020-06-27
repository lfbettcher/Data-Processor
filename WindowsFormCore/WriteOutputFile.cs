using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace WindowsFormCore
{
    class WriteOutputFile
    {
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
            ExcelRange range = outputSheet.Cells[2, 2, numSamples + 1, numCompounds + 1];
            range.Style.Numberformat.Format = "0";

            excelPkg.SaveAs(new FileInfo("C:\\Users\\Lisa\\OneDrive\\Desktop\\out.xlsx"));
        }

        public static void RemoveNA(ExcelPackage excelPkg)
        {
            // Make copy of sheet and remove #NA
            ExcelWorksheet detectedSheet = Copy(excelPkg, "Formatted Data", "Compounds Detected");

            int cols = detectedSheet.Dimension.Columns;
            int rows = detectedSheet.Dimension.Rows;
            for (int col = 2; col <= cols; col++)
            {
                // Count of samples the compound is detected in
                var colLetter = GetColumnLetter(col);
                detectedSheet.Cells[rows + 1, col].Formula = $"COUNT({colLetter}2:{colLetter}{rows})";
            }
            detectedSheet.Cells[rows + 1, 2, rows + 1, cols].Calculate();

            for (int col = cols; col > 1; col--)
            {
                if (string.Equals(detectedSheet.Cells[rows + 1, col].Value.ToString(), "0"))
                {
                    detectedSheet.DeleteColumn(col);
                }
            }
            detectedSheet.DeleteRow(rows + 1);

            excelPkg.SaveAs(new FileInfo("C:\\Users\\Lisa\\OneDrive\\Desktop\\out.xlsx"));
        }

        public static void CalculateRatios(ExcelPackage excelPackage)
        {

        }

        /* COPY EXCEL SHEET */
        public static ExcelWorksheet Copy(ExcelPackage excelPackage, string copyFrom, string copyTo)
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Copy(copyFrom, copyTo);
            return worksheet;
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
