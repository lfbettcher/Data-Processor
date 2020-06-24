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
                Dictionary<string, List<KeyValuePair<string, string>>> dataMap = 
                    new Dictionary<string, List<KeyValuePair<string, string>>>();

                for (int i = 1; i <= totalRows; i++)
                {
                    string compound = worksheet.Cells[i, 1].Value.ToString();
                    string sample = worksheet.Cells[i, 3].Value.ToString();
                    string area = worksheet.Cells[i, 10].Value.ToString();

                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>(sample, area);

                    if (dataMap.ContainsKey(compound))
                    {
                        List<KeyValuePair<string, string>> pairList = dataMap[compound];
                        pairList.Add(pair);
                        dataMap[compound] = pairList;
                    }
                    else
                    {
                        List<KeyValuePair<string, string>> pairList = new List<KeyValuePair<string, string>>();
                        pairList.Add(pair);
                        dataMap.Add(compound, pairList);
                    }
                }

                int numCompounds = dataMap.Count - 1;
                System.Diagnostics.Debug.WriteLine(numCompounds);

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
