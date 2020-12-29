using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ModernWPFcore
{
    class Merge
    {
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

        public static string RenameDuplicate(ICollection allKeys, string currentName)
        {
            List<string> allNamesList = allKeys.Cast<string>().ToList();
            while (allNamesList.Contains(currentName))
                currentName += "*";

            return currentName;
        }
    }
}
