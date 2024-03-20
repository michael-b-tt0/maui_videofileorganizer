using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoFileRenamer;


public class VideoResolutionConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 && values[0] is int width && values[1] is int height && targetType == typeof(string))
        {
            // Determine the resolution label based on width and height
            string resolutionLabel = GetResolutionLabel(width, height);
            return resolutionLabel;
        }
        return values;
        
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    private string GetResolutionLabel(int width, int height)
    {
        // Determine the resolution label based on width and height
        if (width == 3840 && height == 2160)
        {
            return "4K UHD";
        }
        else if (width == 1920 && height == 1080)
        {
            return "Full HD (1080p)";
        }
        else if (width == 1280 && height == 720)
        {
            return "HD (720p)";
        }
        else if (width == 854 && height == 480)
        {
            return "480p";
        }
        else
        {
            return $"{width}x{height}";
        }
    }
}

