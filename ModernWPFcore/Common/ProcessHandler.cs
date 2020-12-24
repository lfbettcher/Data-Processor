using System;
using System.Collections.Generic;
using System.Text;
using ModernWPFcore.Pages;

namespace ModernWPFcore
{
    class ProcessHandler
    {
        public static void Run(string menuSelection, List<string> filePathList, Dictionary<string, string> options, ProgressPage progressPage)
        {

            // Read input - All data formats are read into one dataMap format for further processing
            progressPage.ProgressTextBox.AppendText("Processing inputs\n");
            var dataMap = ReadInputs(menuSelection, filePathList, options, progressPage);

            // Perform data options

            // Write output
        }

        public static Dictionary<string, Dictionary<string, string>> ReadInputs(string menuSelection, List<string> filePathList,
            Dictionary<string, string> options, ProgressPage progressPage)
        {
            var dataMap = new Dictionary<string, Dictionary<string, string>>();

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
    }
}
