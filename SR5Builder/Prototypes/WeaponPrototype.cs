using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Prototypes
{
    public class WeaponPrototype: GearPrototype
    {
        public bool UseStrength { get; set; }

        public string Skill { get; set; }

        public int DV { get; set; }

        public DamageType DamageType { get; set; }

        public int AP { get; set; }

        public decimal RatingAP { get; set; }

        public int Acc { get; set; }

        public override Gear ToGear(SR5Character owner)
        {
            return new Weapon(owner, this);
        }

    }
}
