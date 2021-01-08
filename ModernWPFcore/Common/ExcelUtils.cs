using System;
using System.Text.RegularExpressions;
using OfficeOpenXml;

namespace ModernWPFcore
{
    public static class ExcelUtils
    {
        /// <summary>
        /// Safely converts string row number to int. Defaults to 1 if unsuccessful.
        /// Row number input text box is a string.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static int GetRowNum(string row)
        {
            return int.TryParse(row, out var rowNum) ? rowNum : 1;
        }

        /// <summary>
        /// Safely converts string column letter or number to int.
        /// Column number input text box is a string and can be either letters or numbers.
        /// </summary>
        /// <param name="col">Column can be a letter or number</param>
        /// <returns>Column number as int, or 1 if input could not be converted
        /// to a valid column number</returns>
        public static int GetColNum(string col)
        {
            // Parse number or convert letter
            return int.TryParse(col, out var colNum) ? colNum : ColumnNameToNumber(col);
        }

        /// <summary>
        /// Gets the dimensions of a portion of the worksheet.
        /// Dimensions are in the format "A1:E5". In that example, it would be
        /// the 5x5 cell region from column A to E and row 1 to 5.
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="startCell"></param>
        /// <returns></returns>
        public static string GetDimensions(ExcelWorksheet worksheet, string startCell = "none")
        {
            var startRow = 1;
            var startCol = 1;
            var endRow = 1;
            var endCol = 1;

            if ("none".Equals(startCell))
            {
                // Explore the first 5 columns and rows to find the maximum dimensions
                for (var i = 1; i <= 5; ++i)
                {
                    endRow = Math.Max(endRow, RowsInColumn(worksheet, i));
                    endCol = Math.Max(endCol, ColumnsInRow(worksheet, i));
                }
            }
            else
            {
                (startRow, startCol) = GetRowCol(startCell);
                endRow = RowsInColumn(worksheet, startCol);
                endCol = ColumnsInRow(worksheet, startRow);
            }

            var startAddress = GetAddress(startRow, startCol);
            var endAddress = GetAddress(endRow, endCol);

            return startAddress + ":" + endAddress;
        }

        /// <summary>
        /// Converts column name, such as AZ, to a number.
        /// ref https://stackoverflow.com/a/667902
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int ColumnNameToNumber(string columnName)
        {
            columnName = columnName.ToUpper();
            int sum = 0;

            foreach (char c in columnName)
            {
                sum *= 26;
                sum += (c - 'A' + 1);
            }

            return sum;
        }

        /// <summary>
        /// Converts column number to name, such as AZ.
        /// ref https://stackoverflow.com/a/182924
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public static string ColumnNumberToName(int columnNumber)
        {
            var columnName = string.Empty;
            int modulo;

            while (columnNumber > 0)
            {
                modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return columnName;
        }

        /// <summary>
        /// Returns the number of filled rows in a specified column.
        /// Requires 2 consecutive empty cells to stop counting
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int RowsInColumn(ExcelWorksheet sheet, int col)
        {
            if (sheet.Dimension == null) return 0;

            var rows = 1;
            string cellText;
            bool cellEmpty;

            do
            { 
                cellText = sheet.Cells[rows, col].Text;
                cellEmpty = string.IsNullOrWhiteSpace(cellText);
                if (cellEmpty)
                {
                    bool nextCellEmpty = string.IsNullOrWhiteSpace(sheet.Cells[rows + 1, col].Text);
                    if (nextCellEmpty) return rows - 1;
                    cellEmpty = false;
                }
                rows++;
            } while (!cellEmpty);

            return rows - 1;
        }

        /// <summary>
        /// Returns the number of filled rows in a specified column
        /// Requires 2 consecutive empty cells to stop counting
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static int ColumnsInRow(ExcelWorksheet sheet, int row)
        {
            if (sheet.Dimension == null) return 0;

            var cols = 1;
            string cellText;
            bool cellEmpty;

            do
            {
                cellText = sheet.Cells[row, cols].Text;
                cellEmpty = string.IsNullOrWhiteSpace(cellText);
                if (cellEmpty)
                {
                    bool nextCellEmpty = string.IsNullOrWhiteSpace(sheet.Cells[row, cols + 1].Text);
                    if (nextCellEmpty) return cols - 1;
                    cellEmpty = false;
                }
                cols++;
            } while (!cellEmpty);

            return cols - 1;
        }

        /// <summary>
        /// Returns row and column number from cell address
        /// Cell address AQ389 returns row 389, col 43
        /// </summary>
        /// <param name="cellAddress"></param>
        /// <returns></returns>
        public static (int row, int col) GetRowCol(string cellAddress)
        {
            // Split letter and number portion into group 1 and group 2
            var match = Regex.Match(cellAddress, @"([A-Za-z]+)(\d+)");
            string colName = match.Groups[1].Value;
            int col = ColumnNameToNumber(colName);
            int.TryParse(match.Groups[2].Value, out var row);
            return (row, col);
        }

        /// <summary>
        /// Returns cell address for row and column number
        /// Row 389, col 43 returns cell address AQ389
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string GetAddress(int row, int col)
        {
            string colName = ColumnNumberToName(col);
            return colName + row;
        }

        public static ExcelPackage FormatCells(ExcelPackage excelPkg, string sheetName, string format,
            string startCell = "B2", string endCell = "none")
        {
            var worksheet = excelPkg.Workbook.Worksheets[sheetName];

            // TODO - use startCell and endCell

            worksheet.Cells.Style.Numberformat.Format = format;
            worksheet.Cells.AutoFitColumns();
            excelPkg.Save();
            return excelPkg;
        }
    }
}
