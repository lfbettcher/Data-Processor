using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;
using Path = System.IO.Path;

namespace ModernWPFcore
{
    public class PathToFileNameConverter : IValueConverter
    {
    
        // Define the Convert method to change a File Path to File Name
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // The value parameter is the data from the source object
            string filePath = (string) value;
            string fileName = Path.GetFileName(filePath) ?? string.Empty;

            // Return the file name value to pass to target
            Debug.WriteLine("In name converter");
            Debug.WriteLine(fileName);
            return fileName;
        }
    
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as string;
        }
    }

}