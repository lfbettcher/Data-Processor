using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using Accessibility;
using OfficeOpenXml;

namespace WindowsFormCore
{
    class ReadInput
    {
        /// <summary>
        /// Reads MultiQuant exported txt file(s) into excel sheet and returns dataMap.
        /// MultiQuant exported files for Sciex 6500+ have compounds in rows and samples in columns.
        /// There are normally two files, POS and NEG.
        /// </summary>
        /// <param name="filePaths">List of string file paths</param>
        /// <returns>dataMap is a Dictionary with compound name as key and
        ///   value is another dictionary with sample names as key and data as value
        ///   (compound, (sample name, data))</returns>
        public static Dictionary<string, Dictionary<string, string>> ReadMultiQuantTxt(List<string> filePaths)
        {
            // <compound, <sample name, data>>
            var dataMap = new Dictionary<string, Dictionary<string, string>>();

            /* Since text files can only be read row by row, first write the
                data to an excel sheet and then read it into the data map. */

            // Create "Import" tab in data template
            var templateFile = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\DataProcessor\\targeted_300_template-tissue.xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var excelPkg = new ExcelPackage(templateFile);
            var worksheet = excelPkg.Workbook.Worksheets.Add("Import");

            int row = 1, col = 1, namesRow = 1;

            // One file at a time. Descending order makes POS before NEG (doesn't really matter)
            foreach (var filePath in filePaths.OrderByDescending(i => i))
            {
                // Read data from txt file to worksheet
                string[] lines = System.IO.File.ReadAllLines(filePath);

                // First line contains sample names with _POS
                string[] names = lines[0].Split("\t");

                foreach (var name in names)
                {
                    // Write name to cell. Name is the part before the _POS or _NEG
                    worksheet.Cells[namesRow, col++].Value = name.Split('_')[0];
                }
                // Done with first line, increment row and reset column
                ++row;
                col = 1;

                // Do remaining lines
                for (var i = 1; i < lines.Length; ++i)
                {
                    var line = lines[i];
                    string[] words = line.Split("\t");

                    foreach (var word in words)
                    {
                        // Write to cell, increment column after writing
                        worksheet.Cells[row, col++].Value = word;
                    }

                    // Done with line, increment row and reset column
                    ++row;
                    col = 1;
                }

                // Read data into dataMap (compound, (sample name, data))
                var currMap = CompoundsInRowsToMap(excelPkg, "Import", namesRow);
                // Merge map with dataMap
                dataMap = MergeMaps(dataMap, currMap);

                /* At the end of each file, the next file's name row is the next row.
                    This is important to make sure the dataMap isn't affected by
                    mismatched sample order between the two files. */
                namesRow = row;
            }
            excelPkg.SaveAs(new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) +
                                         $"\\DataProcessor\\output.xlsx"));

            //CompoundsInRowsToMap(excelPkg, "Import", namesRow);

            return dataMap;
        }

        public static Dictionary<string, Dictionary<string, string>> MergeMaps(
            Dictionary<string, Dictionary<string, string>> destMap, Dictionary<string, Dictionary<string, string>> srcMap)
        {
            
            foreach (var compound in srcMap.Keys)
            {
                // Copy compounds from srcMap to destMap
                if (!destMap.TryAdd(compound, srcMap[compound]))
                {
                    // If can't copy, compound is already in destMap, merge Dictionary<sample name, data>
                    // If sample name already has data, overwrite data for that sample
                    // *** Test if this actually works with duplicated sample data ***
                    destMap[compound].Union(srcMap[compound]
                            .Where(k => !destMap.ContainsKey(k.Key)))
                            .ToDictionary(k => k.Key, v => v.Value);
                }
            }
            
            return destMap;
        }

        public static void TxtToExcelSheet(string txtFilePath, ExcelPackage excelPkg, string sheetName)
        {

        }

        /// <summary>
        /// Reads data from excel sheet where compound names are in rows (the first column)
        /// and sample IDs are in columns (the first row).
        /// </summary>
        /// <param name="excelPkg">Excel workbook</param>
        /// <param name="sheetName">Name of worksheet from which to read data</param>
        /// <param name="namesRow">Row that contains the sample ID names</param>
        /// <returns>dataMap is a Dictionary with compound name as key and
        ///   value is another dictionary with sample names as key and data as value
        ///   (compound, (sample name, data))</returns>
        public static Dictionary<string, Dictionary<string, string>> CompoundsInRowsToMap(ExcelPackage excelPkg,
                                                                                          string sheetName, int namesRow)
        {
            // <compound, <sample name, data>>
            var dataMap = new Dictionary<string, Dictionary<string, string>>();

            var worksheet = excelPkg.Workbook.Worksheets[sheetName];

            int cols = worksheet.Dimension.Columns;
            int rows = worksheet.Dimension.Rows;

            // Read spreadsheet data into a map
            // Data starts after nameRow
            for (int r = namesRow + 1; r <= rows; r++)
            {
                // Compound in first column
                var compound = worksheet.Cells[r, 1].Value.ToString();
                var samplesMap = new Dictionary<string, string>();

                for (int c = 2; c <= cols; c++)
                {
                    // Sample name in nameRow
                    var sample = worksheet.Cells[namesRow, c].Value.ToString();
                    var area = worksheet.Cells[r, c].Value.ToString();
                    samplesMap.Add(sample, area);
                }
                dataMap.Add(compound, samplesMap);
            }
            return dataMap;
        }

        /// <summary>
        /// Reads data from excel sheet where compound names are in columns (the first row)
        /// and sample IDs are in rows (the first column).
        /// </summary>
        /// <param name="excelPkg">Excel workbook</param>
        /// <param name="sheetName">Name of worksheet from which to read data</param>
        /// <returns>dataMap is a Dictionary with compound name as key and
        ///   value is another dictionary with sample names as key and data as value
        ///   (compound, (sample name, data))</returns>
        public static Dictionary<string, Dictionary<string, string>> CompoundsInColumnsToMap(ExcelPackage excelPkg, string sheetName)
        {
            // <metabolite, <sample name, data>>
            var dataMap = new Dictionary<string, Dictionary<string, string>>();

            // Get worksheet
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var worksheet = excelPkg.Workbook.Worksheets[sheetName];

            int cols = worksheet.Dimension.Columns;
            int rows = worksheet.Dimension.Rows;

            // Read spreadsheet data into a map
            for (int c = 2; c <= cols; ++c)
            {
                // Compounds in first row
                var compound = worksheet.Cells[1, c].Value.ToString();
                var samplesMap = new Dictionary<string, string>();

                for (int r = 2; r <= rows; ++r)
                {
                    // Sample name in first column
                    var sample = worksheet.Cells[r, 1].Value.ToString();
                    var area = worksheet.Cells[r, c].Value.ToString();
                    samplesMap.Add(sample, area);
                }
                dataMap.Add(compound, samplesMap);
            }
            return dataMap;
        }
    }
}
