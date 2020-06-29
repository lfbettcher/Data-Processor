using System;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormCore
{
    class IsotopeCalc
    {
        public Dictionary<string, List<string>> isotopeToCompound;
        public Dictionary<string, string> compoundToIsotope;
        public Dictionary<string, double[]> curveRatios;

        public enum IsotopeSheet {Blank, Peptide, Isotope, C1, C2, C3, C4, C5, C6}

        public IsotopeCalc()
        {
            this.isotopeToCompound = new Dictionary<string, List<string>>();
            this.compoundToIsotope = new Dictionary<string, string>();
            this.curveRatios = new Dictionary<string, double[]>();
        }

        /// <summary>
        /// Puts data into two maps: isotopeToCompound and compoundToIsotope.
        /// </summary>
        /// <param name="filePath">File with isotope data</param>
        /// <returns>isotopeToCompound map</returns>
        public Dictionary<string, List<string>> IsotopeMap(string filePath)
        {
            var progressWindow = (ProgressWindow)Application.OpenForms["Form2"];
            progressWindow.progressTextBox.SetText("Importing isotope information");

            // Get isotope match worksheet
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var excelPkg = new ExcelPackage(new FileInfo(filePath));
            var worksheet = excelPkg.Workbook.Worksheets["Isotope"];

            int cols = worksheet.Dimension.Columns;
            int rows = worksheet.Dimension.Rows;

            // Get column index of compound and isotope
            /*
            int compoundCol = -1, isotopeCol = -1;
            for (int c = 1; c <= cols; c++)
            {
                if (string.Equals(worksheet.Cells[1, c].Value.ToString(), "Peptide")) compoundCol = c;
                if (string.Equals(worksheet.Cells[1, c].Value.ToString(), "Isotope")) isotopeCol = c;
            }

            // ERROR: Columns not found
            if (compoundCol == -1 || isotopeCol == -1)
            {
                progressWindow.progressTextBox.AppendLine("ERROR: Columns not found");
                return isotopeMap;
            }
            */
            int compoundCol = (int) IsotopeSheet.Peptide;
            int isotopeCol = (int) IsotopeSheet.Isotope;

            // Read isotope matches into a map
            for (int r = 2; r <= rows; r++)
            {
                var compound = worksheet.Cells[r, compoundCol].Value.ToString();
                var isotope = worksheet.Cells[r, isotopeCol].Value.ToString();

                if (isotopeToCompound.ContainsKey(isotope)) isotopeToCompound[isotope].Add(compound);
                else isotopeToCompound.Add(isotope, new List<string>() {compound});

                compoundToIsotope.Add(compound, isotope);

                // Save curve ratios to curveRatios map
                double[] ratios = new double[6];
                for (int c = 1; c <= 6; c++)
                {
                    ratios[c - 1] = double.Parse(worksheet.Cells[r, c + 2].Value.ToString());
                }
                curveRatios.Add(compound, ratios);
            }

            return isotopeToCompound;
        }


        public enum Conc {A, B, C1, C2, C3, C4, C5, C6}
        public enum CalcSheet {blank, sample, weight, concStandard, ratio, x, y, xy, xx, n, slope, intercept}

        /// <summary>
        /// Calculates the slope and intercept for a compound curve
        /// </summary>
        /// <param name="filePath">File with worksheet that calculates curve</param>
        /// <param name="compound">Compound to get the curve of</param>
        /// <returns>slope and intercept in a double[]</returns>
        public double[] SlopeIntercept(string filePath, string compound)
        {
            double[] slopeIntercept = new double[2];

            var progressWindow = (ProgressWindow)Application.OpenForms["Form2"];
            progressWindow.progressTextBox.AppendLine($"Calculating slope and intercept for {compound}");

            // Get isotope calculation worksheet
            //var excelPkg = new ExcelPackage(new FileInfo(filePath));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            string filePath_ = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\calculation.xlsx"; // for testing
            var excelPkg = new ExcelPackage(new FileInfo(filePath_));
            var worksheet = excelPkg.Workbook.Worksheets[compoundToIsotope[compound]];
            worksheet.Cells.Style.Numberformat.Format = "0.000000";
            // Put ratio data into Ratio column
            for (int c = 1; c <= 6; c++)
            {
                //worksheet.Cells[c + 1, 4].Value = curveRatios[compound][c - 1];
                worksheet.Cells[c + 1, 4].Value = curveRatios[compound].GetValue(c - 1);
            }

            var slope = worksheet.Cells[2, (int) CalcSheet.slope].Value.ToString();
            var intercept = worksheet.Cells[2, (int) CalcSheet.intercept].Value.ToString();

            slopeIntercept[0] = double.Parse(slope);
            slopeIntercept[1] = double.Parse(intercept);

            SaveFile(excelPkg, "calculation");
            return slopeIntercept;
        }

        /* SAVE FILE */
        public static void SaveFile(ExcelPackage excelPkg, string filename)
        {
            excelPkg.SaveAs(new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{filename}.xlsx"));
        }
    }
}