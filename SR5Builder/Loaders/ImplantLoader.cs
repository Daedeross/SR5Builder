using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Loaders
{
    public class ImplantLoader: GearLoader
    {
        public AugmentLoader[] Augments { get; set; }
    }
}