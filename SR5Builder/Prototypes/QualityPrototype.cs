using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ExpressionEvaluator;
using SR5Builder.Helpers;
using SR5Builder.DataModels;

namespace SR5Builder.Prototypes
{
    public class QualityPrototype: TraitExtPrototype
    {
        public int Max { get; set; }

        public int Karma { get; set; }

        public string DisplayKarma
        {
            get
            {
                if (Max > 1)
                {
                    return string.Format("{0} ea. (max {1})", Karma, Max);
                }
                else return Karma.ToString();
            }
        }

        public AugmentPrototype[] Augments { get; set; }

        public string PrereqExpression { get; set; }

        public bool PrereqMet(DataModels.SR5Character character)
        {
            if (PrereqDelegate == null)
            {
                if (PrereqExpression != null)
                {
                    PrereqCE = new CompiledExpression<bool>(PrereqExpression);
                    PrereqDelegate = PrereqCE.ScopeCompile<DataModels.SR5Character>();
                }
                else
                {
                    PrereqDelegate = c => true;
                }
            }
            return PrereqDelegate(character);
        }

        private Func<DataModels.SR5Character, bool> PrereqDelegate;

        private CompiledExpression<bool> PrereqCE;

        public Quality ToQuality(DataModels.SR5Character c, string ext)
        {
            if ((ExtKind != null && ExtKind.Length > 0) && (ext == null || ext.Length == 0))
            {
                throw new ArgumentException("Quality requires a name extension.", "ext");
            }

            Quality q;

            if (string.IsNullOrWhiteSpace(PrereqExpression))
            {
                q = new Quality(c, Karma);
            }
            else
            {
                q = new Quality(c, Karma, PrereqExpression);
            }
            q.Name = string.Format(Name, ext);
            q.Min = 1;
            q.Max = Max;
            q.Book = Book;
            q.Page = Page;
            q.BaseRating = 1;

            if (Augments != null)
            {
                foreach (AugmentPrototype a in Augments)
                {
                    if (a.Target == "%ext%")
                        q.GivenAugments.Add(a.ToAugment(q, ext));
                    else q.GivenAugments.Add(a.ToAugment(q));
                }
            }

            return q;
        }

        public static List<QualityPrototype> LoadFromFile(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            if (!fi.Exists)
            {
                Log.LogMessage("Unable to load {0}", filename);
            }

            using (StreamReader reader = new StreamReader(filename))
            {
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(List<QualityPrototype>), "SR5Builder/Qualities.xsd");
                    List <QualityPrototype> obj = (List<QualityPrototype>)ser.Deserialize(reader);
                    return obj;
                }
                catch (IOException e)
                {
                    Log.LogMessage(e.Message);
                    return new List<QualityPrototype>();
                }
            }
        }
    }
}
