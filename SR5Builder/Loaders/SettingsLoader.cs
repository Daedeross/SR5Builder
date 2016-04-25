﻿using System.Xml;
using System.Linq;
using System.IO;
using DrWPF.Windows.Data;
using System.Xml.Linq;
using System.Collections.Generic;
using System;

namespace SR5Builder.Loaders
{
    //public class CharacterSetting
    //{
    //    public Type DataType { get; set; }
    //    public object Value { get; set; }

    //    public static CharacterSetting Parse(string t, string v)
    //    {
    //        if (t == "int")
    //        {
    //            int i;
    //            Int32.TryParse(v, out i); // i = 0 if this fails
    //            return new CharacterSetting() { DataType = typeof(int), Value = i };
    //        }
    //        else if (t == "float")
    //        {
    //            float f;
    //            float.TryParse(v, out f);
    //            return new CharacterSetting() { DataType =typeof(float), Value = f };
    //        }
    //        else if (t == "CharGenMethod")
    //        {
    //            CharGenMethod m;
    //            Enum.TryParse<CharGenMethod>(v, out m);
    //            return new CharacterSetting() { DataType = typeof(CharGenMethod), Value = m };
    //        }
    //        else
    //        {
    //            return new CharacterSetting() { DataType = typeof(string), Value = string.Copy(v) };
    //        }
    //    }
    //}
    public class SettingsLoader
    {
        public ObservableDictionary<string, object> Properties { get; set; }

        public SettingsLoader()
        {
            Properties = new ObservableDictionary<string, object>();
        }

        public static SettingsLoader LoadFromFile(string filename)
        {
            XDocument doc = XDocument.Load(filename);
            SettingsLoader loader = new SettingsLoader();

            // pull data from file
            loader.Properties = new ObservableDictionary<string, object>((
                from el in doc.Descendants("GenSettings")
                select new KeyValuePair<string, object>
                    (el.Name.ToString(), ParseProperty(el.Attribute("type").Value,
                                                                el.Value))
                ).ToDictionary(e => e.Key, e => e.Value));

            return loader;
        }

        public static object ParseProperty(string t, string v)
        {
            if (t == "int")
            {
                int i;
                Int32.TryParse(v, out i); // i = 0 if this fails
                return i;
            }
            else if (t == "float")
            {
                float f;
                float.TryParse(v, out f);
                return f;
            }
            else if (t == "CharGenMethod")
            {
                CharGenMethod m;
                Enum.TryParse<CharGenMethod>(v, out m);
                return m;
            }
            else
            {
                return v;
            }
        }
    }
}
