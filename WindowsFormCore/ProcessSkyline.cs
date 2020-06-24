using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Linq;

namespace WindowsFormCore
{
    class ProcessSkyline
    {
        //public int NUM_CPNDS = 59;
        //public int NUM_SAMPLES = 150;

        public static void run(string filePath)
        {
            FileInfo inputFile = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage xl = new ExcelPackage(inputFile))
            {
                // Get first worksheet
                ExcelWorksheet worksheet = xl.Workbook.Worksheets.FirstOrDefault();
                int totalRows = worksheet.Dimension.Rows;

                // Get set of compounds
                HashSet<string> compounds = new HashSet<string>();
                
                for (int i = 1; i <= totalRows; i++)
                {
                    compounds.Add(worksheet.Cells[i, 1].Value.ToString());
                }

                int numCompounds = compounds.Count - 1;
                //System.Diagnostics.Debug.WriteLine(numCompounds);

                /*
                string peptideColContent = string.Empty;
                string replicateColContent = string.Empty;
                string areaColContent = string.Empty;
                
                for (int i = 1; i <= totalRows; i++)
                {
                    // Get all content from peptide, replicate, area columns
                    peptideColContent += worksheet.Cells[i, 1].Value.ToString();
                    replicateColContent += worksheet.Cells[i, 3].Value.ToString();
                    areaColContent += worksheet.Cells[i, 11].Value.ToString();

                }*/
            }
               
        }
    }
}
