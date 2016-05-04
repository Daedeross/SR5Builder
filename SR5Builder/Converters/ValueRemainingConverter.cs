using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SR5Builder.Converters
{
    public class ValueRemainingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                return "0 / 0";
            }
            // get text for value 1 and 2
            string v1, v2;
            v1 = values[0].ToString() ?? "0";
            v2 = values[1].ToString() ?? "0";
            return string.Format("{0} / {1}", v1, v2);
        }

        private static char[] delims = new char[] { '/' };

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            string input = value.ToString();
            string[] vals = input.Split(delims);
            object[] ret = new object[2];

            try
            {
                decimal v0, v1;
                decimal.TryParse(vals[0].Trim(), out v0);
                ret[0] = v0;
                decimal.TryParse(vals[1].Trim(), out v1);
                ret[1] = v1;
            }
            catch (IndexOutOfRangeException)
            {
                ret[0] = ret[0] ?? (decimal)0;
                ret[1] = ret[1] ?? (decimal)0;
            }
            return ret;
        }
    }
}
