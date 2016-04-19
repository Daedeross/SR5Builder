using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using SR5Builder.DataModels;

namespace SR5Builder.Loaders
{
    public class SpellLoader : TraitExtLoader
    {
        //public string Category { get; set; }

        public string Type { get; set; }

        public string Range { get; set; }

        public string Damage { get; set; }

        public string Duration { get; set; }

        public string Drain { get; set; }

        public string[] Tags { get; set; }

        /// <summary>
        /// Creates Spell instance from the loader and attatches it to a character.
        /// </summary>
        /// <param name="owner">The character to attatch to.</param>
        /// <param name="ext">The name extention</param>
        /// <returns>An instance of the spell with proper name.</returns>
        public Spell ToSpell(SR5Character owner, string ext = "")
        {
            if ((ExtKind != null && ExtKind.Length > 0) && (ext == null || ext.Length == 0))
            {
                throw new ArgumentException("Spell requires an name extension.", "ext");
            }

            Spell sp = new Spell(owner);
            if (ExtKind != null && ExtKind.Length > 0)
            {
                sp.Name = String.Format(Name, ext);
            }
            else
            {
                sp.Name = Name;
            }
            sp.Type = Type;
            sp.Range = Range;
            sp.Damage = Damage;
            sp.Duration = Duration;
            sp.Drain = Drain;
            sp.Tags = Tags;
            sp.Book = Book;
            sp.Page = Page;

            return sp;
        }

        public static List<SpellLoader> LoadFromFile(string filename)
        {
            List<SpellLoader> list;

            XmlSerializer ser = new XmlSerializer(typeof(List<SpellLoader>));//, "SR5Builder/Spells.xsd");
            StreamReader reader = new StreamReader(filename);

            list = (List<SpellLoader>)ser.Deserialize(reader);

            reader.Close();

            return list;
        }
    }
}
