using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SR5Builder.DataModels;

namespace SR5Builder.Prototypes
{
    public abstract class TraitExtPrototype: TraitPrototype
    {
        [XmlIgnore]
        public string DisplayName
        {
            get
            {
                if (ExtLabel != null && ExtLabel.Length > 0)
                    return String.Format(Name, ExtLabel);
                else return Name;
            }
        }

        public List<string> ExtArray { get; set; }

        public string ExtKind { get; set; }

        public string ExtLabel { get; set; }

        public string ExtPrompt { get; set; }

        protected void CopyToTrait(BaseTrait trait, string ext)
        {
            trait.Name = string.Format(Name, ext);
            trait.Book = Book;
            trait.Page = Page;
            trait.Category = Category;
        }
    }
}
