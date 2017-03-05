using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR5Builder.Prototypes.Loaders
{
    public class GearLoader: GearPrototype
    {
        public string[] AddedMods { get; set; }
        public int Base { get; set; }
        public int Improved { get; set; }

        public GearLoader()
        { }
        public GearLoader(Gear g)
        {
            Name = g.Name;
            Min = g.Min;
            Max = g.Max;
            ModCategories = g.ModCategories.ToArray();
            HasRating = g.HasRating;
            Mods = g.AvailableMods.ToArray();
            BaseMods = g.BaseMods.Keys.ToArray();
            AddedMods = g.Mods.Keys.ToArray();
            Base = g.BaseRating;
            Improved = g.ImprovedRating;
        }

        public override Gear ToGear(SR5Character owner)
        {
            var g = new Gear(owner, this);

            return g;
        }
    }
}
