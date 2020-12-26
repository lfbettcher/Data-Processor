using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            MessageBox.Show(io.AbsoluteQuantTabName.Text);
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
                MapToExcel.WriteSamplesInColumns(dataMap, options, excelPkg, "Raw Data", progressPage);
            else
                MapToExcel.WriteSamplesInRows(dataMap, options, excelPkg, "Raw Data", progressPage);

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
