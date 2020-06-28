using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormCore
{
    internal class IsotopeCalc
    {
        public static Dictionary<string, List<string>> IsotopeMap(string filePath)
        {
            var progressWindow = (ProgressWindow)Application.OpenForms["Form2"];
            progressWindow.progressTextBox.SetText("Importing isotope information");

            var isotopeMap = new Dictionary<string, List<string>>();

            // Get isotope match worksheet
            var inputFile = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license
            var excelPkg = new ExcelPackage(inputFile);
            var worksheet = excelPkg.Workbook.Worksheets["Isotope"];

            int cols = worksheet.Dimension.Columns;
            int rows = worksheet.Dimension.Rows;

            // Get column index of compound, sample name, and area
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

            // Read isotope matches into a map
            for (int i = 2; i <= rows; i++)
            {
                var compound = worksheet.Cells[i, compoundCol].Value.ToString();
                var isotope = worksheet.Cells[i, isotopeCol].Value.ToString();

                if (isotopeMap.ContainsKey(isotope)) isotopeMap[isotope].Add(compound);
                else isotopeMap.Add(isotope, new List<string>() {compound});
            }

            return isotopeMap;
        }
    }
}