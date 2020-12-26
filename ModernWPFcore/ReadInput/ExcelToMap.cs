using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace ModernWPFcore
{
    class ExcelToMap
    {
        // Read multiple files
        public static OrderedDictionary ReadAllFiles(List<string> pathsList, Dictionary<string, string> options)
        {
            // <compound, <sample name, data>>
            var dataMap = new OrderedDictionary();

            foreach (var filePath in pathsList)
            {
                ExcelPackage excelPkg = new ExcelPackage(new FileInfo(filePath));

                var curMap = ReadAllSheets(excelPkg, options);
                Merge.MergeMaps(dataMap, curMap);
            }

            return dataMap;
        }

        // Read multiple sheets
        public static OrderedDictionary ReadAllSheets(ExcelPackage excelPkg, Dictionary<string, string> options)
        {
            // <compound, <sample name, data>>
            var sheetsMap = new OrderedDictionary();

            foreach (var sheet in excelPkg.Workbook.Worksheets)
            {
                Debug.WriteLine(sheet.Name);
                var curMap = new OrderedDictionary();

                try
                {
                    curMap = options["SamplesIn"] == "rows"
                        ? SamplesInRowsToMap(1, 1, excelPkg, sheet.Name)
                        : SamplesInColumnsToMap(1, 1, excelPkg, sheet.Name);
                }
                catch { }

                Merge.MergeMaps(sheetsMap, curMap);
            }

            return sheetsMap;
        }

        /// <summary>
        /// Reads data from excel sheet where sample IDs are in rows (one column)
        /// and compound names are in columns (one row).
        /// </summary>
        /// <param name="compoundRow">Row containing compound names</param>
        /// <param name="sampleColumn">Column containing sample names</param>
        /// <param name="excelPkg">Excel workbook</param>
        /// <param name="sheetName">Optional: Name of worksheet from which to read data.
        /// Uses first sheet if no name is provided.</param>
        /// <returns>dataMap is an OrderedDictionary with compound name as key and
        ///   value is another OrderedDictionary with sample names as key and data as value
        ///   (compound, (sample name, data))</returns>
        public static OrderedDictionary SamplesInRowsToMap(int compoundRow,
            int sampleColumn, ExcelPackage excelPkg, string sheetName = "_first")
        {
            // <compound, <sample name, data>>
            var dataMap = new OrderedDictionary();

            // Get worksheet - default is first sheet
            var worksheet = excelPkg.Workbook.Worksheets.First();
            // Use sheetName if provided
            if (sheetName != "_first") 
                worksheet = excelPkg.Workbook.Worksheets[sheetName];

            int cols = worksheet.Dimension.Columns;
            int rows = worksheet.Dimension.Rows;

            // Read spreadsheet data into a map
            // Data for each compound starts in column after sample ID column
            // Go compound by compound (column by column)
            for (int c = sampleColumn + 1; c <= cols; ++c)
            {
                // Compounds in specified row
                var compound = worksheet.Cells[compoundRow, c]?.Value?.ToString();
                if (compound is null) continue;

                var samplesMap = new OrderedDictionary();

                // Data starts in row after compound name row
                for (int r = compoundRow + 1; r <= rows; ++r)
                {
                    // Sample name in specified column
                    var sample = worksheet.Cells[r, sampleColumn]?.Value?.ToString();
                    if (sample is null) continue;

                    // Check if sample name already exists
                    if (samplesMap.Contains(sample))
                        sample = Merge.RenameDuplicate(samplesMap.Keys, sample);

                    var area = worksheet.Cells[r, c]?.Value?.ToString();
                    samplesMap.Add(sample, area);
                }

                if (!dataMap.Contains(compound)) dataMap.Add(compound, samplesMap);
            }

            return dataMap;
        }

        /// <summary>
        /// Reads data from excel sheet where sample IDs are in columns (one row)
        /// and compound names are in rows (one column).
        /// </summary>
        /// <param name="compoundRow">Row containing compound names</param>
        /// <param name="sampleColumn">Column containing sample names</param>
        /// <param name="excelPkg">Excel workbook</param>
        /// <param name="sheetName">Optional: Name of worksheet from which to read data.
        /// Uses first sheet if no name is provided.</param>
        /// <returns>dataMap is an OrderedDictionary with compound name as key and
        ///   value is another Orderedictionary with sample names as key and data as value
        ///   (compound, (sample name, data))</returns>
        public static OrderedDictionary SamplesInColumnsToMap(int sampleRow,
            int compoundColumn, ExcelPackage excelPkg, string sheetName = "_first")
        {
            // <compound, <sample name, data>>
            var dataMap = new OrderedDictionary();

            // Get worksheet - default is first sheet
            var worksheet = excelPkg.Workbook.Worksheets.First();
            // Use sheetName if provided
            if (sheetName != "_first")
                worksheet = excelPkg.Workbook.Worksheets[sheetName];

            int cols = worksheet.Dimension.Columns;
            int rows = worksheet.Dimension.Rows;

            // Read spreadsheet data into a map
            // Data for each compound starts in row after sample ID row
            // Go compound by compound (row by row)
            for (int r = sampleRow + 1; r <= rows; ++r)
            {
                // Compounds in specified column
                var compound = worksheet.Cells[r, compoundColumn]?.Value?.ToString();
                if (compound is null) continue;

                var samplesMap = new OrderedDictionary();

                // Data starts in column after compound name column
                for (int c = compoundColumn + 1; c <= cols; ++c)
                {
                    // Sample name in specified row
                    var sample = worksheet.Cells[sampleRow, c]?.Value?.ToString();
                    if (sample is null) continue;

                    // Check if sample name already exists
                    if (samplesMap.Contains(sample)) 
                        sample = Merge.RenameDuplicate(samplesMap.Keys, sample);

                    var area = worksheet.Cells[r, c]?.Value?.ToString();
                    samplesMap.Add(sample, area);
                }

                if (!dataMap.Contains(compound)) dataMap.Add(compound, samplesMap);
            }

            return dataMap;
        }

    }
}
