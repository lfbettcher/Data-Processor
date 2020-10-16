using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace WindowsFormCore
{
    class WriteOutputFile
    {
        private static string outputFileName = "DataProcessor\\bile-acid_out";

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

        /// <summary>
        /// Writes dataMap to excel spreadsheet.
        /// Compounds are in the first or second column, sample names are in the nameRow.
        /// Column 1 are compound names without MRM, column 2 has MRM
        /// </summary>
        /// <param name="progressWindow"></param>
        /// <param name="dataMap"></param>
        /// <param name="filePath"></param>
        public static void WriteMapToSheetCompoundsInRows(ProgressWindow progressWindow,
            Dictionary<string, Dictionary<string, string>> dataMap, string filePath, string tabName, int nameRow)
        {
            // Open workbook and specified tab
            var excelPkg = new ExcelPackage(new FileInfo(filePath));
            var outputSheet = excelPkg.Workbook.Worksheets[tabName];

            // Info to write
            var compoundList = new List<string>(dataMap.Keys);
            var sampleList = new List<string>(dataMap[compoundList[0]].Keys);
            int numSamples = sampleList.Count;

            int rows = outputSheet.Dimension.Rows;

            // Write sample names in nameRow, starting from col 5
            for (int col = 5; col < numSamples + 5; ++col)
            {
                outputSheet.Cells[nameRow, col].Value = sampleList[col - 5];
            }

            // Write each compound (row)
            for (int row = nameRow + 1; row <= rows; ++row)
            {
                for (int col = 5; col < numSamples + 5; col++)
                {
                    // Write peak area corresponding to compound and sample name
                    // Compounds in col 1 or col 2 depending on MRM
                    var compound = outputSheet.Cells[row, 1].Value.ToString();
                    if (compound != null && dataMap.ContainsKey(compound))
                    {
                        // dataMap[compound] get value of sample name in row 1 which is data
                        dataMap[compound].TryGetValue(outputSheet.Cells[1, col].Value.ToString(), out var data);
                        if (double.TryParse(data, out double dataNum)) outputSheet.Cells[row, col].Value = dataNum;
                        else outputSheet.Cells[row, col].Value = data;
                    }
                    // Compound names with MRM are in col 2
                    var compoundMRM = outputSheet.Cells[row, 2].Value.ToString();
                    if (compoundMRM != null && dataMap.ContainsKey(compoundMRM))
                    {
                        // dataMap[compoundMRM] get value of sample name in row 1 which is data
                        dataMap[compoundMRM].TryGetValue(outputSheet.Cells[1, col].Value.ToString(), out var data);
                        if (double.TryParse(data, out double dataNum)) outputSheet.Cells[row, col].Value = dataNum;
                        else outputSheet.Cells[row, col].Value = data;
                    }
                }
            }
            outputSheet.Cells[2, 5, rows, numSamples].Style.Numberformat.Format = "0";
            outputSheet.Cells[2, 5, rows, numSamples].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            excelPkg.SaveAs(new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) +
                                         $"\\DataProcessor\\sciex6500_output.xlsx"));

            //SaveFile(excelPkg, outputFileName);
        }
    

        public static void WriteSciex(bool replaceNA, string missingValReplace, ProgressWindow progressWindow,
            Dictionary<string, Dictionary<string, string>> dataMap, string filePath)
        {
            var inputFile = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var excelPkg = new ExcelPackage(inputFile);
            var worksheet = excelPkg.Workbook.Worksheets.FirstOrDefault();
            int cols = worksheet.Dimension.Columns;
            int rows = worksheet.Dimension.Rows;

            // Write data to next worksheet in run order
            var outputSheet = excelPkg.Workbook.Worksheets.Last();
            var numCompounds = outputSheet.Dimension.Columns - 1;
            var numSamples = outputSheet.Dimension.Rows - 1;

            // Write each compound (col)
            for (int c = 2; c <= numCompounds + 1; c++)
            {
                // Write each sample
                for (int r = 2; r <= numSamples + 1; r++)
                {
                    // Write peak area corresponding to compound and sample name
                    var compoundName = outputSheet.Cells[1, c].Value.ToString();
                    dataMap[compoundName].TryGetValue(outputSheet.Cells[r, 1].Value.ToString(), out var peakArea);
                    if (double.TryParse(peakArea, out double peakAreaNum)) outputSheet.Cells[r, c].Value = peakAreaNum;
                    else outputSheet.Cells[r, c].Value = peakArea;
                }
            }
            outputSheet.Cells[2, 2, numSamples + 1, numCompounds + 1].Style.Numberformat.Format = "0";
            SaveFile(excelPkg, "riseout3");
            if (replaceNA)
            {
                progressWindow.progressTextBox.AppendLine($"Replacing NA with {missingValReplace}");
                ReplaceNA(excelPkg, "order", missingValReplace);
            }
            SaveFile(excelPkg, "riseoutNoNA");
            progressWindow.progressTextBox.AppendLine("Normalizing to QC");
            NormalizeToQC(excelPkg, dataMap, "S");
            NormalizeToQC(excelPkg, dataMap, "I");
            progressWindow.progressTextBox.AppendLine("Done");
            progressWindow.UseWaitCursor = false;
        }

        public static void FormatToRunOrder(ExcelPackage excelPkg,
            Dictionary<string, Dictionary<string, string>> dataMap)
        {
            //
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

        /// <summary>
        /// Calculates the ratio compound area / isotope area.
        /// </summary>
        /// <param name="excelPkg"></param>
        /// <param name="isotopeCalc"></param>
        /// <param name="detectedMap"></param>
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
                        if (ratioNum == 0.0)
                        {
                            concentrationSheet.Cells[r, c].Value = 0.0;
                        }
                        else
                        {
                            concentrationSheet.Cells[r, c].Value = (ratioNum - slopeIntercept[1]) / slopeIntercept[0];
                        }
                    }
                }
            }

            //isotopeRatioSheet.Cells[2, 2, numSamples + 1, numCompounds + 1].Style.Numberformat.Format = "0.000000";
            concentrationSheet.Cells.AutoFitColumns();
            SaveFile(excelPkg, outputFileName);
        }

        public static void NormalizeToQC(ExcelPackage excelPkg, Dictionary<string, Dictionary<string, string>> dataMap, string qcSorI)
        {
            var normalizedSheet = excelPkg.Workbook.Worksheets.Copy("order", "normalizedToQC"+qcSorI);
            var rows = normalizedSheet.Dimension.Rows;
            var cols = normalizedSheet.Dimension.Columns;

            // Iterate through samples QC groups
            int start = 2;
            while (start <= rows - 2)
            {
                int twoQC = 0;
                int row = start;
                int n = 0;
                var sampleQC = new List<string>();

                while (twoQC < 2 && row <= rows)
                {
                    var sampleName = normalizedSheet.Cells[row, 1].Value.ToString(); 
                    if (qcSorI.ToUpper().Equals("I"))
                    {
                        if (sampleName.Contains('I'))
                        {
                            ++twoQC;
                            sampleQC.Add(sampleName);
                        }
                        else if (!sampleName.Contains('S'))
                        {
                            ++n;
                            sampleQC.Add(sampleName);
                        }
                        ++row;
                    }
                    else
                    {
                        if (sampleName.Contains('S'))
                        { 
                            ++twoQC;
                            sampleQC.Add(sampleName);
                        }
                        else if (!sampleName.Contains('I'))
                        {
                            ++n;
                            sampleQC.Add(sampleName);
                        }
                        ++row;
                    }
                }

                int rowStart = row - sampleQC.Count - 1;
                int rowEnd = row - 1;

                // Do correction for each compound (column)
                for (int c = 2; c <= cols; c++)
                {
                    var compoundName = normalizedSheet.Cells[1, c].Value.ToString();

                    // Get peak areas for QCs
                    var QC1 = AreaOrZero(compoundName, sampleQC.First(), dataMap);
                    var QC2 = AreaOrZero(compoundName, sampleQC.Last(), dataMap);

                    // QC mixture sum for samples
                    for (int i = 1; i < sampleQC.Count - 1; ++i)
                    {
                        var sampleName = sampleQC[i];
                        var qcMixSum = QC1 * (1 - (i / (n + 1.0))) + QC2 * (i / (n + 1.0));
                        var peakArea = AreaOrZero(compoundName, sampleName, dataMap);
                        if (qcMixSum == 0) continue;

                        // Get row index of sampleName
                        var findRow = from cell in normalizedSheet.Cells[$"A{rowStart}:A{rowEnd}"]
                                                    where cell.Value.ToString() == sampleName
                                                    select cell.Start.Row;
                        normalizedSheet.Cells[findRow.First(), c].Value = peakArea / qcMixSum;
                    }
                }

                //// Get ready for next group of QCs, check if next is another QC
                //if (row > rows) break;
                //var nextIsQC = normalizedSheet.Cells[row, 1].Value.ToString().Contains('Q');
                //if (nextIsQC) start = row - 1;
                //else start = row - 2;
                start = row - 1;
            }
            normalizedSheet.Cells[2, 2, rows, cols].Style.Numberformat.Format = "0.00000";
            SaveFile(excelPkg, "riseNorm");
        }

        public static double AreaOrZero(string compoundName, string sampleName,
            Dictionary<string, Dictionary<string, string>> dataMap)
        {
            dataMap[compoundName].TryGetValue(sampleName, out var peakAreaStr);
            return double.TryParse(peakAreaStr, out double peakAreaNum) ? peakAreaNum : 0.0;
        }

        /* SAVE FILE */
        public static void SaveFile(ExcelPackage excelPkg, string filename)
        {
            excelPkg.SaveAs(new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{filename}.xlsx"));
        }

    }
}
