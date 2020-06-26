using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormCore
{
    class ProcessSkyline
    {
        public int COL_CMPD;
        public int COL_SAMPLE;
        public int COL_AREA;

        public static void run(string filePath)
        {
            ProgressWindow form2 = (ProgressWindow)Application.OpenForms["Form2"];
            form2.progressTextBox.SetText("Opening file...");

            FileInfo inputFile = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage ExcelPkg = new ExcelPackage(inputFile))
            {
                // Get first worksheet
                ExcelWorksheet worksheet = ExcelPkg.Workbook.Worksheets.FirstOrDefault();
                int totalRows = worksheet.Dimension.Rows;

                // Read spreadsheet data into a map
                Dictionary<string, List<KeyValuePair<string, string>>> dataMap = 
                    new Dictionary<string, List<KeyValuePair<string, string>>>();

                for (int i = 1; i <= totalRows; i++)
                {
                    string compound = worksheet.Cells[i, 1].Value.ToString();
                    string sample = worksheet.Cells[i, 3].Value.ToString();
                    string area = worksheet.Cells[i, 10].Value.ToString();

                    KeyValuePair<string, string> pair = 
                        new KeyValuePair<string, string>(sample, area);

                    if (dataMap.ContainsKey(compound))
                    {
                        List<KeyValuePair<string, string>> pairList = dataMap[compound];
                        pairList.Add(pair);
                        dataMap[compound] = pairList;
                    }
                    else
                    {
                        List<KeyValuePair<string, string>> pairList = 
                            new List<KeyValuePair<string, string>>() { pair };
                        dataMap.Add(compound, pairList);
                    }
                }

                form2.progressTextBox.AppendLine("Finished reading data.\r\nWriting data...");

                List<string> compoundList = new List<string>(dataMap.Keys);
                int numCompounds = dataMap.Count - 1;
                int numSamples = dataMap[compoundList[1]].Count;
                System.Diagnostics.Debug.WriteLine(numCompounds);
                System.Diagnostics.Debug.WriteLine(numSamples);
                System.Diagnostics.Debug.WriteLine(compoundList[1]);

                // Write data to new sheet
                ExcelWorksheet outputSheet = ExcelPkg.Workbook.Worksheets.Add("Formatted Data");

                // Populate first row with compounds
                outputSheet.Cells[1, 1].Value = "Sample";
                for (int col = 2; col <= numCompounds + 1; col++)
                {
                    outputSheet.Cells[1, col].Value = compoundList[col - 1];
                }

                // Write each sample (row)
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

                            if (double.TryParse(peakArea, out double peakAreaNum))
                            {
                                outputSheet.Cells[i, j].Value = peakAreaNum;
                            }
                            else
                            {
                                outputSheet.Cells[i, j].Value = peakArea;
                            }
                        }
                    }
                }
                ExcelRange range = outputSheet.Cells[2, 2, numSamples + 1, numCompounds + 1];
                range.Style.Numberformat.Format = "0";

                ExcelPkg.SaveAs(new FileInfo("C:\\Users\\Lisa\\OneDrive\\Desktop\\out.xlsx"));

                // Make copy of sheet and remove #NA
                ExcelWorksheet detectedSheet = Copy(ExcelPkg, "Formatted Data", "Compounds Detected");

                for (int col = 2; col <= numCompounds + 1; col++)
                {
                    // Count of samples the compound is detected in
                    var colLetter = GetColumnLetter(col);
                    detectedSheet.Cells[numSamples + 2, col].Formula = 
                        $"=COUNT({colLetter}2:{colLetter}{numSamples + 1})";
                }

                ExcelPkg.SaveAs(new FileInfo("C:\\Users\\Lisa\\OneDrive\\Desktop\\out.xlsx"));

                form2.progressTextBox.AppendLine("Done");
                form2.UseWaitCursor = false;
            }
        }

        /* COPY EXCEL SHEET */
        public static ExcelWorksheet Copy(ExcelPackage excelPackage, string copyFrom, string copyTo)
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Copy(copyFrom, copyTo);
            return worksheet;
        }

        /* GET COLUMN LETTER */
        public static string GetColumnLetter(int columnNumber)
        {
            int dividend = columnNumber;
            string columnLetter = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnLetter = Convert.ToChar(65 + modulo).ToString() + columnLetter;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnLetter;
        }
    }
}
