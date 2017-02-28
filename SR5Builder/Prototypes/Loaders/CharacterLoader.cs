using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.DataModels;
using System.Xml.Serialization;
using Attribute = SR5Builder.DataModels.Attribute;
using System.IO;
using SR5Builder.Helpers;

namespace SR5Builder.Prototypes.Loaders
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

        public List<SkillLoader> Skills { get; set; }
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
            Settings = character.Settings;

            Attributes = character.Attributes.Select(a =>
                new AttributeLoader
                {
                    Name     = a.Key,
                    Base     = a.Value.BaseRating,
                    Improved = a.Value.ImprovedRating
                }).ToList();

            Skills = character.SkillList.Select(s =>
            new SkillLoader
            {
                Name = s.Key,
                Base = s.Value.BaseRating,
                GroupName = s.Value.GroupName,
                Improved = s.Value.ImprovedRating,
                Kind = s.Value.Kind,
                Limit = s.Value.UsualLimit,
                LinkedAttribute = s.Value.LinkedAttribute
            }).ToList();

        }

        public void WriteToFile(string filename)
        {
            using (var stream = new StreamWriter(filename))
            {
                var ser = new XmlSerializer(typeof(CharacterLoader));
                ser.Serialize(stream, this);
            }
        }

        public CharacterLoader LoadFromFile(string filename)
        {
            using (var stream = new StreamReader(filename))
            {
                var ser = new XmlSerializer(typeof(CharacterLoader));
                try
                {
                    var cl = (CharacterLoader)ser.Deserialize(stream);
                    return cl;
                }
                catch (Exception)
                {
                    Log.LogMessage($"Error deserializing character from file: {filename}");
                    throw;
                }
            }
        }
    }
}
