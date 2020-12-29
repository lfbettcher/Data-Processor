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

        // Writes data into a new Excel tab with samples in rows (in column 1)
        // and compounds in columns (in row 1)
        public static ExcelPackage WriteSamplesInRows(
            OrderedDictionary dataMap, Dictionary<string, string> options, 
            ExcelPackage excelPkg, string tabName, ProgressPage progressPage)
        {
            // Create new tab
            var worksheet = excelPkg.Workbook.Worksheets.Add(tabName);

            // First cell
            worksheet.Cells[1, 1].Value = "Sample Name";

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

            // Write data into cells corresponding with sample and compound names
            excelPkg = WriteIntoTemplate(dataMap, excelPkg, options, tabName, false, 2, 2);

            // Save file
            excelPkg.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));

            // Progress text
            progressPage.ProgressTextBox.AppendText("Finished writing data\n");
            return excelPkg;
        }


        // Writes data into a new Excel tab with samples in columns (in row 1)
        // and compounds in rows (in column 1)
        public static ExcelPackage WriteSamplesInColumns(
            OrderedDictionary dataMap, Dictionary<string, string> options, 
            ExcelPackage excelPkg, string tabName, ProgressPage progressPage)
        {
            // Create new tab
            var worksheet = excelPkg.Workbook.Worksheets.Add(tabName);

            // First cell
            worksheet.Cells[1, 1].Value = "Compound";

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

            // Write data into cells corresponding with sample and compound names
            excelPkg = WriteIntoTemplate(dataMap, excelPkg, options, tabName, false, 2, 2);

            // Save file
            excelPkg.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));

            // Progress text
            progressPage.ProgressTextBox.AppendText("Finished writing output file\n");
            return excelPkg;
        }


        // Writes data into Excel tab containing compound and sample names.
        // Compound names must be in the sheet. Sample names are optional
        // and will be checked for and filled in if missing. Put sample names in sheet
        // if you want a particular order.
        //
        // The following are optional parameters and can be omitted if using info from options dictionary:
        // useOptions - default is true. If false, you must fill in the remaining parameters
        // startRow and startCol - where to start writing data
        // sampleLoc and compoundLoc - where the sample and compound names are located
        public static ExcelPackage WriteIntoTemplate(
            OrderedDictionary dataMap, ExcelPackage excelPkg, Dictionary<string, string> options, string templateTab,
            bool useOptions = true, int startRow = 2, int startCol = 2, int sampleLoc = 1, int compoundLoc = 1)
        {
            var worksheet = excelPkg.Workbook.Worksheets[templateTab];
            int numSamples, numCompounds, endRow, endCol;

            // Check if map contains sample names, prevents index error later
            List<string> samplesKeys;
            try
            {
                samplesKeys = ((OrderedDictionary) dataMap[0]).Keys.Cast<string>().ToList();
            }
            catch
            {
                MessageBox.Show($"No data to write in {templateTab}. " +
                                $"Check if template and samples are in rows/columns.");
                return excelPkg;
            }

            // Get row and col to start writing
            if (useOptions)
            {
                (startRow, startCol) = ExcelUtils.GetRowCol(options["StartInCell"]);

                // Convert column name to number
                sampleLoc = int.TryParse(options["SampleLoc"], out var sampleLocNum)
                    ? sampleLocNum
                    : ExcelUtils.ColumnNameToNumber(options["SampleLoc"]);
                compoundLoc = int.TryParse(options["CompoundLoc"], out var compoundLocNum)
                    ? compoundLocNum
                    : ExcelUtils.ColumnNameToNumber(options["CompoundLoc"]);
            }

            if (options["SamplesOut"] == "rows")
            {
                // Fill in sample names if not in template
                var name = worksheet.Cells[startRow, sampleLoc].Value?.ToString();
                if (name is null || name.Length < 1)
                {
                    var r = startRow;
                    foreach (string sampleName in samplesKeys)
                    {
                        worksheet.Cells[r++, sampleLoc].Value = sampleName;
                    }
                }

                // Get approximate bounds, samples in rows (in column sampleLoc)
                numSamples = ExcelUtils.RowsInColumn(worksheet, sampleLoc);
                numCompounds = ExcelUtils.ColumnsInRow(worksheet, compoundLoc);
                endRow = numSamples + startRow;
                endCol = numCompounds + startCol;

                // Write each compound data (column)
                for (int col = startCol; col <= endCol; ++col)
                {
                    for (int row = startRow; row <= endRow; ++row)
                    {
                        // Write peak area corresponding to compound and sample name
                        // Compound in row compoundLoc
                        var compound = worksheet.Cells[compoundLoc, col]?.Value?.ToString();
                        if (compound != null && dataMap.Contains(compound))
                        {
                            // Read sample name from sample column, lookup data for sample in map
                            var sampleName = worksheet.Cells[row, sampleLoc]?.Value?.ToString();
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
                // Fill in sample names if not in template
                var name = worksheet.Cells[sampleLoc, startCol].Value?.ToString();
                if (name is null || name.Length < 1)
                {
                    var c = startCol;
                    foreach (string sampleName in samplesKeys)
                    {
                        worksheet.Cells[sampleLoc, c++].Value = sampleName;
                    }
                }

                // Get approximate bounds, samples in columns (in row sampleLoc)
                numCompounds = ExcelUtils.RowsInColumn(worksheet, compoundLoc);
                numSamples = ExcelUtils.ColumnsInRow(worksheet, sampleLoc);
                endRow = numCompounds + startRow;
                endCol = numSamples + startCol;

                // Write each compound data (row)
                for (int row = startRow; row <= endRow; ++row)
                {
                    for (int col = startCol; col <= endCol; ++col)
                    {
                        // Write peak area corresponding to compound and sample name
                        // Compounds in column compoundLoc
                        var compound = worksheet.Cells[row, compoundLoc]?.Value?.ToString();
                        if (compound != null && dataMap.Contains(compound))
                        {
                            // Read sample name from row, lookup data for sample in map
                            var sampleName = worksheet.Cells[sampleLoc, col]?.Value?.ToString();
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

            worksheet.Cells[startRow, startCol, endRow, endCol].Style.Numberformat.Format = "0";
            worksheet.Cells[startRow, startCol, endRow, endCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            excelPkg.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));
            //progressPage.ProgressTextBox.AppendText("Finished writing into template\n");
            return excelPkg;
        }
    }
}
