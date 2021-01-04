using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Windows.Data;
using Path = System.IO.Path;

namespace ModernWPFcore
{

    public class PathToDirectoryConverter : IValueConverter
    {
        // Define the Convert method to change a File Path to File Name
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // The value parameter is the data from the source object
            string filePath = (string)value;
            string fileDirectory = Path.GetDirectoryName(filePath) ?? string.Empty;
            // Return the file name value to pass to target
            Debug.WriteLine("In Dir converter");
            Debug.WriteLine(fileDirectory);
            return fileDirectory;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as string;
        }
    }
}
