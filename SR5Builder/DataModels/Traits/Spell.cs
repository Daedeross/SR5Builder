using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SR5Builder.DataModels
{
    public class Spell: BaseTrait, IKarmaCost
    {
        //public string Category { get; set; }
        protected bool mFree;
        public bool Free
        {
            get { return mFree; }
            set
            {
                if (value != mFree)
                {
                    mFree = value;
                    RaisePropertyChanged(nameof(Free));
                    RaisePropertyChanged(nameof(Karma));
                }
            }
        }

        public string Type { get; set; }

        public string Range { get; set; }

        public string Damage { get; set; }

        public string Duration { get; set; }

        public string Drain { get; set; }

        public string[] Tags { get; set; }

        public int Karma
        {
            get { return Free ? 0 : mOwner.Settings.SpellKarma; }
        }

        public Spell()
            : base(null)
        {

        }


        public Spell(SR5Character c, bool isFree)
            :base(c)
        {
            mFree = isFree;
        }

        #region Public Methods

        public Spell Clone(SR5Character c)
        {
            Spell newSpell = new Spell(c, Free);
            newSpell.Name = this.Name;
            newSpell.Type = this.Type;
            newSpell.Range = this.Range;
            newSpell.Damage = this.Damage;
            newSpell.Duration = this.Duration;
            newSpell.Drain = this.Drain;
            newSpell.Tags = (string[])this.Tags.Clone();
            newSpell.Book = this.Book;
            newSpell.Page = this.Page;
            return newSpell;
        }

        public static List<Spell> LoadFromFile(string filename)
        {
            List<Spell> list;

            XmlSerializer ser = new XmlSerializer(typeof(List<Spell>));
            StreamReader reader = new StreamReader(filename);

            list = (List<Spell>)ser.Deserialize(reader);

            reader.Close();

            return list;
        }

        #endregion
    }
}
