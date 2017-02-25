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

        public GenSettings Settings { get; set; }
        public string Name { get; set; }

        public string Metatype { get; set; }
        public SpecialChoice SpecialChoice { get; set; }

        #region Attributes
        public List<AttributeLoader> Attributes { get; set; }
        #endregion

        #endregion

        /// <summary>
        /// Parameterless Constructor, for Deserialize
        /// </summary>
        public CharacterLoader()
        {
            Attributes = new List<AttributeLoader>();
        }

        public CharacterLoader(SR5Character character)
        {
            Attributes = new List<AttributeLoader>();
            Settings = character.Settings;
            foreach (var kvp in character.Attributes)
            {
                Attributes.Add(new AttributeLoader
                {
                    Name = kvp.Key,
                    Base = kvp.Value.BaseRating,
                    Improved = kvp.Value.ImprovedRating
                });
            }
        }
    }
}
