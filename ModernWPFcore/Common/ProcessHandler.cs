using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using ModernWPFcore.Pages;
using OfficeOpenXml;

namespace ModernWPFcore
{
    class ProcessHandler
    {
        
        public static void Run(string menuSelection, List<string> filePathList, Dictionary<string, string> options, ProgressPage progressPage, InputOutputPage io)
        {

            // Read input - All data formats are read into one dataMap format for further processing
            progressPage.ProgressTextBox.AppendText("Processing inputs\n");
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
                case "SciexLipidyzer":
                    break;
                default:
                    break;
            }
            return dataMap;
        }

        public static void WriteOutput(string menuSelection, OrderedDictionary dataMap, 
            Dictionary<string, string> options, ProgressPage progressPage)
        {
            var templateFile = new FileInfo(options["TemplatePath"]);
            var excelPkg = new ExcelPackage(templateFile);

            // Write Raw Data tab
            if (options["SamplesOut"] == "columns")
                excelPkg = MapToExcel.WriteSamplesInColumns(dataMap, options, excelPkg, "Raw Data", progressPage);
            else
                MapToExcel.WriteSamplesInRows(dataMap, options, excelPkg, "Raw Data", progressPage);

            // Write to template if selected
            progressPage.ProgressTextBox.AppendText("Writing data into template\n");
            excelPkg = MapToExcel.WriteIntoTemplate(dataMap, excelPkg, options, options["TemplateTabName"]);

            // Absolute Quant Calc
            progressPage.ProgressTextBox.AppendText("Absolute Quantitation\n");
            var compoundLoc = int.TryParse(options["CompoundLoc"], out var compoundLocNum)
                ? compoundLocNum
                : ExcelUtils.ColumnNameToNumber(options["CompoundLoc"]);

            excelPkg = MapToExcel.WriteIntoTemplate(dataMap, excelPkg, options, options["AbsoluteQuantTabName"], false, 2, 3, 1, compoundLoc);
            excelPkg = WriteSciex6500Output.AbsoluteQuantCalc(excelPkg, options, compoundLoc);

            switch (menuSelection)
            {
                case "Sciex6500":
                    break;
                case "SciexLipidyzer":
                    break;
                default:
                    break;
            }
        }
    }
}
