using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoFileRenamer;

public class DatetimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime timeSpan)
        {
            string format = "dd.MM.yyyy";// Default format is "g" (general short time pattern)
            string formattedTimeSpan = timeSpan.ToString(format);


            return  formattedTimeSpan;
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }


}
