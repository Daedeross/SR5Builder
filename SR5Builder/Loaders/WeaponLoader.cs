using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Loaders
{
    public class WeaponLoader: GearLoader
    {
        public string Skill { get; set; }

        public int DV { get; set; }

        public DamageType DamageType { get; set; }

        public int AP { get; set; }

        public int Acc { get; set; }

        public override Gear ToGear(SR5Character owner)
        {
            return new Weapon(owner, this);
        }

    }
}
