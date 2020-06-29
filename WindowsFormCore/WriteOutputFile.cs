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
        private static string outputFileName = "out";

        public static void Run(bool removeNA, bool replaceNA, string missingValPercent, string missingValReplace,
                               ProgressWindow progressWindow, Dictionary<string, Dictionary<string, string>> dataMap,
                               IsotopeCalc isotopeCalc)
        {
            var excelPkg = new ExcelPackage();

            FormatToColumns(excelPkg, dataMap);

            if (removeNA)
            {
                progressWindow.progressTextBox.AppendLine("Removing NA");
                RemoveNA(excelPkg, missingValPercent, dataMap);
            }

            if (replaceNA)
            {
                progressWindow.progressTextBox.AppendLine($"Replacing NA with {missingValReplace}...");
                ReplaceNA(excelPkg, "Compounds Detected", missingValReplace);
            }

            if (isotopeCalc.isotopeToCompound.Count > 0)
            {
                progressWindow.progressTextBox.AppendLine("Calculating ratios");
                CalculateRatios(excelPkg, isotopeCalc, dataMap);
            }

            progressWindow.progressTextBox.AppendLine("Calculating concentrations");
            Concentration(excelPkg, isotopeCalc);
            // More WriteOutputFile....
            progressWindow.progressTextBox.AppendLine("Done");
            progressWindow.UseWaitCursor = false;
        }

        public static void FormatToColumns(ExcelPackage excelPkg,
                                           Dictionary<string, Dictionary<string, string>> dataMap)
        {
            var compoundList = new List<string>(dataMap.Keys);
            var sampleList = new List<string>(dataMap[compoundList[0]].Keys);
            int numCompounds = compoundList.Count;
            int numSamples = sampleList.Count;

            // Write data to new excel worksheet
            var outputSheet = excelPkg.Workbook.Worksheets.Add("Formatted Data");

            // Populate first row with compounds
            outputSheet.Cells[1, 1].Value = "Sample";
            for (int col = 2; col <= numCompounds + 1; col++)
            {
                outputSheet.Cells[1, col].Value = compoundList[col - 2];
            }

            // Write each sample (row)
            for (int i = 2; i <= numSamples + 1; i++)
            {
                // Write sample name in first column
                outputSheet.Cells[i, 1].Value = sampleList[i - 2];

                for (int j = 2; j <= numCompounds + 1; j++)
                {
                    // Write peak area corresponding to compound and sample name
                    dataMap[outputSheet.Cells[1, j].Value.ToString()].TryGetValue(outputSheet.Cells[i, 1].Value.ToString(), out var peakArea);
                    if (double.TryParse(peakArea, out double peakAreaNum)) outputSheet.Cells[i, j].Value = peakAreaNum;
                    else outputSheet.Cells[i, j].Value = peakArea;
                }
            }
            outputSheet.Cells[2, 2, numSamples + 1, numCompounds + 1].Style.Numberformat.Format = "0";

            SaveFile(excelPkg, outputFileName);
        }

        public static void RemoveNA(ExcelPackage excelPkg, string missingValPercent, Dictionary<string, Dictionary<string, string>> dataMap)
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
                if (count < cutoffCount)
                {
                    dataMap.Remove(detectedSheet.Cells[1, col].Value.ToString());
                    detectedSheet.DeleteColumn(col);
                }
            }
            SaveFile(excelPkg, outputFileName);
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
            SaveFile(excelPkg, outputFileName);
        }

        public static void CalculateRatios(ExcelPackage excelPkg, IsotopeCalc isotopeCalc,
                                           Dictionary<string, Dictionary<string, string>> detectedMap)
        {
            var compoundList = new List<string>(detectedMap.Keys);
            var sampleList = new List<string>(detectedMap[compoundList[0]].Keys);
            int numSamples = sampleList.Count;

            // Write data to new excel worksheet
            var isotopeRatioSheet = excelPkg.Workbook.Worksheets.Add("Isotope Ratio");
            isotopeRatioSheet.Cells[1, 1].Value = "Sample";

            // Fill in sample names
            for (int r = 2; r <= numSamples + 1; r++)
            {
                isotopeRatioSheet.Cells[r, 1].Value = sampleList[r - 2]; // Write sample name in first column
            }

            int col = 2;
            foreach (var isotope in isotopeCalc.isotopeToCompound.Keys)
            {
                foreach (var compound in isotopeCalc.isotopeToCompound[isotope].Where(compound => compoundList.Contains(compound)))
                {
                    isotopeRatioSheet.Cells[1, col].Value = compound; // Put compound name in first row
                    for (int row = 2; row <= numSamples + 1; row++) // Fill in sample ratios
                    {
                        // Get area value from map with compound/sample match
                        var sampleName = isotopeRatioSheet.Cells[row, 1].Value.ToString();
                        detectedMap[compound].TryGetValue(sampleName, out var compoundArea);
                        if (double.TryParse(compoundArea, out var compoundAreaNum))
                        {
                            detectedMap[isotope].TryGetValue(sampleName, out var isotopeArea);
                            if (double.TryParse(isotopeArea, out var isotopeAreaNum))
                            {
                                isotopeRatioSheet.Cells[row, col].Value = compoundAreaNum / isotopeAreaNum;
                            }
                            else isotopeRatioSheet.Cells[row, col].Value = "No Isotope";
                        }
                        else isotopeRatioSheet.Cells[row, col].Value = 0.0;
                    }
                    col++;
                }
            }

            isotopeRatioSheet.Cells[2, 2, numSamples + 1, col - 1].Style.Numberformat.Format = "0.000000";
            isotopeRatioSheet.Cells.AutoFitColumns();
            SaveFile(excelPkg, outputFileName);
        }

        public static void Concentration(ExcelPackage excelPkg, IsotopeCalc isotopeCalc)
        {
            // Copy ratios data to new sheet to do calculation
            var concentrationSheet = excelPkg.Workbook.Worksheets.Copy("Isotope Ratio", "Concentrations");
            var rows = concentrationSheet.Dimension.Rows;
            var cols = concentrationSheet.Dimension.Columns;

            // Iterate through compounds
            int col = 2;
            for (int c = 2; c <= cols; c++)
            {
                var compound = concentrationSheet.Cells[1, c].Text;
                // Get slope and intercept for compound
                var slopeIntercept = isotopeCalc.SlopeIntercept("filePath", compound);

                // Calculate for each sample
                for (int r = 2; r <= rows; r++)
                {
                    var ratio = concentrationSheet.Cells[r, c].Text;
                    if (double.TryParse(ratio, out var ratioNum))
                    {
                        concentrationSheet.Cells[r, c].Value = (ratioNum - slopeIntercept[1]) / slopeIntercept[0];
                    }
                }
            }

            //isotopeRatioSheet.Cells[2, 2, numSamples + 1, numCompounds + 1].Style.Numberformat.Format = "0.000000";
            concentrationSheet.Cells.AutoFitColumns();
            SaveFile(excelPkg, outputFileName);
        }

        /* SAVE FILE */
        public static void SaveFile(ExcelPackage excelPkg, string filename)
        {
            excelPkg.SaveAs(new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{filename}.xlsx"));
        }

    }
}
