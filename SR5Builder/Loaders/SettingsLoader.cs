﻿using System.Xml;
using System.Linq;
using System.IO;
using DrWPF.Windows.Data;
using System.Xml.Linq;
using System.Collections.Generic;
using System;

namespace SR5Builder.Loaders
{
    public class SettingsLoader
    {
        public Dictionary<string, object> Properties { get; set; }

        public SettingsLoader()
        {
            Properties = new Dictionary<string, object>();
        }

        public static SettingsLoader LoadFromFile(string filename)
        {            
            XDocument doc = XDocument.Load(filename);
            SettingsLoader loader = new SettingsLoader();
            
            // pull data from file
            List<KeyValuePair<string, object>> tmpDict = new List<KeyValuePair<string, object>>();
            var settingsList = (from sett in doc.Descendants("GenSettings")
                           select sett.Nodes()).First();
            Dictionary<string, object> settings =
                (from sett in settingsList
                 where sett is XElement
                 select new KeyValuePair<string, object>(
                     (sett as XElement).Name.ToString(),
                     ParseProperty((sett as XElement).Attribute("type").Value, (sett as XElement).Value)
                     ) ).ToDictionary(e => e.Key, e => e.Value);
            loader.Properties = new Dictionary<string, object>(settings);
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
