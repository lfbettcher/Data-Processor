using System.Collections.Generic;
using System.Collections.Specialized;
using OfficeOpenXml;

namespace ModernWPFcore
{
    class AbsoluteQuant
    {
        /// <summary>
        /// Absolute Quant Calc with Sciex 6500 template 
        /// </summary>
        /// <param name="excelPkg"></param>
        /// <param name="options"></param>
        /// <param name="compoundLoc"></param>
        /// <returns></returns>
        public static ExcelPackage Sciex6500Template(
            ExcelPackage excelPkg, Dictionary<string, string> options, int compoundLoc)
        {
            var calcSheet = excelPkg.Workbook.Worksheets[options["AbsoluteQuantTabName"]];
            var writeSheet = excelPkg.Workbook.Worksheets["Absolute Quant Data"];

            OrderedDictionary concMap = new OrderedDictionary();

            // Get approximate bounds
            var rows = ExcelUtils.RowsInColumn(calcSheet, compoundLoc);
            var cols = ExcelUtils.ColumnsInRow(calcSheet, 1);

            // Fill in formulas and calculate concentration
            for (int row = 4; row <= rows; ++row)
            {
                // Calculate row with "Concentration (uM)"
                var compound = calcSheet.Cells[row, compoundLoc]?.Value?.ToString();
                if ("Concentration (uM)".Equals(compound))
                {
                    // Change "Concentration (uM)" to compound name
                    compound = calcSheet.Cells[row - 2, compoundLoc]?.Value?.ToString();
                    calcSheet.Cells[row, compoundLoc].Value = compound;

                    if (!concMap.Contains(compound))
                        concMap.Add(compound, new OrderedDictionary());

                    // Copy formula into empty cells and calculate
                    for (int col = 2; col <= cols; ++col)
                    {
                        var cell = calcSheet.Cells[row, col]?.Value?.ToString();
                        if (cell is null || cell.Length < 1)
                            calcSheet.Cells[row, col - 1].Copy(calcSheet.Cells[row, col]);

                        calcSheet.Cells[row, col].Calculate();

                        try
                        {
                            var sampleName = calcSheet.Cells[1, col]?.Value?.ToString();
                            if (sampleName is null || sampleName == "Compound") continue;

                            if (((OrderedDictionary) concMap[compound]).Contains(sampleName))
                                sampleName = Merge.RenameDuplicate(((OrderedDictionary) concMap[compound]).Keys,
                                    sampleName);

                            var conc = calcSheet.Cells[row, col]?.Value?.ToString();

                            ((OrderedDictionary) concMap[compound]).Add(sampleName, conc);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            // Save calculations
            excelPkg.Save();

            // Write concentrations to "Absolute Quant Data" tab
            excelPkg = MapToExcel.WriteIntoTemplate(concMap, excelPkg, options, "Absolute Quant Data");

            // Format to 3 decimal places
            var writeCols = ExcelUtils.ColumnsInRow(writeSheet, 1);
            writeSheet.Cells[1, 4, 31, writeCols].Style.Numberformat.Format = "0.000";
            excelPkg.Save();

            // Insert CV columns for QC
            excelPkg = QualityControl.InsertCVColumns(excelPkg, options, "Absolute Quant Data");
            excelPkg.Save();

            // Replace missing values with N/A
            excelPkg = MissingValue.ReplaceMissing(excelPkg, "Absolute Quant Data", "N/A", options["StartInCell"]);
            excelPkg.Save();

            // Write "Absolute Concentration (mM or micromoles/Liter)" above samples
            return excelPkg;
        }
    }
}
