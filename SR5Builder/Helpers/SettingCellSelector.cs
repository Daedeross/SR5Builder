using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SR5Builder.Helpers
{
    public class SettingCellSelector : DataTemplateSelector
    {
        public DataTemplate IntTemplate { get; set; }

        public DataTemplate FloatTemplate { get; set; }

        public DataTemplate StringTemplate { get; set; }

        public DataTemplate MethodTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            object val;
            if (item != null && item is KeyValuePair<string, object>?)
            {
                val = (item as KeyValuePair<string, object>?).Value;
            }
            else
            {
                return StringTemplate;
            }

            if (val is int)
            {
                return IntTemplate;
            }
            if (val is float)
            {
                return FloatTemplate;
            }
            if (val is CharGenMethod)
            {
                return MethodTemplate;
            }
            return StringTemplate;
        }
    }
}
