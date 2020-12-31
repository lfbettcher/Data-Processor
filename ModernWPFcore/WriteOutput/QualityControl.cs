using System.Collections.Generic;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ModernWPFcore
{
    class QualityControl
    {
        public static ExcelPackage WriteQCTab(ExcelPackage excelPkg, Dictionary<string, string> options)
        {
            // Copy data tab
            var copyTabName = options["WriteDataInTemplate"] == "True" ? options["TemplateTabName"] : "Raw Data";
            var qcTab = excelPkg.Workbook.Worksheets.Copy(copyTabName, "Data Reproducibility");
            qcTab.View.SetTabSelected(false);
            excelPkg.Save();

            // Remove non-QC
            excelPkg = RemoveNonQC(excelPkg, options, options["QCTabName"]);

            // Insert CV
            excelPkg = options["SamplesOut"] == "columns" 
                ? InsertCVColumns(excelPkg, options, options["QCTabName"]) 
                : InsertCVRows(excelPkg, options, options["QCTabName"]);

            return excelPkg;
        }

        public static ExcelPackage RemoveNonQC(ExcelPackage excelPkg, Dictionary<string, string> options, string tabName)
        {
            var worksheet = excelPkg.Workbook.Worksheets[tabName];

            if (options["SamplesOut"] == "columns")
            {
                var (nameRow, startCol) = GetNameLocStartLoc(
                    "columns", options["WriteDataInTemplate"], options["SampleLoc"], options["StartInCell"]);

                var cols = worksheet.Dimension.Columns;
                // Remove non-QC 
                for (var col = startCol; col <= cols; ++col)
                {
                    var cell = worksheet.Cells[nameRow, col]?.Value?.ToString();
                    if (cell?.Contains("QC") == false)
                        worksheet.DeleteColumn(col--);
                }
            }
            else if (options["SamplesOut"] == "rows")
            {
                var (nameCol, startRow) = GetNameLocStartLoc(
                    "rows", options["WriteDataInTemplate"], options["SampleLoc"], options["StartInCell"]);

                var rows = worksheet.Dimension.Rows;
                // Remove non-QC 
                for (var row = startRow; row <= rows; ++row)
                {
                    var cell = worksheet.Cells[row, nameCol]?.Value?.ToString();
                    if (cell?.Contains("QC") == false)
                        worksheet.DeleteRow(row--);
                }
            }
            excelPkg.Save();
            //excelPkg.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));

            return excelPkg;
        }

        public static ExcelPackage InsertCVColumns(ExcelPackage excelPkg, Dictionary<string, string> options, string tabName)
        {
            var worksheet = excelPkg.Workbook.Worksheets[tabName];

            var (nameRow, startCol) = GetNameLocStartLoc(
                "columns", options["WriteDataInTemplate"], options["SampleLoc"], options["StartInCell"]);
            var lastRow = 1;
            var cols = ExcelUtils.ColumnsInRow(worksheet, nameRow);
            
            // Insert CV columns after QC(I) and QC(S). CV, Avg CV, Median CV
            for (var col = startCol; col <= cols + 6; ++col)
            {
                var cell = worksheet.Cells[nameRow, col]?.Value?.ToString();
                if (cell?.Contains("QC") == true)
                {
                    // Compare current QC string to next cell QC string
                    var nextCell = worksheet.Cells[nameRow, col + 1]?.Value?.ToString();
                    if (cell?.Contains("I") == true && nextCell?.Contains("I") == false ||
                        cell?.Contains("S") == true && nextCell?.Contains("S") == false ||
                        string.IsNullOrWhiteSpace(nextCell))
                    {
                        // Insert 3 columns
                        worksheet.InsertColumn(col + 1, 3);
                        worksheet.Cells[nameRow, col + 1].Value = "CV";
                        worksheet.Cells[nameRow, col + 2].Value = "Avg CV";
                        worksheet.Cells[nameRow, col + 3].Value = "Median CV";

                        /* Guide for row and column names
                         *              startCol              col     col + 1   col + 2   col + 3
                         * nameRow      QC(I)#1    QC(I)#2   QC(I)#3   CV       Avg CV    Median CV
                         * nameRow + 1  startCalc   ...      endCalc  formula
                         * row           data       data      data    copy formula
                         */

                        // Write first CV formula
                        var startCalc = worksheet.Cells[nameRow + 1, startCol].Address;
                        var endCalc = worksheet.Cells[nameRow + 1, col].Address;

                        worksheet.Cells[nameRow + 1, col + 1].Formula =
                            $"STDEV({startCalc}:{endCalc})/AVERAGE({startCalc}:{endCalc})";

                        // Copy CV formula down while there is data
                        var row = nameRow + 2;
                        while (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col]?.Value?.ToString()))
                        {
                            worksheet.Cells[row - 1, col + 1].Copy(worksheet.Cells[row, col + 1]);
                            ++row;
                        }

                        lastRow = row;
                        
                        // Calculate CV column
                        worksheet.Cells[nameRow + 1, col + 1, row - nameRow, col + 1].Calculate();

                        // Calculate average and median CV
                        var startCV = worksheet.Cells[nameRow + 1, col + 1].Address;
                        var endCV = worksheet.Cells[row - nameRow, col + 1].Address;
                        worksheet.Cells[nameRow + 1, col + 2]
                            .CreateArrayFormula($"AVERAGE(IF(ISNUMBER({startCV}:{endCV}),{startCV}:{endCV}))");
                        worksheet.Cells[nameRow + 1, col + 3]
                            .CreateArrayFormula($"MEDIAN(IF(ISNUMBER({startCV}:{endCV}),{startCV}:{endCV}))");

                        // Format to % with 2 decimal places
                        worksheet.Cells[nameRow + 1, col + 1, row - nameRow, col + 3]
                            .Style.Numberformat.Format = "#0.00%";

                        // Center and bold text in cell
                        worksheet.Cells[nameRow, col + 1, row - nameRow, col + 3]
                            .Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[nameRow, col + 1, row - nameRow, col + 3]
                            .Style.Font.Bold = true;
                        worksheet.Cells[nameRow, col + 1, row - nameRow, col + 3]
                            .Style.Font.Color.SetColor(Color.Red);

                        // Header - will be inserted into row 1 at the end to preserve current row count
                        if (cell.Contains("I"))
                        {
                            worksheet.Cells[lastRow, startCol].Value = "Instrument QC (Pooled Serum Samples) Reproducibility";
                            worksheet.Cells[nameRow, startCol, lastRow, col + 3]
                                .Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[nameRow, startCol, lastRow, col + 3]
                                .Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        }
                        else if (cell.Contains("S"))
                        {
                            worksheet.Cells[lastRow, startCol].Value = "Sample QC (Pooled Study Samples) Reproducibility";
                            worksheet.Cells[nameRow, startCol, lastRow, col + 3]
                                .Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[nameRow, startCol, lastRow, col + 3]
                                .Style.Fill.BackgroundColor.SetColor(1, 146, 208, 80); 
                            // Excel Standard Color "Light Green"
                        }
                        // Merge header cells
                        worksheet.Cells[lastRow, startCol, lastRow, col + 3].Merge = true;

                        // Reset for next group of QC
                        col += 3;
                        startCol = col + 1;
                    }
                }
                else
                {
                    // Cell was not QC, increment startCol to track the start of CV calculation range
                    ++startCol;
                }
            }

            // Move header to row 1
            var lastCol = ExcelUtils.ColumnsInRow(worksheet, nameRow);
            worksheet.InsertRow(1, 1);
            lastRow += 1;
            var headerRow = worksheet.Cells[1, 1, 1, lastCol];
            worksheet.Cells[lastRow, 1, lastRow, lastCol].Copy(headerRow);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Font.Size = 14;
            worksheet.DeleteRow(lastRow);

            excelPkg.Save();
            //excelPkg.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));

            return excelPkg;
        }

        public static ExcelPackage InsertCVRows(ExcelPackage excelPkg, Dictionary<string, string> options, string tabName)
        {
            var worksheet = excelPkg.Workbook.Worksheets[tabName];

            var (nameCol, startRow) = GetNameLocStartLoc(
                "rows", options["WriteDataInTemplate"], options["SampleLoc"], options["StartInCell"]);

            var rows = ExcelUtils.RowsInColumn(worksheet, nameCol);

            // Insert CV columns after QC(I) and QC(S). CV, Avg CV, Median CV
            for (var row = startRow; row <= rows + 6; ++row)
            {
                var cell = worksheet.Cells[row, nameCol]?.Value?.ToString();
                if (cell?.Contains("QC") == true)
                {
                    // Compare current QC string to next cell QC string
                    var nextCell = worksheet.Cells[row + 1, nameCol]?.Value?.ToString();
                    if (cell?.Contains("I") == true && nextCell?.Contains("I") == false ||
                        cell?.Contains("S") == true && nextCell?.Contains("S") == false ||
                        string.IsNullOrWhiteSpace(nextCell))
                    {
                        // Insert 3 rows
                        worksheet.InsertRow(row + 1, 3);
                        worksheet.Cells[row + 1, nameCol].Value = "CV";
                        worksheet.Cells[row + 2, nameCol].Value = "Avg CV";
                        worksheet.Cells[row + 3, nameCol].Value = "Median CV";

                        /* Guide for row and column names
                         * nameCol    nameCol + 1  nameCol + 2      col
                         * startRow   QC(I)#1      data  startCalc  data
                         *            QC(I)#2      data     |       data
                         * row        QC(I)#3      data  endCalc    data
                         * row + 1    CV           formula     ->  copy formula 
                         * row + 2    Avg CV
                         * row + 3    Median CV
                         */

                        // Write first CV formula
                        var startCalc = worksheet.Cells[startRow, nameCol + 1].Address;
                        var endCalc = worksheet.Cells[row, nameCol + 1].Address;

                        worksheet.Cells[row + 1, nameCol + 1].Formula =
                            $"STDEV({startCalc}:{endCalc})/AVERAGE({startCalc}:{endCalc})";

                        // Copy CV formula across while there is data
                        var col = nameCol + 2;
                        while (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col]?.Value?.ToString()))
                        {
                            worksheet.Cells[row + 1, col - 1].Copy(worksheet.Cells[row + 1, col]);
                            ++col;
                        }
                        // Calculate CV row
                        worksheet.Cells[row + 1, nameCol + 1, row + 1, col - nameCol].Calculate();

                        // Calculate average and median CV
                        var startCV = worksheet.Cells[row + 1, nameCol + 1].Address;
                        var endCV = worksheet.Cells[row + 1, col - nameCol].Address;
                        worksheet.Cells[row + 2, nameCol + 1]
                            .CreateArrayFormula($"AVERAGE(IF(ISNUMBER({startCV}:{endCV}),{startCV}:{endCV}))");
                        worksheet.Cells[row + 3, nameCol + 1]
                            .CreateArrayFormula($"MEDIAN(IF(ISNUMBER({startCV}:{endCV}),{startCV}:{endCV}))");

                        // Format to % with 2 decimal places
                        worksheet.Cells[row + 1, nameCol + 1, row + 3, col - nameCol]
                            .Style.Numberformat.Format = "#0.00%";

                        // Center and bold text in cell
                        worksheet.Cells[row + 1, nameCol, row + 3, col - nameCol]
                            .Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row + 1, nameCol, row + 3, col - nameCol].Style.Font.Bold = true;

                        // Reset for next group of QC
                        row += 3;
                        startRow = row + 1;
                    }
                }
                else
                {
                    // Cell was not QC, increment startRow to track the start of CV calculation range
                    ++startRow;
                }
            }
            excelPkg.Save();
            //excelPkg.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));

            return excelPkg;
        }

        // Returns the location of sample names and starting data cell
        public static (int, int) GetNameLocStartLoc(
            string rowsOrColumns, string writeDataInTemplate, string sampleLoc, string startInCell)
        {
            // Default
            var nameLoc = 1;
            var startLoc = 2;

            if (writeDataInTemplate == "True")
            {
                if (rowsOrColumns == "columns")
                {
                    nameLoc = ExcelUtils.GetRowNum(sampleLoc);  // nameRow
                    startLoc = ExcelUtils.GetRowCol(startInCell).col;  // startCol
                }
                else if (rowsOrColumns == "rows")
                {
                    nameLoc = ExcelUtils.GetColNum(sampleLoc);  // nameCol
                    startLoc = ExcelUtils.GetRowCol(startInCell).row;  // startRow
                }
            }
            return (nameLoc, startLoc);
        }

    }
}
