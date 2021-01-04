using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using OfficeOpenXml;

namespace ModernWPFcore
{
    class Merge
    {
        /// <summary>
        /// Merges two dataMaps into one.
        /// </summary>
        /// <param name="destMap"></param>
        /// <param name="srcMap"></param>
        /// <param name="replace"></param>
        /// <returns>Merged dataMap</returns>
        public static OrderedDictionary MergeMaps(
            OrderedDictionary destMap, OrderedDictionary srcMap, bool replace = false)
        {
            // <compound, <sample name, data>>
            foreach (DictionaryEntry compoundEntry in srcMap)
            {
                var compound = (string) compoundEntry.Key;
                // Add compound to destMap if not already there
                if (!destMap.Contains(compound)) 
                    destMap.Add(compound, new OrderedDictionary());

                // Add every sample data for that compound
                foreach (DictionaryEntry sampleData in (OrderedDictionary) compoundEntry.Value)
                {
                    var sampleName = (string) sampleData.Key;

                    // Check if sample name already exists for that compound
                    if (((OrderedDictionary) destMap[compound]).Contains(sampleName))
                    {
                        if (replace)
                        {
                            // Overwrite duplicate sample name
                            ((OrderedDictionary)destMap[compound])[sampleName] = sampleData.Value;
                        }
                        else
                        {
                            // Don't overwrite duplicate sample name, instead rename
                            var allKeys = ((OrderedDictionary)destMap[compound]).Keys;
                            sampleName = RenameDuplicate(allKeys, sampleName);
                        }
                    }
                    ((OrderedDictionary)destMap[compound]).Add(sampleName, sampleData.Value);
                }
            }
            return destMap;
        }

        /// <summary>
        /// Renames a sample if the sample name already exists in the dictionary.
        /// An asterisk * is appended to the end of the sample name.
        /// </summary>
        /// <param name="allKeys"></param>
        /// <param name="currentName"></param>
        /// <returns>Renamed sample as a string</returns>
        public static string RenameDuplicate(ICollection allKeys, string currentName)
        {
            List<string> allNamesList = allKeys.Cast<string>().ToList();
            while (allNamesList.Contains(currentName))
                currentName += "*";

            return currentName;
        }

        public static ExcelPackage MergeCells(
            ExcelPackage excelPkg, string tabName, string startCell, string endCell, string text = "")
        {
            var worksheet = excelPkg.Workbook.Worksheets[tabName];
            worksheet.Cells[startCell].Value = text;
            worksheet.Cells[$"{startCell}:{endCell}"].Merge = true;
            return excelPkg;
        }

        public static ExcelPackage MergeCells(
            ExcelPackage excelPkg, string tabName, int startRow, int startCol, int endRow, int endCol, string text = "")
        {
            var worksheet = excelPkg.Workbook.Worksheets[tabName];
            worksheet.Cells[startRow, startCol].Value = text;
            worksheet.Cells[startRow, startCol, endRow, endCol].Merge = true;
            return excelPkg;
        }
    }
}
