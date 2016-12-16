using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.DataModels;
//using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SR5Builder.Loaders
{
    public class CharacterLoader
    {
        #region Properties

        GenSettings Settings { get; set; }

        public string Name { get; set; }

        public string Metatype { get; set; }

        public Dictionary<string, AttributeLoader> Attributes { get; set; }

        public string SpecialChoice { get; set; }

        public List<QualityLoader> Qualities { get; set; }

        #endregion

        /// <summary>
        /// Parameterless Constructor, for Deserialize
        /// </summary>
        public CharacterLoader()
        {
            Attributes = new Dictionary<string, AttributeLoader>();
        }

        public SR5Character ToCharacter()
        {
            var character = new SR5Character();
            return character;
        }
    }
}
