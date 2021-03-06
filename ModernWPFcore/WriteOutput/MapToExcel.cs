﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ModernWPFcore.Pages;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ModernWPFcore
{
    class MapToExcel
    {
        /// <summary>
        /// Writes sample names into rows in a specified column.
        /// </summary>
        /// <param name="dataMap"></param>
        /// <param name="excelPkg"></param>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public static ExcelPackage WriteSamplesInRows(
            OrderedDictionary dataMap, ExcelPackage excelPkg, string tabName, 
            int sampleCol = 1, int startRow = 2)
        {
            // Create new tab or write in existing tab
            ExcelWorksheet worksheet;
            try
            {
                worksheet = excelPkg.Workbook.Worksheets.Add(tabName);
            }
            catch
            {
                worksheet = excelPkg.Workbook.Worksheets[tabName];
            }

            // Header
            if (startRow - 1 >= 1)
            {
                var header = worksheet.Cells[startRow - 1, sampleCol]?.Value?.ToString();
                if (string.IsNullOrWhiteSpace(header))
                    worksheet.Cells[startRow - 1, sampleCol].Value = "Sample";
            }

            // Write sample names in column, starting in startRow
            var r = startRow;
            foreach (string sampleName in ((OrderedDictionary) dataMap[0]).Keys)
            {
                worksheet.Cells[r++, sampleCol].Value = sampleName;
            }

            excelPkg.Save();
            return excelPkg;
        }

        /// <summary>
        /// Writes sample names into rows in a specified column.
        /// </summary>
        /// <param name="dataMap"></param>
        /// <param name="options"></param>
        /// <param name="excelPkg"></param>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public static ExcelPackage WriteCompoundsInColumns(
            OrderedDictionary dataMap, ExcelPackage excelPkg, string tabName, 
            int compoundRow = 2, int startCol = 2)
        {
            // Create new tab or write in existing tab
            ExcelWorksheet worksheet;
            try
            {
                worksheet = excelPkg.Workbook.Worksheets.Add(tabName);
            }
            catch
            {
                worksheet = excelPkg.Workbook.Worksheets[tabName];
            }

            // Write compound names in row, starting in column startCol
            var c = startCol;
            foreach (string compoundName in dataMap.Keys)
            {
                worksheet.Cells[compoundRow, c++].Value = compoundName;
            }

            excelPkg.Save();
            return excelPkg;
        }

        /// <summary>
        /// Writes data into a new Excel tab with samples in columns (in row 1)
        /// and compounds in rows (in column 1) 
        /// </summary>
        /// <param name="dataMap"></param>
        /// <param name="options"></param>
        /// <param name="excelPkg"></param>
        /// <param name="tabName"></param>
        /// <param name="progressPage"></param>
        /// <returns></returns>
        public static ExcelPackage WriteSamplesInColumns(
            OrderedDictionary dataMap, ExcelPackage excelPkg, string tabName,
            Dictionary<string, string> options, int startRow = 2, int startCol = 2)
        {
            // Create new tab or write in existing tab
            ExcelWorksheet worksheet;
            try
            {
                worksheet = excelPkg.Workbook.Worksheets.Add(tabName);
            }
            catch
            {
                worksheet = excelPkg.Workbook.Worksheets[tabName];
            }

            // First cell
            worksheet.Cells[1, 1].Value = "Compound";

            // Write compound names in column, starting in startRow
            var compoundCol = ExcelUtils.GetColNum(options["CompoundLoc"]);
            var r = startRow;
            foreach (string compoundName in dataMap.Keys)
            {
                worksheet.Cells[r++, compoundCol].Value = compoundName;
            }

            // Write sample names in row 1, starting in column 2
            var sampleRow = ExcelUtils.GetRowNum(options["SampleLoc"]);
            var c = startCol;
            foreach (string sampleName in ((OrderedDictionary)dataMap[0]).Keys)
            {
                worksheet.Cells[sampleRow, c++].Value = sampleName;
            }

            // Write data into cells corresponding with sample and compound names
            excelPkg = WriteIntoTemplate(dataMap, excelPkg, options, tabName, false, startRow, startCol);

            // Save file
            excelPkg.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));

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
                                $"Check if template and samples are in rows or columns.");
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

            // TODO - check that save as new name works
            //excelPkg.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));
            excelPkg.Save();
            return excelPkg;
        }

    }
}
