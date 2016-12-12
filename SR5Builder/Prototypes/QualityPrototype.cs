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
        public string[] LevelNames { get; set; }

        public int Max { get; set; }

        public int Karma { get; set; }

        public int[] KarmaArray { get; set; }

        public string DisplayKarma
        {
            get
            {
                if (Max > 1)
                {
                    if ((KarmaArray == null || KarmaArray.Length <= 1))
                    {
                        return $"{Karma} ea. (max {Max})";
                    }
                    else
                    {
                        int min = KarmaArray[1];
                        int max = KarmaArray.Last();
                        return $"{min} to {max}";
                    }
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
            if (( ExtKind != null && ExtKind.Length > 0 && ExtKind != "level"))
            {
                throw new ArgumentException("Quality requires an name extension.", "ext");
            }

            Quality q = new Quality(c, Karma, this.PrereqExpression);
            base.CopyToTrait(q, ext);
            q.LevelNames = LevelNames;
            q.KarmaArray = KarmaArray;
            q.Min = 1;
            q.Max = Max;
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
                    XmlSerializer ser = new XmlSerializer(typeof(List<QualityPrototype>));
                    List<QualityPrototype> obj = (List<QualityPrototype>)ser.Deserialize(reader);
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
