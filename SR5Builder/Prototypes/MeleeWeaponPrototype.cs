using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Prototypes
{
    public class MeleeWeaponPrototype: WeaponPrototype
    {
        public int Reach { get; set; }

        public override Gear ToGear(SR5Character owner)
        {
            return new MeleeWeapon(owner, this);
        }
    }
}
