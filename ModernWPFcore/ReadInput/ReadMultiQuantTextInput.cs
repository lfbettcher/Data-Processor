using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ModernWPFcore.Pages;

namespace ModernWPFcore.ReadInput
{
    class ReadMultiQuantTextInput
    {

        /// <summary>
        /// Reads MultiQuant exported txt file(s) into excel sheet and returns dataMap.
        /// There are normally two files, POS and NEG.
        /// </summary>
        /// <param name="filePaths">List of string file paths</param>
        /// <param name="options">Dictionary of options from form</param>
        /// <returns>dataMap is a Dictionary with compound name as key and
        ///   value is another dictionary with sample names as key and data as value
        ///   (compound, (sample name, data))</returns>
        public static Dictionary<string, Dictionary<string, string>> 
            ReadMultiQuantText(List<string> filePaths, Dictionary<string, string> options, ProgressPage progressPage)
        {
            // <compound, <sample name, data>>
            var dataMap = new Dictionary<string, Dictionary<string, string>>();

            /* Since text files can only be read row by row, first write the
                data to an excel sheet and then read it into the data map. */

            // Create "Import" tab in data template
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var templateFile = new FileInfo(options["TemplatePath"]);
            var excelPkg = new ExcelPackage(templateFile);
            var worksheet = excelPkg.Workbook.Worksheets.Add("Import");

            int row = 1, col = 1, namesRow = 1;

            // One file at a time. Descending order makes POS before NEG (doesn't really matter)
            foreach (var filePath in filePaths.OrderByDescending(i => i))
            {
                progressPage.ProgressTextBox.AppendText("More progress\n");
                // Read data from txt file to worksheet
                string[] lines = File.ReadAllLines(filePath);

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
                //var currMap = CompoundsInRowsToMap(excelPkg, "Import", namesRow);
                // Merge map with dataMap
                //dataMap = MergeMaps(dataMap, currMap);

                /* At the end of each file, the next file's name row is the next row.
                    This is important to make sure the dataMap isn't affected by
                    mismatched sample order between the two files. */
                namesRow = row;
            }
            excelPkg.SaveAs(new FileInfo(options["OutputPath"] + "\\" +
                                         options["OutputFileName"]));

            progressPage.ProgressTextBox.AppendText("Done reading file\n");

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
    }

}
