namespace SR5Builder.Loaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DataModels;
    using Prototypes;

    public class QualityLoader: Prototypes.QualityPrototype
    {
        public int BaseRating { get; set; }

        public int ImprovedRating { get; set; }

        public string Ext { get; set; }

        public Quality ToQuality(SR5Character c)
        {
            Quality q = new Quality(c, Karma, this.PrereqExpression);
            base.CopyToTrait(q, "");
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
                        q.GivenAugments.Add(a.ToAugment(q, Ext));
                    else q.GivenAugments.Add(a.ToAugment(q));
                }
            }

            return q;
        }
    }
}
