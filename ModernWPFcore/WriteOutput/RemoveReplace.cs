using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OfficeOpenXml;

namespace ModernWPFcore.WriteOutput
{
    class RemoveReplace
    {
        public static ExcelPackage RemoveSamples(ExcelPackage excelPkg, string sheetName, Dictionary<string, string> options)
        {
            var worksheet = excelPkg.Workbook.Worksheets[sheetName];
            string[] removeNames = options["RemoveNames"].Split(",")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            if (options["SamplesIn"] == "rows")
            {
                var sampleCol = ExcelUtils.GetColNum(options["SampleLoc"]);
                var numRows = ExcelUtils.RowsInColumn(worksheet, sampleCol);

                for (var r = 1; r <= numRows; ++r)
                {
                    var sampleName = worksheet.Cells[r, sampleCol]?.Value?.ToString();
                    if (sampleName is null) continue;

                    foreach (var name in removeNames)
                    {
                        if (sampleName.Contains(name, StringComparison.OrdinalIgnoreCase) && 
                            !sampleName.Contains("prep", StringComparison.OrdinalIgnoreCase)) 
                            worksheet.DeleteRow(r--);
                    }
                }
            }
            return excelPkg;
        }

        public static ExcelPackage RemoveCompounds(ExcelPackage excelPkg)
        {
            return excelPkg;
        }

        public static ExcelPackage ReplaceMissing(ExcelPackage excelPkg, string sheetName, string replacement)
        {
            var worksheet = excelPkg.Workbook.Worksheets[sheetName];
            var range = ExcelUtils.GetDimensions(worksheet);
            foreach (var cell in worksheet.Cells[range])
            {
                var value = cell?.Value?.ToString();
                if (cell != null && string.IsNullOrWhiteSpace(value))
                    cell.Value = replacement;
            }

            return excelPkg;
        }

    }
}
