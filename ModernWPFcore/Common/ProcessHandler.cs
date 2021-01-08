using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using ModernWPFcore.Pages;
using ModernWPFcore.WriteOutput;
using OfficeOpenXml;

namespace ModernWPFcore
{
    class ProcessHandler
    {
        
        public static void Run(string menuSelection, List<string> filePathList, Dictionary<string, string> options, ProgressPage progressPage, InputOutputPage io)
        {
            // Read input - All data formats are read into one dataMap format for further processing
            progressPage.ProgressTextBox.AppendText("Processing inputs\n");
            if (menuSelection == "Lipidyzer")
            {
                Lipidyzer(filePathList, options);
                return;
            }

            var dataMap = ReadInputs(menuSelection, filePathList, options, progressPage);
            
            // Perform data options

            // Write output
            progressPage.ProgressTextBox.AppendText("Writing output file\n");
            WriteOutput(menuSelection, dataMap, options, progressPage);
        }

        public static OrderedDictionary ReadInputs(string menuSelection, List<string> filePathList,
            Dictionary<string, string> options, ProgressPage progressPage)
        {
            var dataMap = new OrderedDictionary();

            switch (menuSelection)
            {
                case "Sciex6500":
                    dataMap = options["InputType"] == "text"
                        ? ReadMultiQuantTextInput.ReadMultiQuantText(filePathList, options, progressPage)
                        : ExcelToMap.ReadAllFiles(filePathList, options);
                    break;
                case "Lipidyzer":
                    // Read one book at a time, write into template
                    break;
                default:
                    break;
            }
            return dataMap;
        }

        public static void WriteOutput(string menuSelection, OrderedDictionary dataMap, 
            Dictionary<string, string> options, ProgressPage progressPage)
        {
            // Get template and save as output file
            var excelPkg = new ExcelPackage(new FileInfo(options["TemplatePath"]));
            excelPkg.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));

            var (startRow, startCol) = ExcelUtils.GetRowCol(options["StartInCell"]);

            // Write Raw Data tab
            if (options["SamplesOut"] == "columns")
                excelPkg = MapToExcel.WriteSamplesInColumns(dataMap, excelPkg, "Raw Data", options);
            else
                // TODO - fix this. Method was separated/changed
                //MapToExcel.WriteSamplesInRows(dataMap, excelPkg, "Raw Data", options);
                //MapToExcel.WriteCompoundsInColumns
                
            // Write to template if selected
            progressPage.ProgressTextBox.AppendText("Writing data into template\n");
            excelPkg = MapToExcel.WriteIntoTemplate(dataMap, excelPkg, options, options["TemplateTabName"]);

            // QC - Copy data tab, remove non-QC, calculate CV
            progressPage.ProgressTextBox.AppendText("Calculating QC CV\n");
            excelPkg = QualityControl.WriteQCTab(excelPkg, options);

            // Absolute Quant Calc
            progressPage.ProgressTextBox.AppendText("Absolute Quantitation\n");
            var compoundLoc = int.TryParse(options["CompoundLoc"], out var compoundLocNum)
                ? compoundLocNum
                : ExcelUtils.ColumnNameToNumber(options["CompoundLoc"]);

            excelPkg = MapToExcel.WriteIntoTemplate(dataMap, excelPkg, options, options["AbsoluteQuantTabName"], false, 2, 3, 1, compoundLoc);
            excelPkg = AbsoluteQuant.Sciex6500Template(excelPkg, options, compoundLoc);
            progressPage.ProgressTextBox.AppendText("Finished writing Absolute Quant Tab\n");

            switch (menuSelection)
            {
                case "Sciex6500":
                    break;
                case "Lipidyzer":
                    break;
                default:
                    break;
            }
        }

        // Process Handler for Sciex Lipidyzer
        public static void Lipidyzer(List<string> filePathList, Dictionary<string, string> options)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus license

            // Get template and save as output file
            var destExcel = new ExcelPackage(new FileInfo(options["TemplatePath"]));
            destExcel.SaveAs(new FileInfo(options["OutputFolder"] + "\\" + options["OutputFileName"]));

            filePathList.Sort();

            // Merge each file into template
            foreach (var file in filePathList)
            {
                var srcExcel = new ExcelPackage(new FileInfo(file));
                destExcel = Merge.MergeWorkbooks(srcExcel, destExcel, options);
            }

            // Format and options for every sheet
            foreach (var sheet in destExcel.Workbook.Worksheets)
            {
                // Do nothing on the "Key" tab
                if (sheet.Name.Contains("Key", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Remove unwanted samples
                destExcel = RemoveReplace.RemoveSamples(destExcel, sheet.Name, options);

                // Replace missing values
                destExcel = RemoveReplace.ReplaceMissing(destExcel, sheet.Name, options["Replacement"]);

                // Format data to 4 decimal places
                destExcel = ExcelUtils.FormatCells(destExcel, sheet.Name, "0.0000");

            }

            destExcel.Save();
        }

    }
}
