using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ModernWPFcore.Pages;

namespace ModernWPFcore
{
    class ReadMultiQuantTextInput
    {

        public static Dictionary<string, Dictionary<string, string>>
            ReadMultiQuantText(List<string> filePaths, Dictionary<string, string> options, ProgressPage progressPage)
        {
            // <compound, <sample name, data>>
            var dataMap = new Dictionary<string, Dictionary<string, string>>();

            /* Since text files can only be read row by row, first write the
                data to an excel sheet and then read it into the data map.
            This cross references the sample ID and compound name when storing the data. */

            // Create "Import" tab in data template
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var templateFile = new FileInfo(options["TemplatePath"]);
            var excelPkg = new ExcelPackage(templateFile);
            var worksheet = excelPkg.Workbook.Worksheets.Add("Import");

            int row = 1, col = 1, namesRow = 1, fileCount = 1;

            // One file at a time. Descending order makes POS before NEG (doesn't really matter)
            foreach (var filePath in filePaths.OrderByDescending(i => i))
            {
                progressPage.ProgressTextBox.AppendText($"Reading file {fileCount++}\n");

                // Read data from txt file to worksheet
                string[] lines = File.ReadAllLines(filePath);

                /* If samples in columns (MultiQuant export "Transposed" checked),
                     first line contains sample names with _POS. */
                /* If samples in rows (MultiQuant export "Transposed" not checked),
                     first line contains compound names. */
                string[] names = lines[0].Split("\t");

                foreach (var name in names)
                {
                    if (options["SamplesIn"] == "columns")
                    {
                        // Write sample name to cell. Name is the part before the _POS or _NEG
                        worksheet.Cells[namesRow, col++].Value = name.Split('_')[0];
                    }
                    else
                    {
                        // Write compound name to cell.
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
                        words[0] = words[0].Split('_')[0];

                    // Write to cell, increment column after writing
                    foreach (var word in words)
                    {
                        worksheet.Cells[row, col++].Value = word;
                    }

                    // Done with line, increment row and reset column
                    ++row;
                    col = 1;
                }

                var curMap = new Dictionary<string, Dictionary<string, string>>();

                // Read data into current map <compound, <sample name, data>
                curMap = options["SamplesIn"] == "rows"
                    ? ExcelToMap.SamplesInRowsToMap(namesRow, 1, excelPkg, "Import")
                    : ExcelToMap.SamplesInColumnsToMap(namesRow, 1, excelPkg, "Import");

                // Merge current map with dataMap
                dataMap = MergeMaps.MergeMapsReplaceDuplicates(dataMap, curMap);

                /* At the end of each file, the next file's name row is the next row.
                    This is important to make sure the dataMap isn't affected by
                    mismatched sample order between the two files. */
                namesRow = row;
            }

            excelPkg.SaveAs(new FileInfo(options["OutputPath"] + "\\" + options["OutputFileName"]));

            progressPage.ProgressTextBox.AppendText("All input files have been read.\n");

            return dataMap;
        }

    }

}
