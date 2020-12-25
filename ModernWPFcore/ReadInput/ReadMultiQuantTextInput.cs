using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ModernWPFcore.Pages;

namespace ModernWPFcore
{
    class ReadMultiQuantTextInput
    {

        public static OrderedDictionary ReadMultiQuantText(
            List<string> filePaths, Dictionary<string, string> options, ProgressPage progressPage)
        {
            // <compound, <sample name, data>>
            var dataMap = new OrderedDictionary();

            /* Since text files can only be read row by row, first write the
                data to an excel sheet and then read it into the data map.
            This cross references the sample ID and compound name when storing the data. */

            // Create "Import" tab in data template
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var templateFile = new FileInfo(options["TemplatePath"]);
            var excelPkg = new ExcelPackage(templateFile);
            var worksheet = excelPkg.Workbook.Worksheets.Add("Import");

            int row = 1, col = 1, namesRow = 1, fileCount = 1;

            // One file at a time. Descending order makes POS before NEG
            foreach (var filePath in filePaths.OrderByDescending(i => i))
            {
                progressPage.ProgressTextBox.AppendText($"Reading file {fileCount++}\n");

                // Read data from txt file to worksheet
                string[] lines = File.ReadAllLines(filePath);

                /* If samples in columns (MultiQuant export "Transposed" checked),
                     first line contains sample names with _POS or _NEG. */
                /* If samples in rows (MultiQuant export "Transposed" not checked),
                     first line contains compound names. */
                string[] names = lines[0].Split("\t");

                foreach (var name in names)
                {
                    // Write name to column
                    // Remove _POS or _NEG if name is sample name
                    if (options["SamplesIn"] == "columns")
                    {
                        if (name.ContainsIgnoreCase("POS_"))
                            worksheet.Cells[namesRow, col++].Value = Regex.Split(name, "POS_", RegexOptions.IgnoreCase)[1];
                        else if (name.ContainsIgnoreCase("POS_"))
                            worksheet.Cells[namesRow, col++].Value = Regex.Split(name, "_POS", RegexOptions.IgnoreCase)[0];
                        //if (name.Contains("_POS"))
                        //    worksheet.Cells[namesRow, col++].Value = name.Split("_POS")[0];
                        //if (name.Contains("POS_"))
                        //    worksheet.Cells[namesRow, col++].Value = name.Split("_POS")[0];
                        //else if (name.Contains("_NEG"))
                        //    worksheet.Cells[namesRow, col++].Value = name.Split("_NEG")[0];
                        else worksheet.Cells[namesRow, col++].Value = name;
                    }
                    else
                    {
                        worksheet.Cells[namesRow, col++].Value = name;
                    }
                }

                // Done with first line, increment row and reset column
                ++row;
                col = 1;

                // Write remaining lines
                for (var i = 1; i < lines.Length; ++i)
                {
                    var line = lines[i];
                    string[] words = line.Split("\t");

                    // If samples in rows, first word is sample ID, remove _POS or _NEG
                    if (options["SamplesIn"] == "rows")
                    {
                        if (words[0].Contains("_POS"))
                            words[0] = words[0].Split("_POS")[0];
                        else if (words[0].Contains("_NEG"))
                            words[0] = words[0].Split("_NEG")[0];
                    }

                    // Write to cell, increment column after writing
                    foreach (var word in words)
                    {
                        worksheet.Cells[row, col++].Value = word;
                    }

                    // Done with line, increment row and reset column
                    ++row;
                    col = 1;
                }

                var curMap = new OrderedDictionary();

                // Read data into current map <compound, <sample name, data>
                curMap = options["SamplesIn"] == "rows"
                    ? ExcelToMap.SamplesInRowsToMap(namesRow, 1, excelPkg, "Import")
                    : ExcelToMap.SamplesInColumnsToMap(namesRow, 1, excelPkg, "Import");

                // Merge current map with dataMap
                dataMap = Merge.MergeMaps(dataMap, curMap);

                /* At the end of each file, the next file's name row is the next row.
                    This is important to make sure the dataMap isn't affected by
                    mismatched sample order between the two files. */
                namesRow = row;
            }

            excelPkg.SaveAs(new FileInfo(options["OutputPath"] + "\\" + "import_" + options["OutputFileName"]));

            progressPage.ProgressTextBox.AppendText("All input files have been read.\n");

            return dataMap;
        }

    }

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static bool ContainsIgnoreCase(this string source, string toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }

}
