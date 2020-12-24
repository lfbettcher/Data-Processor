using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernWPFcore
{
    class MergeMaps
    {
        public static Dictionary<string, Dictionary<string, string>> MergeMapsReplaceDuplicates(
            Dictionary<string, Dictionary<string, string>> destMap, Dictionary<string, Dictionary<string, string>> srcMap)
        {
            foreach (var compound in srcMap.Keys)
            {
                // Copy compounds from srcMap to destMap
                if (!destMap.TryAdd(compound, srcMap[compound]))
                {
                    // If can't copy, compound is already in destMap, merge Dictionary<sample name, data>
                    // If sample name already has data, overwrite data for that sample
                    // *** Test if this actually works with duplicated sample data ***
                    destMap[compound].Union(srcMap[compound]
                            .Where(k => !destMap.ContainsKey(k.Key)))
                        .ToDictionary(k => k.Key, v => v.Value);
                }
            }

            return destMap;
        }
    }
}
