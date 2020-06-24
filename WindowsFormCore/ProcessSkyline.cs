using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using System.Data;
using System.Runtime.CompilerServices;

namespace WindowsFormCore
{
    class ProcessSkyline
    {
        public static DataTable ToDataTable(ExcelPackage package)
        {
            ExcelWorksheet sheet = package.Workbook.Worksheets.First();
            DataTable dataTable = new DataTable();
            int rows = sheet.Dimension.End.Row;
            int cols = sheet.Dimension.End.Column;
            foreach (var firstRowCell in sheet.Cells[1,1,1,sheet.Dimension.End.Column])
            {
                dataTable.Columns.Add(firstRowCell.Text);
            }
            for (int rowNumber = 2; rowNumber <= rows; rowNumber++)
            {
                var row = sheet.Cells[rowNumber, 1, rowNumber, cols];
                var newRow = dataTable.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                dataTable.Rows.Add(newRow);
            }
            return dataTable;
        }



        public static void run(string filePath)
        {
            FileInfo inputFile = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage xl = new ExcelPackage(inputFile);
            ToDataTable(xl);
            /*
            using (ExcelPackage xl = new ExcelPackage(inputFile))
            {
                // Get first worksheet
                ExcelWorksheet worksheet = xl.Workbook.Worksheets.FirstOrDefault();

                int totalRows = worksheet.Dimension.Rows;
                string peptideColContent = string.Empty;
                string replicateColContent = string.Empty;
                string areaColContent = string.Empty;

                for (int i = 1; i <= totalRows; i++)
                {
                    // Get all content from peptide, replicate, area columns
                    peptideColContent += worksheet.Cells[i, 1].Value.ToString();
                    replicateColContent += worksheet.Cells[i, 3].Value.ToString();
                    areaColContent += worksheet.Cells[i, 11].Value.ToString();

                }
                System.Diagnostics.Debug.WriteLine($"Peptide data {peptideColContent}");
                System.Diagnostics.Debug.WriteLine($"Replicate data {replicateColContent}");
                System.Diagnostics.Debug.WriteLine($"Area data {areaColContent}");
                System.Diagnostics.Debug.WriteLine("Console WriteLine");
            */

            
        }
    }
}