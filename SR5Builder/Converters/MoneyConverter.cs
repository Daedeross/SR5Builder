using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace SR5Builder.Converters
{
    /// <summary>
    /// Implements the conversion between an <code>int</code> a 
    /// <code>string</code> formatted for Shadowrun currency.
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    public class MoneyConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is int || value is decimal))
                return "—";

            decimal val = (decimal)value;

            if (val == 0)
                return "—";

            return val.ToString("C", GlobalData.CostFormat);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string val = (string)value;
            if (val == null)
                return 0;

            val = val.Replace(",", "").Replace("¥", "");

            return System.Convert.ToInt32(val);
        }

        #endregion
    }
}
