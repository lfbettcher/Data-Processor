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

            using (ExcelPackage ExcelPkg = new ExcelPackage(inputFile))
            {
                // Get first worksheet
                ExcelWorksheet worksheet = ExcelPkg.Workbook.Worksheets.FirstOrDefault();
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

                List<string> compoundList = new List<string>(dataMap.Keys);
                int numCompounds = dataMap.Count - 1;
                int numSamples = dataMap[compoundList[1]].Count;
                System.Diagnostics.Debug.WriteLine(numCompounds);
                System.Diagnostics.Debug.WriteLine(numSamples);
                System.Diagnostics.Debug.WriteLine(compoundList[1]);

                // Write data to new sheet
                ExcelWorksheet outputSheet = ExcelPkg.Workbook.Worksheets.Add("Formatted Data");

                //ExcelRange areaRange = outputSheet.Cells[2, 2, numSamples + 1, numCompounds + 1];
                //areaRange.Style.Numberformat.Format = "0";

                // Populate first row with compounds
                outputSheet.Cells[1, 1].Value = "Sample";
                for (int col = 2; col <= numCompounds + 1; col++)
                {
                    outputSheet.Cells[1, col].Value = compoundList[col - 1];
                }

                // For each sample (row)
                for (int i = 2; i <= numSamples + 1; i++)
                {
                    // Write sample number in first column
                    outputSheet.Cells[i, 1].Value = i - 1;
                    for (int j = 2; j <= numCompounds + 1; j++)
                    {
                        // Check if sample name in first column matches Key and compound name in first row matches Value
                        if (string.Equals(outputSheet.Cells[i, 1].Value.ToString(), dataMap[compoundList[j - 1]][i - 2].Key) &&
                            string.Equals(outputSheet.Cells[1, j].Value, compoundList[j - 1]))
                        {
                            // Write peak area
                            var peakArea = dataMap[compoundList[j - 1]][i - 2].Value;
                            var peakAreaNum = 0.00;

                            if (double.TryParse(peakArea, out peakAreaNum))
                            {
                                outputSheet.Cells[i, j].Value = peakAreaNum;
                            }
                            else
                            {
                                outputSheet.Cells[i, j].Value = peakArea;
                            }                            
                        }
                        //System.Diagnostics.Debug.WriteLine(outputSheet.Cells[i, 1].Value.ToString());
                        //System.Diagnostics.Debug.WriteLine(dataMap[compoundList[j - 1]][i - 2].Key);
                        //System.Diagnostics.Debug.WriteLine("");


                    }

                }

                //ExcelRange range = outputSheet.Cells[1, 1, numSamples + 1, numCompounds + 1];
                //range.Style.Numberformat.Format = "0";


                ExcelPkg.SaveAs(new FileInfo("C:\\Users\\Lisa\\OneDrive\\Desktop\\out.xlsx"));
                //}

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
