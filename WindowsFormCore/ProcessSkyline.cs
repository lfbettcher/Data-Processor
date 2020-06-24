using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;

namespace WindowsFormCore
{
    class ProcessSkyline
    {
        public static void run(string filePath)
        {
            FileInfo inputFile = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage xl = new ExcelPackage(inputFile))
            {
                // Get first worksheet
                ExcelWorksheet sheet = xl.Workbook.Worksheets[0];

                int col = 2; // Column 2 is the ?
                for (int row = 2; row < 5; row++)
                {
                    Console.WriteLine("\tCell({0},{1}).Value={2}", row, col, sheet.Cells[row, col].Value);
                }
            Console.WriteLine("done");
            }
        }
    }
}
