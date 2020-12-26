using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using ModernWPFcore.Pages;
using OfficeOpenXml.Style;

namespace ModernWPFcore
{
    class MapToExcel
    {
        public static void WriteSamplesInRows(
            OrderedDictionary dataMap, Dictionary<string, string> options, 
            ExcelPackage excelPkg, string tabName, ProgressPage progressPage)
        {
            // Create new tab
            var worksheet = excelPkg.Workbook.Worksheets.Add(tabName);

            // First cell
            worksheet.Cells[1, 1].Value = "Sample Names";

            var numCompounds = dataMap.Count;
            var numSamples = 0;
            try
            {
                numSamples = ((OrderedDictionary) dataMap[0]).Count;
            }
            catch
            {
                MessageBox.Show("No samples found");
                return;
            }

            // Write sample names in column 1, starting in row 2
            var r = 2;
            foreach (string sampleName in ((OrderedDictionary) dataMap[0]).Keys)
            {
                worksheet.Cells[r++, 1].Value = sampleName;
            }

            // Write compound names in row 1, starting in column 2
            var c = 2;
            foreach (string compoundName in dataMap.Keys)
            {
                worksheet.Cells[1, c++].Value = compoundName;
            }

            // Write each compound data (column)
            for (int col = 2; col <= numCompounds + 1; ++col)
            {
                for (int row = 2; row <= numSamples + 1; ++row)
                {
                    // Write peak area corresponding to compound and sample name
                    // Compounds in row 1
                    var compound = worksheet.Cells[1, col]?.Value?.ToString();
                    if (compound != null && dataMap.Contains(compound))
                    {
                        // Read sample name from column 1, lookup data for sample in map
                        var sampleName = worksheet.Cells[row, 1]?.Value?.ToString();
                        if (sampleName is null) continue;
                        var data = ((OrderedDictionary) dataMap[compound])[sampleName]?.ToString();

                        // Write data to cell, as a number if possible
                        if (double.TryParse(data, out double dataNum))
                            worksheet.Cells[row, col].Value = dataNum;
                        else worksheet.Cells[row, col].Value = data;
                    }
                }
            }
            worksheet.Cells[2, 2, numSamples + 1, numCompounds + 1].Style.Numberformat.Format = "0";
            worksheet.Cells[2, 2, numSamples + 1, numCompounds + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            excelPkg.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));
            progressPage.ProgressTextBox.AppendText("Finished writing output file\n");
        }

        public static void WriteSamplesInColumns(
            OrderedDictionary dataMap, Dictionary<string, string> options, 
            ExcelPackage excelPkg, string tabName, ProgressPage progressPage)
        {
            // Create new tab
            var worksheet = excelPkg.Workbook.Worksheets.Add(tabName);

            // First cell
            worksheet.Cells[1, 1].Value = "Compounds";

            var numCompounds = dataMap.Count;
            var numSamples = 0;
            try
            {
                numSamples = ((OrderedDictionary)dataMap[0]).Count;
            }
            catch
            {
                MessageBox.Show("No samples found");
                return;
            }

            // Write compound names in column 1, starting in row 2
            var r = 2;
            foreach (string compoundName in dataMap.Keys)
            {
                worksheet.Cells[r++, 1].Value = compoundName;
            }

            // Write sample names in row 1, starting in column 2
            var c = 2;
            foreach (string sampleName in ((OrderedDictionary)dataMap[0]).Keys)
            {
                worksheet.Cells[1, c++].Value = sampleName;
            }

            // Write each compound data (row)
            for (int row = 2; row <= numCompounds + 1; ++row)
            {
                for (int col = 2; col <= numSamples + 1; ++col)
                {
                    // Write peak area corresponding to compound and sample name
                    // Compounds in column 1
                    var compound = worksheet.Cells[row, 1]?.Value?.ToString();
                    if (compound != null && dataMap.Contains(compound))
                    {
                        // Read sample name from row 1, lookup data for sample in map
                        var sampleName = worksheet.Cells[1, col]?.Value?.ToString();
                        if (sampleName is null) continue;
                        var data = ((OrderedDictionary)dataMap[compound])[sampleName]?.ToString();

                        // Write data to cell, as a number if possible
                        if (double.TryParse(data, out double dataNum))
                            worksheet.Cells[row, col].Value = dataNum;
                        else worksheet.Cells[row, col].Value = data;
                    }
                }
            }
            worksheet.Cells[2, 2, numCompounds + 1, numSamples + 1].Style.Numberformat.Format = "0";
            worksheet.Cells[2, 2, numCompounds + 1, numSamples + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            excelPkg.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));
            progressPage.ProgressTextBox.AppendText("Finished writing output file\n");
        }

        public static void WriteIntoTemplate(
            OrderedDictionary dataMap, ExcelPackage excelPkg, string tabName, 
            int compoundsLoc, int samplesLoc, string samplesIn = "rows")
        {
            var worksheet = excelPkg.Workbook.Worksheets[tabName];

            if (samplesIn == "rows")
            {
                // Samples in rows in one column
                var numSamples = ExcelUtils.RowsInColumn(worksheet, samplesLoc);
                var numCompounds = ExcelUtils.ColumnsInRow(worksheet, compoundsLoc);

                // Write each compound data (column)
                for (int col = 2; col <= numCompounds + 1; ++col)
                {
                    for (int row = 2; row <= numSamples + 1; ++row)
                    {
                        // Write peak area corresponding to compound and sample name
                        // Compounds in row 1
                        var compound = worksheet.Cells[1, col]?.Value?.ToString();
                        if (compound != null && dataMap.Contains(compound))
                        {
                            // Read sample name from column 1, lookup data for sample in map
                            var sampleName = worksheet.Cells[row, 1]?.Value?.ToString();
                            if (sampleName is null) continue;
                            var data = ((OrderedDictionary)dataMap[compound])[sampleName]?.ToString();

                            // Write data to cell, as a number if possible
                            if (double.TryParse(data, out double dataNum))
                                worksheet.Cells[row, col].Value = dataNum;
                            else worksheet.Cells[row, col].Value = data;
                        }
                    }
                }
            }
            else
            {
                // Samples in columns in one row
                var numSamples = ExcelUtils.ColumnsInRow(worksheet, samplesLoc);
                var numCompounds = ExcelUtils.RowsInColumn(worksheet, compoundsLoc);

                // Write each compound data (row)
                for (int row = 2; row <= numCompounds + 1; ++row)
                {
                    for (int col = 2; col <= numSamples + 1; ++col)
                    {
                        // Write peak area corresponding to compound and sample name
                        // Compounds in column 1
                        var compound = worksheet.Cells[row, 1]?.Value?.ToString();
                        if (compound != null && dataMap.Contains(compound))
                        {
                            // Read sample name from row 1, lookup data for sample in map
                            var sampleName = worksheet.Cells[1, col]?.Value?.ToString();
                            if (sampleName is null) continue;
                            var data = ((OrderedDictionary)dataMap[compound])[sampleName]?.ToString();

                            // Write data to cell, as a number if possible
                            if (double.TryParse(data, out double dataNum))
                                worksheet.Cells[row, col].Value = dataNum;
                            else worksheet.Cells[row, col].Value = data;
                        }
                    }
                }
            }
        }
    }
}
