using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Prototypes
{
    public class RangedWeaponPrototype: WeaponPrototype
    {
        public int AmmoCount { get; set; }

        public FireMode[] FireModes { get; set; }

        public ReloadMethod ReloadMethod { get; set; }

        public int RC { get; set; }

        public override DataModels.Gear ToGear(SR5Character owner)
        {
            return new RangedWeapon(owner, this);
        }
    }
}
