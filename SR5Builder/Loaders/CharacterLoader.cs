using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.DataModels;
using System.Xml.Serialization;
using Attribute = SR5Builder.DataModels.Attribute;

namespace SR5Builder.Loaders
{
    public class CharacterLoader
    {
        #region Properties

        public string Name { get; set; }

        public string Metatype { get; set; }

        public List<Attribute> Attributes { get; set; }

        #endregion

        /// <summary>
        /// Parameterless Constructor, for Deserialize
        /// </summary>
        public CharacterLoader()
        {

        }

        public CharacterLoader(SR5Character character)
        {
             
        }
    }
}
