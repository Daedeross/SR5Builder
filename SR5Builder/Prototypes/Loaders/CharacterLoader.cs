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
        [XmlElement]
        public string Name { get; set; }

        public string Metatype { get; set; }

        public PrioritiesLoader Priorities { get; set; }

        public string SpecialChoice { get; set; }

        #region Collections
        public List<AttributeLoader> Attributes { get; set; }
        public List<SkillLoader> Skills { get; set; }
        public List<SkillGroupLoader> SkillGroups { get; set; }
        public List<SpellLoader> Spells { get; set; }
        public List<AdeptPowerLoader> AdeptPowers { get; set; }
        #endregion

        #endregion

        /// <summary>
        /// Parameterless Constructor, for Deserialize
        /// </summary>
        public CharacterLoader()
        {
            Attributes = new List<AttributeLoader>();
        }

        public CharacterLoader(SR5Character c)
        {
            Settings = c.Settings;

            Name = c.Name;

            Metatype = c.Metatype;

            SpecialChoice = c.SpecialChoice.Name;

            Priorities = new PrioritiesLoader
            {
                Metatype = c.Priorities.Metatype,
                Attributes = c.Priorities.Attributes,
                Special = c.Priorities.Special,
                Skills = c.Priorities.Skills,
                Resources = c.Priorities.Resources
            };

            Attributes = c.Attributes.Select(a =>
                new AttributeLoader
                {
                    Name     = a.Key,
                    Base     = a.Value.BaseRating,
                    Improved = a.Value.ImprovedRating
                }).ToList();

            Skills = c.SkillList.Select(s =>
                new SkillLoader(s.Value)).ToList();
            SkillGroups = c.SkillGroupsList.Select(s =>
                new SkillGroupLoader(s.Value)).ToList();
            Spells = c.SpellList.Select(s =>
                new SpellLoader(s.Value)).ToList();
            AdeptPowers = c.PowerList.Select(p =>
                new AdeptPowerLoader(p.Value)).ToList();
        }

        public void WriteToFile(string filename)
        {
            using (var stream = new StreamWriter(filename))
            {
                var ser = new XmlSerializer(typeof(CharacterLoader));
                ser.Serialize(stream, this);
            }
        }

        public static CharacterLoader LoadFromFile(string filename)
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
