using OfficeOpenXml;

namespace ModernWPFcore
{
    class MissingValue
    {
        public static ExcelPackage Run(ExcelPackage excelPkg)
        {
            return excelPkg;
        }

        /// <summary>
        /// Replaces missing values with the specified replacement.
        /// TODO - broaden criteria for what is "missing".
        /// TODO Currently it looks for the "!" character which is in " #DIV/0!" and "#VALUE!"
        /// </summary>
        /// <param name="excelPkg"></param>
        /// <param name="tabName"></param>
        /// <param name="replacement"></param>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        /// <returns></returns>
        public static ExcelPackage ReplaceMissing(ExcelPackage excelPkg, string tabName, 
            string replacement, string startCell = "A1", string endCell = "none")
        {
            var worksheet = excelPkg.Workbook.Worksheets[tabName];
            string dimensions;

            if ("none".Equals(endCell)) 
                dimensions = ExcelUtils.GetDimensions(worksheet, startCell);
            else
                dimensions = startCell + ":" + endCell;

            foreach (var cell in worksheet.Cells[dimensions])
            {
                if (cell.Value?.ToString()?.Contains("!") == true)
                    cell.Value = replacement;
            }

            excelPkg.Save();
            return excelPkg;
        }

        public static ExcelWorksheet RemoveMissing(ExcelWorksheet worksheet, float percentMissing)
        {
            return worksheet;
        }

        public static bool IsMissing(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;
            return !double.TryParse(value, out _);
        }

    }
}
