using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using SR5Builder.DataModels;

namespace SR5Builder.Prototypes
{
    public class SpellPrototype : TraitExtPrototype
    {
        //public string Category { get; set; }

        public string Type { get; set; }

        public string Range { get; set; }

        public string Damage { get; set; }

        public string Duration { get; set; }

        public string Drain { get; set; }

        public string[] Tags { get; set; }

        public SpellPrototype()
        { }

        public SpellPrototype(Spell s)
        {
            Name = s.Name;
            Page = s.Page;
            Book = s.Book;
            Type = s.Type;
            Range = s.Range;
            Damage = s.Damage;
            Duration = s.Duration;
            Drain = s.Drain;
            Tags = s.Tags;
        }

        /// <summary>
        /// Creates Spell instance from the loader and attatches it to a character.
        /// </summary>
        /// <param name="owner">The character to attatch to.</param>
        /// <param name="ext">The name extention</param>
        /// <returns>An instance of the spell with proper name.</returns>
        public Spell ToSpell(SR5Character owner, bool isFree, string ext = "")
        {
            if ((ExtKind != null && ExtKind.Length > 0) && (ext == null || ext.Length == 0))
            {
                throw new ArgumentException("Spell requires an name extension.", "ext");
            }

            Spell sp = new Spell(owner, isFree);
            sp.Name = string.Format(Name, ext);
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

        public static List<SpellPrototype> LoadFromFile(string filename)
        {
            List<SpellPrototype> list;

            XmlSerializer ser = new XmlSerializer(typeof(List<SpellPrototype>));//, "SR5Builder/Spells.xsd");
            StreamReader reader = new StreamReader(filename);

            list = (List<SpellPrototype>)ser.Deserialize(reader);

            reader.Close();

            return list;
        }
    }
}
