using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR5Builder.Prototypes.Loaders
{
    public class QualityLoader: QualityPrototype
    {
        public int Base { get; set; }
        public int Improved { get; set; }
        public string Ext { get; set; }

        public Quality ToQuality(SR5Character c)
        {
            var q = base.ToQuality(c, Ext);
            q.BaseRating = Base;
            q.ImprovedRating = Improved;
            return q;
        }
    }
}
