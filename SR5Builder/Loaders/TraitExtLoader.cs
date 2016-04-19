﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SR5Builder.Loaders
{
    public abstract class TraitExtLoader: TraitLoader
    {
        [XmlIgnore]
        public string DisplayName
        {
            get
            {
                if (ExtLabel != null && ExtLabel.Length > 0)
                    return String.Format(Name, "["+ExtLabel+"]");
                else return Name;
            }
        }

        public List<string> ExtArray { get; set; }

        public string ExtKind { get; set; }

        public string ExtLabel { get; set; }

        public string ExtPrompt { get; set; }
    }
}
