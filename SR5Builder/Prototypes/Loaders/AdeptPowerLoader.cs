using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SR5Builder.DataModels;

namespace SR5Builder.Prototypes.Loaders
{
    public class AdeptPowerLoader: AdeptPowerPrototype
    {
        public int Base { get; set; }
        public int Improved { get; set; }
        public string Ext { get; set; }

        public AdeptPowerLoader()
        { }

        public AdeptPowerLoader(AdeptPower p)
        {
            Name = p.Name;
            Book = p.Book;
            Page = p.Page;
            Min = p.Min;
            Max = p.Max;
            Base = p.BaseRating;
            Improved = p.ImprovedRating;
            Ext = p.Ext;
            Augments = p.GivenAugments.Select(a => new AugmentPrototype
            {
                Bonus = a.BonusArray,
                Kind = a.Kind,
                Target = a.TargetName
            }).ToArray();
        }

        public AdeptPower ToPower(SR5Character owner)
        {
            var power = base.ToPower(owner, Ext);
            power.BaseRating = Base;
            power.ImprovedRating = Improved;
            return power;
        }
    }
}
