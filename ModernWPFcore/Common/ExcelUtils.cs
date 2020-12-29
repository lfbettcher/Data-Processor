using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OfficeOpenXml;

namespace ModernWPFcore
{
    class ExcelUtils
    {
        // Converts column name, such as AZ, to a number
        // ref https://stackoverflow.com/a/667902
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

        // Converts column number to name, such as AZ
        // ref https://stackoverflow.com/a/182924
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

        // Returns the number of filled rows in a specified column
        // Requires 2 consecutive empty cells to stop counting
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
                    if (nextCellEmpty) return rows;
                    cellEmpty = false;
                }
                rows++;
            } while (!cellEmpty);

            return rows;
        }

        // Returns the number of filled rows in a specified column
        // Requires 2 consecutive empty cells to stop counting
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
                    if (nextCellEmpty) return cols;
                    cellEmpty = false;
                }
                cols++;
            } while (!cellEmpty);

            return cols;
        }

        // Returns row and column number from cell name
        // Cell name AQ389 would return row 389 and col 43
        public static (int row, int col) GetRowCol(string cellName)
        {
            // Split letter and number portion into group 1 and group 2
            var match = Regex.Match(cellName, @"([A-Za-z]+)(\d+)");
            string colName = match.Groups[1].Value;
            int col = ColumnNameToNumber(colName);
            int.TryParse(match.Groups[2].Value, out var row);
            return (row, col);
        }
    }
}
