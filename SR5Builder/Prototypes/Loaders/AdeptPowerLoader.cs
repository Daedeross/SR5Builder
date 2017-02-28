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

        public AdeptPower ToPower(SR5Character owner)
        {
            var power = base.ToPower(owner, Ext);
            power.BaseRating = Base;
            power.ImprovedRating = Improved;
            return power;
        }
    }
}
